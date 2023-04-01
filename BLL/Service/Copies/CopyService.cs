using BLL.DTO.Games;
using BLL.Tools;
using DAL.Context;
using DAL.Entity.Copies;
using DAL.UoW;
using Microsoft.Extensions.Logging;
using UnitsOfWork.Interfaces;

namespace BLL.Service.Copies
{
    public class CopyService : ICopyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CopyService> _logger;
        public CopyService(GameContext context,
            ILogger<CopyService> logger)
        {
            _unitOfWork = new UnitOfWork(context);
            _logger = logger;
        }

        public async Task<bool> CreateCopy(string data, int gameId)
        {
            Copy newCopy = new()
            {
                Data = data,
                Game = await _unitOfWork.Games.GetAsync(gameId),
                IsSold = false,
            };
            try
            {
                await _unitOfWork.Copies.AddAsync(newCopy);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                _logger.LogError("Error occure in Create Copy method with error: {message}", message);
                return false;
            }
            return true;
        }
    }
}
