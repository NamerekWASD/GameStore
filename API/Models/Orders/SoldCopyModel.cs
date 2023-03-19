using API.Models.Copies;

namespace API.Models.Orders
{
    public class SoldCopyModel
    {
        public int Id { get; set; }
        public CopyModel? Copy { get; set; }
		public decimal Price { get; set; }
	}
}
