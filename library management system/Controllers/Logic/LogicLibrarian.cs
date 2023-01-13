using library_management_system.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Text.Json;
using library_management_system.Controllers;
using library_management_system.DatabaseLayer;

namespace library_management_system.Controllers.Logic
{
    public class LogicLibrarian: BookController
    {

        //     public async virtual Task<string> DbGetAllBooks() { return ""; }
        DbLibrarian librarian = new DbLibrarian();
        public async Task<string> GetBooksLogic()
        {
            string result = await librarian.DbGetAllBooks();
            return result;
        }

        public async Task<string> AddBookLogic(string jsonFile)
        {
            BookDetails book = JsonSerializer.Deserialize<BookDetails>(jsonFile);
            string result = await librarian.DbAddBook(book);
            return result;

        }

        public async Task<string> UpdateBookLogic(string jsonFile)
        {
            BookDetails book = JsonSerializer.Deserialize<BookDetails>(jsonFile);
            string result = await librarian.DbUpdateBook(book);
            return result;

        }

        public async Task<string> DeleteBookLogic(string jsonFile)
        {
            dynamic book = JsonSerializer.Deserialize<dynamic>(jsonFile);
            string BookId = book["BookId"];
            string result = await librarian.DbDeleteBook(BookId);
            return result;

        }

        public async Task<string> IssuedBooksLogic()
        {
            string result = await librarian.DbIssuedBooks();
            return result;
        //    BookDetails books = JsonSerializer.Deserialize<BookDetails>(result);
        //    return books;
        }

        public async Task<string> DueBooksLogic()
        {
            string result = await librarian.DbDueBooks();
            return result;
        }
    }
}
