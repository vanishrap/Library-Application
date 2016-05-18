using LibraryApplication.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LibraryApplication.DataAccessObjects
{
    public class AdminDAO : DefaultDAO
    {
        public AdminDAO(): base() { }


        public void GetBookHistory(Book b)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                string sql = "EXEC ReadBookhistory @ID = " + b.ID;
                SqlCommand cmd = new SqlCommand(sql, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    b.Readers.Add(new Reader()
                    {
                        Name = reader["Name"].ToString(),
                        ID = int.Parse(reader["ReaderID"].ToString()),
                        EMail = reader["E-mail"].ToString()
                    });

                    b.DateBookTaken.Add(Convert.ToDateTime(reader["DateTaken"].ToString()));
                }
            }
        }



        public void InsertBookAuthor(string AuthorName, string BookTitle)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                string sql = "EXEC InsertBookAuthor @AuthorName = '" + AuthorName + "', @BookTitle = '" + BookTitle + "'";
                SqlCommand cmd = new SqlCommand(sql, con);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }


        //Inserting Book Info
        //Parsing single string in multiple strings
        //Checking if author exists & inserting author if needed
        public void InsertBook(string Authors, string BookTitle, int Quantity)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                string sql = "EXEC InsertBook @Name = '" + BookTitle + "', @Quantity = " + Quantity;
                SqlCommand cmd = new SqlCommand(sql, con);
                con.Open();
                cmd.ExecuteNonQuery();
            }

            string[] AuthorsArray = Authors.Split(',');
            foreach (string s in AuthorsArray)
            {
                InsertBookAuthor(s, BookTitle);
            }
        }


        public void DeleteBook(Book b)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                string sql = "EXEC DeleteBook @ID = "+ b.ID;
                SqlCommand cmd = new SqlCommand(sql, con);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

    }
}