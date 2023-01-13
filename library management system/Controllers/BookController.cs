using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using library_management_system.Models;
using System.Text.Json.Serialization;
using System.Text.Json;
using library_management_system.Controllers.Logic;
using Microsoft.Extensions.Configuration;

namespace library_management_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController: ControllerBase
    {
       /* private readonly IConfiguration _config;
        public string connectionString = "";
        public BookController(IConfiguration config)
        {
            _config = config;
            connectionString = _config.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value;
        }
       */


        LogicLibrarian librarian = new LogicLibrarian();

        [HttpGet]
        public async Task<string> GetBooks()
        {
            try 
            {
                string result = librarian.GetBooksLogic().ToString();
                return result;
                
            }
            catch(Exception ex) { }
            {
                return "Books could not be fetched.";
            }
        }

        [HttpPost]

        public async Task<string> AddBook(string jsonFile)
        {
            try
            {
                var result = await librarian.AddBookLogic(jsonFile);
                return result;
            }
            catch(Exception ex) 
            {
                return ex.Message;
            }
        }

        public async Task<string> UpdateBook(string jsonFile)
        {
            try
            {
                var result = await librarian.UpdateBookLogic(jsonFile);
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> DeleteBookLogic(string jsonFile)
        {
            try
            {
                string result = await librarian.DeleteBookLogic(jsonFile);
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }


        }


        [HttpGet]
        public async Task<string> IssuedBooks()
        {
            try
            {
                string result = await librarian.IssuedBooksLogic();
                return result;
            }
            catch(Exception ex) 
            {
                return ex.Message;    
            }
        }
    }
}
