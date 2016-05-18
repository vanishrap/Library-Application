using LibraryApplication.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LibraryApplication.DataAccessObjects
{
    public class UserDAO : DefaultDAO
    {
        public Reader GetReaderInfo(int readerID)
        {

            Reader r = new Reader();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                string sql = "EXEC ReadReader @ID = " + readerID;
                SqlCommand cmd = new SqlCommand(sql, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    r = new Reader()
                    {
                        Name = reader["Name"].ToString(),
                        ID = int.Parse(reader["ID"].ToString()),
                        EMail = reader["E-mail"].ToString()
                    };

                }

            }
            return r;
        }

        public Book TakingABook(Book book)
        {

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {

                string sqlFormattedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string sql = "EXEC TakingABook @ReaderID = " + CurrentUserInfo.Reader.ID + ", @BookID = " + book.ID + ", @DateTaken ='" +sqlFormattedDate+"'";
                
                SqlCommand cmd = new SqlCommand(sql, con);
                con.Open();
                cmd.ExecuteNonQuery();
            }
            UpdateBookQuantity(book, book.Quantity - 1);
            book.DateBookTaken.Add(DateTime.Now);
            book.Readers.Add(CurrentUserInfo.Reader);
            return book;
        }
    }
}