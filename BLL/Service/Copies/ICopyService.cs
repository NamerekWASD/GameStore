using BLL.DTO.Games;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Service.Copies
{
    public interface ICopyService
    {
        Task<bool> CreateCopy(string data, int gameId);
    }
}
