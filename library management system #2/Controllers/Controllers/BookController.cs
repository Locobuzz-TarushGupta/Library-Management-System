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
using Microsoft.AspNetCore.Authorization;

namespace library_management_system.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "Librarian")]
    public class BookController: ControllerBase
    {
        
        private string connectionString = "Data Source=\"(localdb)\\Library system\";Initial Catalog=\"Library Management\";Integrated Security=True";
        
        /*private readonly IConfiguration _config;
        public BookController(IConfiguration config)
        {
            _config = config;
            connectionString = _config.GetValue<string>("ConnectionStrings:DefaultConnection");
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

        [HttpGet]
        public async Task<string> GetIssuedBooks()
        {
            try
            {
                string result = await librarian.IssuedBooksLogic();
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
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

        [HttpPut]
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

        [HttpDelete]
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


        
    }
}
