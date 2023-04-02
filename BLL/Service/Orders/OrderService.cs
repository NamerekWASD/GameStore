using BLL.DTO.Games;
using BLL.DTO.Orders;
using BLL.Tools;
using DAL.Context;
using DAL.Entity.BillingAddresses;
using DAL.Entity.Copies;
using DAL.Entity.Games;
using DAL.Entity.Orders;
using DAL.Entity.SoldCopies;
using DAL.UoW;
using Exceptions;
using Microsoft.Extensions.Logging;
using UnitsOfWork.Interfaces;

namespace BLL.Service.Orders
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork UoW;
#pragma warning disable IDE0052 // Remove unread private members
        private readonly ILogger<OrderService> _logger;
#pragma warning restore IDE0052 // Remove unread private members
        private readonly object locker = new();
        public OrderService(GameContext context, ILogger<OrderService> logger)
        {
            UoW = new UnitOfWork(context);
            _logger = logger;
        }

        public async IAsyncEnumerable<OrderDTO> GetOrders(int userId)
        {
            var user = await UoW.Users.GetAsync(userId) ?? throw new NotFoundException("Користувача не знайдено");
            foreach (var order in user.Orders)
            {
                yield return MapperHelper.Instance.Map<OrderDTO>(order);
            }
        }

        public async Task<OrderDTO?> GetOrder(int userId, int orderId)
        {
            var order = MapperHelper.Instance.Map<OrderDTO>(await UoW.Orders.GetAsync(orderId));

            if (order is null || order.BuyerId != userId)
            {
                return null;
            }
            return order;
        }

        public async Task<int> CreateOrder(OrderLightDTO data)
        {
            var order = new Order
            {
                Buyer = await UoW.Users.GetAsync(data.UserId),
                Copies = new List<SoldCopy>(),
                Created = DateTime.Now,
                Bill = data.BillingAddress?.Id == 0 ?
                MapperHelper.Instance.Map<BillingAddress>(data.BillingAddress) :
                await UoW.BillingAddresses.GetAsync(data.BillId)
            };

            foreach (var game in data.Games)
            {
                await ProcessGameCopies(order, game);
            };

            return (await UoW.Orders.AddAsync(MapperHelper.Instance.Map<Order>(order))).Id;
        }

        public async Task CalculateTotalPrice(OrderLightDTO data)
        {
            decimal sum = 0;

            foreach (var game in await GetGames(data))
            {
                if (game.Price is null) throw new NotSetPrice(string.Format("На цю гру не встановлено ціну.\n{0}: {1}", game.Id, game.Title));
                lock (locker)
                {
                    if (game.DiscountPrice is null)
                    {
                        sum += (int)game.Price;
                        continue;
                    }
                    sum += (decimal)game.DiscountPrice;
                }
            }
            data.TotalPrice = sum;
        }

        private async Task ProcessGameCopies(Order order, GameOrderDTO gameData)
        {
            var game = await UoW.Games.GetAsync(gameData.Id) ?? throw new NotFoundException("Гру не знайдено");
            var copies = game.Copies.Where(copy => !copy.IsSold).Take(gameData.Count);

            if (copies.Count() != gameData.Count)
            {
                throw new ArgumentException("Невірна кількість копій!");
            }

            var tasks = new List<Task>();

            foreach (var copy in copies)
            {
                tasks.Add(CreateSoldCopy(copy, order, game.DiscountPrice ?? game.Price));
            }

            await Task.WhenAll(tasks);

            lock (locker)
            {
                game.SoldCopies += copies.Count();
            }

            await UoW.Games.ModifyAsync(game);
        }

        private async Task CreateSoldCopy(Copy copy, Order order, decimal? price)
		{
			if (price is null) throw new NotSetPrice(string.Format("На цю гру не встановлено ціну.\n{0}: {1}", copy.Game.Id, copy.Game.Title));
			var soldCopy = new SoldCopy
            {
                CopyId = copy.Id,
                Price = (decimal)price
            };
            order.Copies?.Add(soldCopy);

            copy.IsSold = true;
            await UoW.Copies.ModifyAsync(MapperHelper.Instance.Map<Copy>(copy));
        }

        private async Task<List<Game>> GetGames(OrderLightDTO data)
        {
            var games = new List<Game>();
            foreach (var game in data.Games)
            {
                var fromDb = await UoW.Games.GetAsync(game.Id) ?? throw new NotFoundException("Гру не знайдено");
                games.Add(fromDb);
            }
            return games;
        }

    }
}
