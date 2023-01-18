namespace library_management_system.Models
{
    public class IssueBookModel
    {
        public string? BookId { get; set; }
        public string? StudentId { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public float RentPrice { get; set; }
    }
}
