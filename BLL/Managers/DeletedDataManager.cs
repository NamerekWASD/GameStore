namespace DAL.Managers
{
	public class DeletedDataManager
	{
		public static string DeletedDataDirectory => Path.Combine(AppContext.BaseDirectory, "Deleted data");

		public static void EnsureDeletedDataDirectoryExists()
		{
			if (!Directory.Exists(DeletedDataDirectory))
			{
				Directory.CreateDirectory(DeletedDataDirectory);
			}
		}
	}
}