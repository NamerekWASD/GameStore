using DAL.Entity;

namespace BLL.DTO
{
	public class LoginDTO
	{
		public string FirstData { get; set; } = string.Empty;
		public string Password { get; set; } = string.Empty;
		// override object.Equals
		public override bool Equals(object? obj)
		{
			if (obj is null)
			{
				return false;
			}

			if (GetType() == obj.GetType())
			{
				var login = (LoginDTO)obj;
				if (login.FirstData == FirstData
				&& login.Password == Password)
				{
					return true;
				}
				else
				{
					return false;
				}
			}

			if (typeof(User) == obj.GetType())
			{
				var user = (User)obj;
				if (user.UserName == FirstData
				&& user.Email == FirstData
				&& user.PhoneNumber == FirstData
				&& user.PasswordHash == Password)
				{
					return true;
				}
				else
				{
					return false;
				}
			}

			return false;
		}

		public override int GetHashCode()
		{
			return 1;
		}
	}
}