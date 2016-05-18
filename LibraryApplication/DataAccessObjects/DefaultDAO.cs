using LibraryApplication.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace LibraryApplication.DataAccessObjects
{
    //Data Access Object class
    //Its actions available for unregistered user 
    //OR for Administrator & Reader at the same time
    public class DefaultDAO
    {
       public string ConnectionString;

        public DefaultDAO()
        {
            ConnectionString = System.Configuration.ConfigurationManager
                .ConnectionStrings["MyDatabase"].ConnectionString.ToString();
        }

        //Inserting E-Mail & Name of the user
        public void ReaderSignUp(Reader r)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                string sql = "EXEC InsertReader @Email = '" + r.EMail + "', @Name = '" + r.Name + "'";
                SqlCommand cmd = new SqlCommand(sql, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    r.ID = int.Parse(reader["ID"].ToString());
                }
            }
        }

        public List<Author> GetBookAuthors(int BookID)
        {
            List<Author> authors = new List<Author>();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                string sql = "EXEC ReadAuthors @ID = " + BookID;
                SqlCommand cmd = new SqlCommand(sql, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    authors.Add(new Author()
                    {
                        Name = reader["Name"].ToString(),
                        ID = int.Parse(reader["ID"].ToString())
                    });
                }
            }
            return authors;
        }

        public List<Book> GetBooks(bool showOnlyAvailable = true)
        {
            List<Book> books = new List<Book>();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                string sql;
                if (showOnlyAvailable)
                    sql = "EXEC ReadAvailableBooks";
                else
                    sql = "EXEC ReadAllBooks";
                SqlCommand cmd = new SqlCommand(sql, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    books.Add(new Book()
                    {
                        Name = reader["Name"].ToString(),
                        ID = int.Parse(reader["ID"].ToString()),
                        Quantity = int.Parse(reader["Quantity"].ToString())
                    });

                    books.Last().Authors = GetBookAuthors(books.Last().ID);
                }
            }
            return books;
        }


        //Admin: Get Any Reader Book History
        //Reader: Get own Book History 
        public List<Book> GetReaderBooks(Reader r)
        {
            List<Book> books = new List<Book>();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                string sql = "EXEC ReadUserBooks @ID = " + r.ID;
                SqlCommand cmd = new SqlCommand(sql, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    books.Add(new Book()
                    {
                        Name = reader["Name"].ToString(),
                        ID = int.Parse(reader["ID"].ToString()),
                    });
                    books.Last().Authors = GetBookAuthors(books.Last().ID);
                    DateTime d;
                    DateTime.TryParse(reader["DateTaken"].ToString(), out d);
                    
                    books.Last().DateBookTaken.Add(d);
                }
            }
            return books;
        }

        public List<Reader> GetAllReaders()
        {
            List<Reader> readers = new List<Reader>();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                string sql = "EXEC ReadAllReaders";
                SqlCommand cmd = new SqlCommand(sql, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    readers.Add(new Reader()
                    {
                        Name = reader["Name"].ToString(),
                        ID = int.Parse(reader["ID"].ToString()),
                        EMail = reader["E-mail"].ToString()
                    });
                }
            }
            return readers;
        }

        public void UpdateBookQuantity(Book b, int Quantity)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                string sql = "EXEC UpdateBookQuantity @ID = " + b.ID + ", @Quantity = " + Quantity;
                SqlCommand cmd = new SqlCommand(sql, con);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }

}