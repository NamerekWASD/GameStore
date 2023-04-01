using DAL.Entity.Games;
using Microsoft.EntityFrameworkCore;

namespace DAL.Context
{
	public class MockGameContext : GameContext
	{
		public MockGameContext() : base()
		{

		}
	}
}
