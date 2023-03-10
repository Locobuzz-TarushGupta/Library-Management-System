using library_management_system.DatabaseLayer;
using library_management_system.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace library_management_system.Controllers.Logic
{
    public class LogicStudent
    {
        DbStudent student = new DbStudent();
        public static int rentalId = 1;
        public string GetBooksLogic()
        {
            string result = student.GetAllBooksStudentDb();
            return result;
        }

        [HttpGet, HttpPost]
        public async Task<string> SearchBookLogic(string jsonFile)
        {
            dynamic data = JsonConvert.DeserializeObject<dynamic>(jsonFile);
            string Stream = data["Stream"];
            string Medium = data["Medium"];
            string BookName = data["Name"];
            string result = student.SearchBookDb(Stream, Medium, BookName);
            return result;
        }

        [HttpPost, HttpPut]
        public async Task<string> IssueBookLogic(string jsonFile) 
        {
            dynamic data = JsonConvert.DeserializeObject<dynamic>(jsonFile);
            string bookID = data["BookId"];
            string studentID = data["StudentId"];
            string result = student.IssueBookDb(bookID, studentID);
            if (result != "Book is Not-Available")
            {
                return result.ToString();
            }
            string rentalDetails= student.LogTableEntryDb(rentalId,data);
            rentalId++;
            return rentalDetails;
        }

        [HttpPost]
        public async Task<string> ReturnBookLogic(string jsonFile)
        {
            dynamic data = JsonConvert.DeserializeObject<dynamic>(jsonFile);
            string BookRentalID = data["rentalId"];
            string bookId = data["bookId"];
            string result = student.ReturnBookDb(BookRentalID,bookId);
            return result;
        }
    }
}
