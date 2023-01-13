using library_management_system.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using library_management_system.Controllers;
using library_management_system.Controllers.Logic;
using System.Text.Json.Serialization;
using System.Text.Json;
using Newtonsoft.Json;

namespace library_management_system.DatabaseLayer
{
    public class DbLibrarian : LogicLibrarian
    {
        
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da;
        DataSet ds;
        string sql_query;
        string connectionString = "";


        public async Task<string> DbGetAllBooks() 
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SpGetAllBooks", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        SqlDataAdapter da = new SqlDataAdapter();
                        DataTable dt = new DataTable();
                        da.Fill(dt);
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

        public async Task<string> DbAddBook(BookDetails book)
        {
            try {
                using (SqlConnection con = new SqlConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("SpAddBook", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
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
                        return "Book added successfully";
                    }
                }    
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> DbUpdateBook(BookDetails book)
        {
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("SpUpdateBook", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
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
                        return "Book Updeted successfully";
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> DbDeleteBook(string bookId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("SpDeleteBook", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
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

        public async Task<string> DbIssuedBooks()
        {
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("SpIssuedBooks", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
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

        public async Task<string> DbDueBooks()
        {
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("SpDueBooks", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
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
