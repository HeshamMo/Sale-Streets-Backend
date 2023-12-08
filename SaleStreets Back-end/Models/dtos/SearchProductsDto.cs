namespace SaleStreets_Back_end.Models.dtos
{
    public class SearchProductsDto
    {
        public int Id { get; set; }

        public DateTime PublishedAt { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }

    }
}
