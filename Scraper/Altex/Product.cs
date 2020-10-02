namespace Scraper.Altex
{
    internal class Product
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string PhotoLink { get; set; }
        public string ProductLink { get; set; }
        public string NewPrice { get; set; }
        public string OldPrice { get; set; }
    }
}