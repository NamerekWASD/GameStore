using BLL.DTO.Orders;
using BLL.Tools;
using DAL.Context;
using DAL.Entity;
using DAL.UoW;
using Exceptions;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
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
                yield return MapperHelpers.Instance.Map<OrderDTO>(order);
            }
        }

        public async Task<OrderDTO?> GetOrder(int userId, string orderNumber)
        {
            var user = (await UoW.Users.GetAsync(userId) ?? throw new NonAuthorizedException("Ви не авторизувались!"));

            var order = MapperHelpers.Instance.Map<OrderDTO>(user
                .Orders.FirstOrDefault(item => item.OrderNumber.Equals(orderNumber)));
            return order;
        }

        public async Task<string> CreateOrder([NotNull] OrderLightDTO data)
        {
            await CalculateTotalPrice(data);
            var order = new Order
            {
                Buyer = await UoW.Users.GetAsync(data.UserId),
                Copies = new List<SoldCopy>(),
                Created = DateTime.Now,
                OrderNumber = await GenerateOrderNumber(),
                Total = data.Total,
                SubTotal = data.SubTotal,
            };

            if (data.BillingAddress is null || data.BillingAddress.Id is 0)
            {
                order.Bill = MapperHelpers.Instance.Map<BillingAddress>(data.BillingAddress);
            }
            else
            {
                order.BillingAddressId = data.BillingAddress.Id;
            }

            foreach (var game in data.Games)
            {
                await ProcessGameCopies(order, game);
            };

            await UoW.Orders.AddAsyncNoSave(MapperHelpers.Instance.Map<Order>(order));
            return order.OrderNumber;
        }

        private async Task<string> GenerateOrderNumber()
        {
            string orderNumber = "";
            var lastOrder = (await UoW.Orders.GetAllAsync()).OrderByDescending(o => o.Id).FirstOrDefault();

            int nextId = (lastOrder?.Id ?? 0) + 1;
            int maxOrderNumberLength = 10;
            int remainingDigits = maxOrderNumberLength - nextId.ToString().Length;
            char paddingChar = '0';
            orderNumber = nextId.ToString().PadLeft(maxOrderNumberLength, paddingChar);

            return orderNumber;
        }

        private async Task CalculateTotalPrice([NotNull] OrderLightDTO data)
        {
            decimal total = 0;
            decimal subTotal = 0;

            foreach (var game in await GetGames(data))
            {
                decimal discount = game.DiscountPrice ?? game.Price ?? throw new NotSetPrice(string.Format("На цю гру не встановлено ціну.\n{0}: {1}", game.Id, game.Title));
                decimal price = game.Price ?? 0;
                int count = data.Games.FirstOrDefault(item => item.Id == game.Id)?.Count ?? 1;
                lock (locker)
                {
                    total += discount * count;
                    subTotal += price * count;
                }
            }
            data.Total = total;
            data.SubTotal = subTotal;
        }

        private async Task ProcessGameCopies([NotNull] Order order, [NotNull] GameOrderDTO gameData)
        {
            var game = await UoW.Games.GetAsync(gameData.Id) ?? throw new NotFoundException("Гру не знайдено");
            var copies = game.Copies.Where(copy => !copy.IsSold).Take(gameData.Count);

            if (copies.Count() != gameData.Count)
            {
                throw new ArgumentException("Невірна кількість копій!");
            }

            foreach (var copy in copies)
            {
                var price = game.DiscountPrice ?? game.Price ?? throw new NotSetPrice(string.Format("На гру \"{0}\" не встановлено ціну!", copy.Game?.Title));

                CreateSoldCopy(copy, order, price);
            }

            lock (locker)
            {
                game.SoldCopies += copies.Count();
            }

            UoW.Games.ModifyNoSave(game);
        }

        private void CreateSoldCopy([NotNull] Copy copy, [NotNull] Order order, decimal? price)
        {
            if (price is null) throw new NotSetPrice(string.Format("На цю гру не встановлено ціну.\n{0}: {1}", copy.Game?.Id, copy.Game?.Title));
            var soldCopy = new SoldCopy
            {
                CopyId = copy.Id,
                Price = (decimal)price
            };
            order.Copies?.Add(soldCopy);

            copy.IsSold = true;
            UoW.Copies.ModifyNoSave(copy);
        }

        private async Task<List<Game>> GetGames([NotNull] OrderLightDTO data)
        {
            var games = new List<Game>();
            foreach (var game in data.Games)
            {
                var fromDb = await UoW.Games.GetAsync(game.Id) ?? throw new NotFoundException("Гру не знайдено");
                games.Add(fromDb);
            }
            return games;
        }

        public async Task CommitChanges()
        {
            await UoW.CommitChanges();
        }
    }
}