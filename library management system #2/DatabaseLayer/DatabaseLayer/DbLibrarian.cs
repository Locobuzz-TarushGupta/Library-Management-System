using library_management_system.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using library_management_system.Controllers;
using library_management_system.Controllers.Logic;
using System.Text.Json.Serialization;
using System.Text.Json;
using Newtonsoft.Json;
using System.Net;

namespace library_management_system.DatabaseLayer
{
    public class DbLibrarian
    {

        //SqlConnectionStringBuilder sqlcon = new SqlConnectionStringBuilder("Data Source=\"(localdb)\\Library system\";Initial Catalog=\"Library Management\";Integrated Security=True");
        string connectionString = "Data Source=\"(localdb)\\Library system\";Initial Catalog=\"Library Management\";Integrated Security=True";

        [HttpGet]
        public async Task<string> DbGetAllBooks() 
        {
            try
            {

                /*
                var db = new MyDataContext(connectionString);
                IEnumerable<BookDetails> books = from book in db.BookDetail
                                                 where book.Quantity > 0
                                                 select book;
                string result = string.Empty;
                result = JsonConvert.SerializeObject(books);
                return result;
                */
              

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("SELECT * from BookDetails where Quantity > 0", con);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        SqlDataAdapter da = new SqlDataAdapter();
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        con.Close();
                        string result = string.Empty;
                        if(dt.Rows.Count > 0) result = JsonConvert.SerializeObject(dt);
                        return result;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [HttpPost]
        public async Task<string> DbAddBook(BookDetails book)
        {
            try {
                string sqlQuery = "INSERT INTO BookDetail (BookId,BookTitle,BookDescription,Author,Stream,Quantity,RentPrice,Status,Medium)    VALUES (@BookId,@BookTitle,@BookDescription,@Author,@Stream,@Quantity,@RentPrice,@Status,@Medium)";

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@BookId", book.BookId);
                        cmd.Parameters.AddWithValue("@BookTitle", book.BookTitle);
                        cmd.Parameters.AddWithValue("@BookDescription", book.BookDescription);
                        cmd.Parameters.AddWithValue("@Author", book.Author);
                        cmd.Parameters.AddWithValue("@Medium", book.Medium);
                        cmd.Parameters.AddWithValue("@Stream", book.Stream);
                        cmd.Parameters.AddWithValue("@Quantity", book.Quantity);
                        cmd.Parameters.AddWithValue("@RentPrice", book.RentPrice);
                        cmd.Parameters.AddWithValue("@Status", book.Status);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                        return "Book added successfully";
                    }
                }    
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        [HttpPut]
        public async Task<string> DbUpdateBook(BookDetails book)
        {
            try
            {
                string sqlQuery = "UPDATE BookDetail SET BookDetail.BookId = @BookId,      BookDetail.BookTitle = @BookTitle,BookDetail.BookDescription = @BookDescription,       BookDetail.Author = @Author,      BookDetail.Stream = @Stream,      BookDetail.Quantity = @Quantity,      BookDetail.RentPrice = @RentPrice,      BookDetail.Status = @Status WHERE BookDetail.BookId = @BookId";

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@BookId", book.BookId);
                        cmd.Parameters.AddWithValue("@BookTitle", book.BookTitle);
                        cmd.Parameters.AddWithValue("@BookDescription", book.BookDescription);
                        cmd.Parameters.AddWithValue("@Author", book.Author);
                        cmd.Parameters.AddWithValue("@Stream", book.Stream);
                        cmd.Parameters.AddWithValue("@Quantity", book.Quantity);
                        cmd.Parameters.AddWithValue("@RentPrice", book.RentPrice);
                        cmd.Parameters.AddWithValue("@Status", book.Status);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                        return "Book Updated successfully";
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [HttpDelete]
        public async Task<string> DbDeleteBook(string bookId)
        {
            try
            {
                string sqlQuery = "DELETE from BookDetails where BookId = @BookId";

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@BookId", bookId);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                        return "Book Deleted successfully";
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [HttpPost]
        public async Task<string> DbIssuedBooks()
        {
            try
            {
                string sqlQuery = "SELECT * from LogTable where HasReturned = 0";

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        con.Close();
                        string result = string.Empty;
                        result = JsonConvert.SerializeObject(dt);
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [HttpPost]
        public async Task<string> DbDueBooks()
        {
            try
            {
                string sqlQuery = "select * from LogTable where (LogTable.ReturnDate < @currentDate) AND (LogTable.HasReturned = 0)";
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@currentDate", DateTime.Today);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        con.Close();
                        string result = string.Empty;
                        result = JsonConvert.SerializeObject(dt);
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
