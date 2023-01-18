namespace library_management_system.Models
{
    public class BookDetails
    {
        public string? BookId { get; set; }
        public string? BookTitle { get; set; }
        public string? BookDescription { get; set; }
        public string? Author { get; set; }
        public string? Stream { get; set; }
        public int Quantity { get; set; }
        public float RentPrice { get; set; }
        public string? Status { get; set; }
    }
}
