using library_management_system.Controllers.Logic;
using Microsoft.AspNetCore.Mvc;

namespace library_management_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController
    {
        public StudentController() { }

        LogicStudent student = new LogicStudent();
        LogicLibrarian librarian= new LogicLibrarian();

        [HttpGet]
        public async Task<string> GetBooks()
        {
            try
            {
                string result = librarian.GetBooksLogic().ToString();
                return result;

            }
            catch (Exception ex) { }
            {
                return "Books could not be fetched.";
            }
        }

        public async Task<string> IssueBook(string jsonFile)
        {
            string result = await student.IssueBookLogic(jsonFile);
            return result;
        }

        public async Task<string> ReturnBook(string jsonFile)
        {
            string result = await student.ReturnBookLogic(jsonFile);
            return result;
        }

        public async Task<string> SearchBook(string jsonFile)
        {
            string result = await student.SearchBookLogic(jsonFile);
            return result;
        }

    }
}
