namespace library_management_system.Models
{
    public class LogTable
    {
        public int? RentalId { get; set; }
        public string? BookId { get; set; }
        public string? StudentId { get; set; }
        public float? RentTotal { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public int? HasReturned { get; set; }
        public float? Penalty { get; set; }
    }
}
