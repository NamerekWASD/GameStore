using BLL.DTO;
using BLL.DTO.Cheques;
using BLL.DTO.Games;
using BLL.Interface;
using BLL.Tools;
using DAL.Context;
using DAL.Entity;
using DAL.Entity.Cheques;
using DAL.Entity.Games;
using DAL.UoW;
using Exceptions;
using UnitsOfWork.Interfaces;

namespace BLL.Service
{
    public class ChequeService : IChequeService
	{
		private readonly IUnitOfWork UoW;
		public ChequeService(GameContext context)
		{
			UoW = new UnitOfWork(context);
		}

		public async IAsyncEnumerable<ChequeDTO> GetCheques(int userId)
		{
			foreach (var cheque in (await UoW.Users.GetAsync(userId)).Cheques)
			{
				yield return Mapper.Instance.Map<ChequeDTO>(cheque);
			}
		}

		public async Task<ChequeDTO> GetCheque(int userId, int chequeId)
		{
			throw new NotImplementedException();
		}

		public async Task<ChequeDTO> CreateCheque(ChequeLightDTO data)
		{
			var games = await GetGames(data);

			var cheque = new ChequeDTO()
			{
				Buyer = await UoW.Users.GetAsync(data.UserId),
				Bill = data.BillingAddress,
				Games = new List<SoldCopyDTO>(),
				Created = DateTime.Now,
			};
			foreach (var game in games)
			{
				var dataGame = data.Games?.Find(element => element.Id == game.Id);
				await ProcessGameCopies(cheque, game, dataGame);

			}
			return cheque;
		}

		private async Task ProcessGameCopies(ChequeDTO cheque, GameDTO game, GameChequeDTO? dataGame)
		{
			foreach (var copy in game.Copies.Where(copy => !copy.IsSold).Take(dataGame.Count))
			{
				await CreateSoldCopy(copy, cheque);

				copy.IsSold = true;
				await UoW.Copies.ModifyAsync(copy.Id, Mapper.Instance.Map<Copy>(copy));
			}
		}

		private async Task CreateSoldCopy(CopyDTO copy, ChequeDTO cheque)
		{
			var soldCopy = new SoldCopyDTO() { CopyId = copy.Id, Cheque = cheque, Price = copy.Game.Price };
			await UoW.SoldCopies.AddAsync(
				Mapper.Instance.Map<SoldCopy>(soldCopy));
			soldCopy.Copy = copy;
			cheque.Games.Add(soldCopy);
		}

		private async Task<List<GameDTO>> GetGames(ChequeLightDTO data)
		{
			var games = new List<GameDTO>();
			foreach (var game in data.Games)
			{
				games.Add(Mapper.Instance.Map<GameDTO>(await UoW.Games.GetAsync(game.Id)));
			}
			return games;
		}
	}
}
