namespace BLL.Service.Copies
{
	public interface ICopyService
	{
		Task<bool> CreateCopy(string data, int gameId);
	}
}