using Biblioteka.Facades.Sql.Contracts;
using Biblioteka.Facades.Sql.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Biblioteka.Facades.Sql
{
    public class SqlFacade : ISqlFacade
    {
        private string _connectionString = "Data Source=XANDRO\\SQLEXPRESS;Initial Catalog=Biblioteka;Integrated Security=true";

        public bool RemoveBookSQL(int bookId)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                string command = "delete from Knjige where id=@id";
                SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@id", bookId);
                sqlCommand.ExecuteNonQuery();
                return true;
            }
        }

        public bool RemoveMemberSQL(int memberId)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                string command = "delete from Clanovi where id=@id";
                SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@id", memberId);
                sqlCommand.ExecuteNonQuery();
                return true;
            }
        }

        public bool AddBookSQL(Book book)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                string command = "insert into Knjige(naziv,autor,godinaIzdanja,clanId)" +
                                    "values(@naziv,@autor,@godinaIzdanja,@clanId)";
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
                return true;
            }
        }

        public Member CheckBookSQL(Book book)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                string command = "select clanId from Knjige where id = @bookId";
                SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@bookId", book.ID);
                using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                {
                    Member member = null;
                    List<Member> listaClanova = GetAllMembers();
                    while (sqlDataReader.Read())
                    {
                        int.TryParse(sqlDataReader["clanId"].ToString(), out int id);
                        if (book.Member != null)
                        {
                            member = listaClanova.Where(x => x.ID == id).FirstOrDefault();
                        }
                    }
                    return member;
                }
            }
        }

        public bool AddMemberSQL(Member member)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string command = "insert into Clanovi(ime,prezime)" +
                                    "values(@ime,@prezime)";
                connection.Open();
                SqlCommand cmd = new SqlCommand(command, connection);
                cmd.Parameters.AddWithValue("@ime", member.Name);
                cmd.Parameters.AddWithValue("@prezime", member.Lastname);
                cmd.ExecuteNonQuery();
                return true;
            }
        }

        public Member GetMemberByID(int memberId)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                string command = "select id,ime,prezime from Clanovi where id = @memberId";
                SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@memberId", memberId);
                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    Member member = null;
                    while (reader.Read())
                    {
                        int.TryParse(reader["id"].ToString(), out int id);
                        string ime = reader["ime"].ToString();
                        string prezime = reader["prezime"].ToString();
                        member = new Member { ID = id, Name = ime, Lastname = prezime };
                    }
                    return member;
                }
            }
        }

        public Book GetBookByID(int bookId)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                string command = "select id,naziv,autor,godinaIzdanja,clanId from Knjige where id = @bookId";
                SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@bookId", bookId);
                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    Book book = null;
                    while (reader.Read())
                    {
                        int.TryParse(reader["id"].ToString(), out int id);
                        string naziv = reader["naziv"].ToString();
                        string autor = reader["autor"].ToString();
                        int.TryParse(reader["godinaIzdanja"].ToString(),out int godinaIzdanja);
                        int.TryParse(reader["clanId"].ToString(),out int clanId);
                        Member member = GetMemberByID(clanId);
                        book = new Book { ID = id, Name = naziv, Author = autor, ReleaseYear = godinaIzdanja, Member = member };
                    }
                    return book;
                }
            }
        }

        public List<Book> GetAllBooks(List<Member> listaClanova)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("select * from Knjige", connection); ;

                using (SqlDataReader bookReader = command.ExecuteReader())
                {
                    List<Book> listaKnjiga = new List<Book>();
                    while (bookReader.Read())
                    {
                        int.TryParse(bookReader["id"].ToString(), out int id);
                        string naziv = bookReader["naziv"].ToString();
                        string autor = bookReader["autor"].ToString();
                        int.TryParse(bookReader["godinaIzdanja"].ToString(), out int godinaIzdanja);
                        int.TryParse(bookReader["clanId"].ToString(), out int clanId);
                        if (bookReader.GetValue(4) != DBNull.Value)
                        {
                            Member member = listaClanova.Where(x => x.ID == clanId).FirstOrDefault();
                            Book book = new Book { ID = id, Name = naziv, Author = autor, ReleaseYear = godinaIzdanja, Member = member };
                            listaKnjiga.Add(book);
                        }
                        else
                        {
                            Book book = new Book { ID = id, Name = naziv, Author = autor, ReleaseYear = godinaIzdanja };
                            listaKnjiga.Add(book);
                        }
                    }
                    return listaKnjiga;
                }
            }
        }

        public List<Member> GetAllMembers()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("select * from Clanovi", connection);

                using (SqlDataReader memberReader = command.ExecuteReader())
                {
                    List<Member> listaClanova = new List<Member>();
                    while (memberReader.Read())
                    {
                        int.TryParse(memberReader["id"].ToString(), out int id);
                        string ime = memberReader["ime"].ToString();
                        string prezime = memberReader["prezime"].ToString();
                        Member member = new Member { ID = id, Name = ime, Lastname = prezime };
                        listaClanova.Add(member);
                    }
                    return listaClanova;
                }
            }
        }
    }
}
