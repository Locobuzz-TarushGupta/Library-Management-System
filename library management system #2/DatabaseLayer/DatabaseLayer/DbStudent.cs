using library_management_system.Controllers.Logic;
using library_management_system.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Net;

namespace library_management_system.DatabaseLayer
{
    public class DbStudent
    {
        public static int RentalId = 1;
        string connectionString = "";

        [HttpGet]
        public string GetAllBooksStudentDb()
        {
            try
            {
                string sqlQuery = "SELECT * from BookDetails where Quantity > 0";
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
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

        [HttpPost]
        public string SearchBookDb(string Stream, string Medium, string BookTitle)
        {
            try
            {
                string sqlQuery = "SELECT * FROM BookDetail WHERE (@stream IS NULL OR Stream = @stream) AND (@bookTitle IS NULL OR BookTitle = @bookTitle) AND (@medium IS NULL OR Medium = @medium)";
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (Stream != "") cmd.Parameters.AddWithValue("@stream", Stream);
                        if (Medium != "") cmd.Parameters.AddWithValue("@medium", Medium);
                        if (BookTitle != "") cmd.Parameters.AddWithValue("@bookTitle", BookTitle);
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

        [HttpGet]
        DataTable GetBookDetails(string bookId)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM BookDetail WHERE BookId = @bookId", con))
                {
                    cmd.Parameters.AddWithValue("@bookId", bookId);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.Fill(dt);
                    con.Close();
                }
            }
            return dt;
        }

        [HttpGet]
        DataTable GetLogEntry(string rentalId)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SpGetBookDetails", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@rentalId", rentalId);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.Fill(dt);
                    con.Close();
                }
            }
            return dt;
        }

        [HttpGet, HttpPost]
        public string LogTableEntryDb(int rentalId, IssueBookModel data)
        {
            try
            {
                DataTable books = GetBookDetails(data.BookId);
                float RentPrice = (float)books.Rows[0][6];
                DateTime date1 = data.IssueDate;
                DateTime date2 = data.ReturnDate;
                TimeSpan time = date2.Subtract(date1);
                int RentDays = (int)time.TotalDays;
                string sqlQuery = "INSERT INTO BookDetail (RentalId,BookId,StudentId,IssueDate,ReturnDate,RentTotal,HasReturned,Penalty)    VALUES (@RentalId,@BookId,@StudentId,@IssueDate,@ReturnDate,@RentTotal,@HasReturned,@Penalty)";
                
                using (SqlConnection con = new SqlConnection())
                {
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@RentalId", rentalId);
                        cmd.Parameters.AddWithValue("@BookId", data.BookId);
                        cmd.Parameters.AddWithValue("@StudentId", data.StudentId);
                        cmd.Parameters.AddWithValue("@IssueDate", data.IssueDate);
                        cmd.Parameters.AddWithValue("@ReturnDate", data.ReturnDate);
                        cmd.Parameters.AddWithValue("@RentTotal", RentDays * RentPrice);
                        cmd.Parameters.AddWithValue("@HasReturned", 0);
                        cmd.Parameters.AddWithValue("@Penalty", 0);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                        return "Log added successfully.";
                    }
                }

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [HttpGet,HttpPost,HttpPut]
        public string IssueBookDb(string BookId, string StudentId)
        {

            DataTable dt = GetBookDetails(BookId);
            int quantity = 0;
            if (dt.Rows.Count == 0 || (int)dt.Rows[0][5] == 0) return "Book is Not-Available";
            string sqlQuery = "SELECT Quantity FROM BookDetail WHERE BookId = @bookId";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                
                
                sqlQuery = String.Format("UPDATE BookDetail SET Quantity = {0} WHERE BookId = @bookId",quantity - 1);
                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    cmd.Parameters.AddWithValue("@BookId", BookId);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    IssueBookModel issueBookDetails = new IssueBookModel()
                    {
                        BookId = BookId,
                        StudentId = StudentId,
                        IssueDate = DateTime.Now,
                        ReturnDate = DateTime.Now,
                        RentPrice = (float)dt.Rows[0][5]
                    };
                    LogTableEntryDb(RentalId, issueBookDetails);
                    RentalId = RentalId + 1;
                    return "Book Issued successfully. Rental ID" + (RentalId - 1);
                }
            }
        }

        [HttpPost]
        public string ReturnBookDb(string BookRentalId,string bookId)
        {
            try
            {
                DataTable logEntry = GetLogEntry(BookRentalId);
                DataTable book = GetBookDetails(bookId);
                DateTime ReturnDate = (DateTime)logEntry.Rows[0][5];
                DateTime IssueDate = (DateTime)logEntry.Rows[0][4];
                float penalty = 0.0f;

                if(DateTime.Today > ReturnDate)
                {
                    TimeSpan time = DateTime.Today.Subtract(ReturnDate);
                    int RentDays = (int)time.TotalDays;
                    penalty = RentDays * (float)book.Rows[0][6];
                }
                int quantity = (int)book.Rows[0][5];
                string sqlQuery = String.Format("UPDATE BookDetail,LogTable SET BookDetail.Quantity = {0},LogTable.HasReturned = 1 WHERE BookDetail.BookId = @bookId AND LogTable.RentalId = @RentalId",quantity-1);
                
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@bookId", bookId);
                        cmd.Parameters.AddWithValue("@RentalId", BookRentalId);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        SqlDataAdapter da = new SqlDataAdapter();
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        con.Close();
                    }
                }

                if (penalty == 0.0f)
                {
                    return "Thankyou for the return.";
                } 
                else
                {
                    return "Please pay fine rupees" + penalty.ToString();
                }
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

    }
}
