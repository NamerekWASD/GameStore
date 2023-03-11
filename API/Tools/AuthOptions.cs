using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace API.Tools
{
	public class AuthOptions
	{
		public const string ISSUER = "MyAuthServer";
		public const string AUDIENCE = "MyAuthClient";
		private const string KEY = "mysupersecret_secretkey!123";

		public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
			new(Encoding.UTF8.GetBytes(KEY));
	}
}