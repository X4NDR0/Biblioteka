using Biblioteka.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Biblioteka.Services
{
    public class SQLService
    {
        private string _connectionString = "Data Source=XANDRO\\SQLEXPRESS;Initial Catalog=Biblioteka;Integrated Security=true";
        public void RemoveBookSQL(int bookId)
        {
            string command = "delete from Knjige where id=@id";

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@id", bookId);
                sqlCommand.ExecuteNonQuery();
            }
        }

        public void RemoveMemberSQL(int memberId)
        {
            string command = "delete from Clanovi where id=@id";

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@id", memberId);
                sqlCommand.ExecuteNonQuery();
            }
        }

        public void AddBookSQL(Book book)
        {
            string command = "insert into Knjige(naziv,autor,godinaIzdanja,clanId)" +
                                "values(@naziv,@autor,@godinaIzdanja,@clanId)";

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);

                int? memberId = null;

                if (book.Member != null)
                {
                    memberId = book.Member.ID;
                }

                sqlCommand.Parameters.AddWithValue("@naziv", book.Name);
                sqlCommand.Parameters.AddWithValue("@autor", book.Author);
                sqlCommand.Parameters.AddWithValue("@godinaIzdanja", book.ReleaseYear);
                sqlCommand.Parameters.AddWithValue("@clanId", memberId);
                sqlCommand.ExecuteNonQuery();
            }
        }

        public Member CheckBookSQL(Book book)
        {
            string command = "select clanId from Knjige where id = @bookId";

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@bookId", book.ID);
                using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                {
                    Member member = null;
                    List<Member> listaClanova = GetAllMembers();
                    while (sqlDataReader.Read())
                    {
                        if (book.Member != null)
                        {
                            member = listaClanova.Where(x => x.ID == sqlDataReader.GetInt32(0)).FirstOrDefault();
                        }
                    }
                    return member;
                }
            }
        }

        public void AddMemberSQL(Member member)
        {
            string command = "insert into Clanovi(ime,prezime)" +
                                "values(@ime,@prezime)";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(command, connection);
                cmd.Parameters.AddWithValue("@ime", member.Name);
                cmd.Parameters.AddWithValue("@prezime", member.Lastname);
                cmd.ExecuteNonQuery();
            }
        }

        public List<Book> GetAllBooks(List<Member> listaClanova)
        {
            List<Book> listaKnjiga = new List<Book>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("select * from Knjige", connection); ;

                using (SqlDataReader bookReader = command.ExecuteReader())
                {
                    while (bookReader.Read())
                    {
                        if (bookReader.GetValue(4) != DBNull.Value)
                        {
                            Member member = listaClanova.Where(x => x.ID == bookReader.GetInt32(4)).FirstOrDefault();
                            Book book = new Book { ID = bookReader.GetInt32(0), Name = bookReader.GetString(1), Author = bookReader.GetString(2), ReleaseYear = bookReader.GetInt32(3), Member = member };
                            listaKnjiga.Add(book);
                        }
                        else
                        {
                            Book book = new Book { ID = bookReader.GetInt32(0), Name = bookReader.GetString(1), Author = bookReader.GetString(2), ReleaseYear = bookReader.GetInt32(3) };
                            listaKnjiga.Add(book);
                        }
                    }
                }
            }
            return listaKnjiga;
        }

        public List<Member> GetAllMembers()
        {
            List<Member> listaClanova = new List<Member>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("select * from Clanovi", connection);

                using (SqlDataReader memberReader = command.ExecuteReader())
                {
                    while (memberReader.Read())
                    {
                        Member member = new Member { ID = memberReader.GetInt32(0), Name = memberReader.GetString(1), Lastname = memberReader.GetString(2) };
                        listaClanova.Add(member);
                    }
                }
            }
            return listaClanova;
        }
    }
}
