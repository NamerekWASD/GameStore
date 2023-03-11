namespace API.Models.Games
{
    public class GameLightModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ImageURL { get; set; } = string.Empty;
        public string Platform { get; set; } = string.Empty;
        public string CopyType { get; set; } = string.Empty;
		public int CopyCount { get; set; }
		public decimal Price { get; set; }
    }
}
