using library_management_system.Controllers.Logic;
using Microsoft.AspNetCore.Mvc;

namespace library_management_system.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StudentController
    {
        
        public string connectionString = "Data Source=\"(localdb)\\Library system\";Initial Catalog=\"Library Management\";Integrated Security=True";
        
        /*private readonly IConfiguration _config;
        public StudentController(IConfiguration config)
        {
            _config = config;
            connectionString = _config.GetValue<string>("ConnectionStrings:DefaultConnection");
        }*/

        LogicStudent student = new LogicStudent();
        LogicLibrarian librarian= new LogicLibrarian();

        [HttpGet]
        public string GetBooks()
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

        [HttpPost]
        public async Task<string> IssueBook(string jsonFile)
        {
            string result = await student.IssueBookLogic(jsonFile);
            return result;
        }

        [HttpPost]
        public async Task<string> ReturnBook(string jsonFile)
        {
            string result = await student.ReturnBookLogic(jsonFile);
            return result;
        }

        [HttpGet]
        public async Task<string> SearchBook(string jsonFile)
        {
            string result = await student.SearchBookLogic(jsonFile);
            return result;
        }

    }
}
