using BLL.DTO;
using BLL.DTO.Cheques;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface
{
    public interface IChequeService
	{
		IAsyncEnumerable<ChequeDTO> GetCheques(int userId);
		Task<ChequeDTO> GetCheque(int userId, int chequeId);
		Task<ChequeDTO> CreateCheque(ChequeLightDTO data);
	}
}
