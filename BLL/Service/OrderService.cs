using BLL.DTO.Games;
using BLL.DTO.Orders;
using BLL.Interface;
using BLL.Tools;
using DAL.Context;
using DAL.Entity.Games;
using DAL.Entity.Orders;
using DAL.UoW;
using UnitsOfWork.Interfaces;

namespace BLL.Service
{
	public class OrderService : IOrderService
	{
		private readonly IUnitOfWork UoW;
		private readonly object locker = new();
		public OrderService(GameContext context)
		{
			UoW = new UnitOfWork(context);
		}

		public async IAsyncEnumerable<OrderDTO> GetOrders(int userId)
		{
			foreach (var order in (await UoW.Users.GetAsync(userId)).Orders)
			{
				yield return MapperHelper.Instance.Map<OrderDTO>(order);
			}
		}

		public async Task<OrderDTO?> GetOrder(int userId, int orderId)
		{
			var order = MapperHelper.Instance.Map<OrderDTO>(await UoW.Orders.GetAsync(orderId));

			if (order is null || order.Buyer.Id != userId)
			{
				return null;
			}
			return order;
		}

		public async Task<int> CreateOrder(OrderLightDTO data)
		{
			var games = await GetGames(data);

			var order = new OrderDTO()
			{
				Buyer = await UoW.Users.GetAsync(data.UserId),
				Copies = new List<SoldCopyDTO>(),
				Created = DateTime.Now,
			};

			if (data.BillingAddress?.Id == 0)
			{
				order.Bill = data.BillingAddress;
			}
			else
			{
				order.BillId = data.BillId;
			}

			foreach (var game in games)
			{
				var dataGame = data.Games?.Find(element => element.Id == game.Id);
				await ProcessGameCopies(order, game, dataGame);
			};

			return (await UoW.Orders.AddAsync(MapperHelper.Instance.Map<Order>(order))).Id;
		}

		public async Task CalculateTotalPrice(OrderLightDTO data)
		{
			decimal sum = 0;

			foreach (var game in await GetGames(data))
			{
				lock (locker)
				{
					if (game.DiscountPrice is null)
					{
						sum += game.Price;
						continue;
					}
					sum += (decimal) game.DiscountPrice;
				}
			}
			data.TotalPrice = sum;
		}

		private async Task ProcessGameCopies(OrderDTO order, GameDTO game, GameOrderDTO dataGame)
		{
			var copies = game.Copies.Where(copy => !copy.IsSold).Take(dataGame.Count);
			if (copies.Count() != dataGame.Count)
			{
				throw new Exception("Wrong numbers of copy!");
			}
			foreach (var copy in copies)
			{
				await CreateSoldCopy(copy, order);
			}
			lock (locker)
			{
				game.SoldCopies += copies.Count();
			}
			await UoW.Games.ModifyAsync(game.Id, MapperHelper.Instance.Map<Game>(game));
		}

		private async Task CreateSoldCopy(CopyDTO copy, OrderDTO order)
		{
			var soldCopy = new SoldCopyDTO() { CopyId = copy.Id };

			if(copy.Game.DiscountPrice is null)
			{
				soldCopy.Price = copy.Game.Price;
			}
			else
			{
				soldCopy.Price = (decimal) copy.Game.DiscountPrice;
			}

			order.Copies?.Add(soldCopy);

			copy.IsSold = true;
			await UoW.Copies.ModifyAsync(copy.Id, MapperHelper.Instance.Map<Copy>(copy));
		}

		private async Task<List<GameDTO>> GetGames(OrderLightDTO data)
		{
			var games = new List<GameDTO>();
			foreach (var game in data.Games)
			{
				games.Add(MapperHelper.Instance.Map<GameDTO>(await UoW.Games.GetAsync(game.Id)));
			}
			return games;
		}

	}
}
