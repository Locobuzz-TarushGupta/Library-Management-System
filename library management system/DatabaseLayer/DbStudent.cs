using library_management_system.Models;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Net;

namespace library_management_system.DatabaseLayer
{
    public class DbStudent
    {
        string connectionString = "";
        public string GetAllBooksStudentDb()
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

        public string SearchBookDb(string Stream, string Medium, string BookName)
        {
            try
            {

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SpSearchBook", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (Stream != "") cmd.Parameters.AddWithValue("@stream", Stream);
                        if (Medium != "") cmd.Parameters.AddWithValue("@medium", Medium);
                        if (BookName != "") cmd.Parameters.AddWithValue("@bookName", BookName);
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

        public string IssueBookDb(string bookId)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SpGetBookQuantity", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    SqlDataAdapter da = new SqlDataAdapter();
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count == 0) return "Book is Not-Available";
                    con.Close();
                }

                using (SqlCommand cmd = new SqlCommand("SpIssueBook", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BookId", bookId);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    string result = "Book Issued Successfully.";
                    return result;
                }
            }
        }

        DataTable GetBookDetails(string bookId)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SpGetBookDetails", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BookId", bookId);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.Fill(dt);                       
                    con.Close();
                }
            }
            return dt;
        }

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

        public string LogTableEntryDb(int rentalId,IssueBookModel data)
        {
            try
            {
                DataTable dt = GetBookDetails(data.BookId);
                float RentPrice = (float)dt.Rows[0][6];
                DateTime date1 = data.IssueDate;
                DateTime date2 = data.ReturnDate;
                TimeSpan time = date2.Subtract(date1);
                int RentDays = (int)time.TotalDays;
                LogTable log = new LogTable()
                {
                    RentalId = rentalId,
                    BookId = data.BookId,
                    StudentId = data.StudentId,
                    IssueDate = data.IssueDate,
                    ReturnDate = data.ReturnDate,
                    RentTotal = RentDays * RentPrice,
                    HasReturned = 0,
                    Penalty = 0
                };
                string result = string.Empty;
                result = JsonConvert.SerializeObject(log);
                return result;
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        public string ReturnBookDb(string BookRentalId,string bookId)
        {
            try
            {
                DataTable logEntry = GetLogEntry(BookRentalId);
                DateTime ReturnDate = (DateTime)logEntry.Rows[0][5];
                float penalty = 0.0f;
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SpReturnBook", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
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
