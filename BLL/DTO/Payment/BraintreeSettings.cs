using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO.Payment
{
	public class BraintreeSettings
	{
		public string Environment { get; set; } = string.Empty;
		public string MerchantId { get; set; } = string.Empty;
		public string PublicKey { get; set; } = string.Empty;
		public string PrivateKey { get; set; } = string.Empty;

	}
}
