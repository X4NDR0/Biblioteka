using Biblioteka.Models;
using System.Data.SqlClient;

namespace Biblioteka.Services
{
    public class SQLService
    {
        #region Servisi
        private LibraryService _libraryService;
        #endregion

        public void RemoveBook()
        {
            _libraryService.WriteAllBooks(LoadBooks(LoadMember()));
            Console.Write("Unesite ID knjigu koju zelite da obrisete:");
            int.TryParse(Console.ReadLine(), out int idKnjige);

            Console.Clear();

            string command = "delete from Knjige where id=@id";

            using (SqlConnection sqlConnection = new SqlConnection("Data Source=XANDRO\\SQLEXPRESS;Initial Catalog=Biblioteka;Integrated Security=true"))
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@id", idKnjige);
                sqlCommand.ExecuteNonQuery();
            }

            Console.WriteLine("Knjiga je uspesno obrisana iz baze podataka!");
        }

        public void RemoveMember()
        {
            _libraryService.WriteAllMembers(LoadMember());
            Console.Write("Unesite ID clana:");
            int.TryParse(Console.ReadLine(), out int idClana);

            Console.Clear();

            string command = "delete from Clanovi where id=@id";

            using (SqlConnection sqlConnection = new SqlConnection("Data Source=XANDRO\\SQLEXPRESS;Initial Catalog=Biblioteka;Integrated Security=true"))
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@id", idClana);
                sqlCommand.ExecuteNonQuery();
            }
            Console.WriteLine("Clan je uspesno izbrisan iz baze podataka!");
        }

        public void AddBookSQL(bool bookNull, string naziv, string autor, int godinaIzdanja, Member member)
        {
            string command = "insert into Knjige(naziv,autor,godinaIzdanja,clanId)" +
                                "values(@naziv,@autor,@godinaIzdanja,@clanId)";

            using (SqlConnection sqlConnection = new SqlConnection("Data Source=XANDRO\\SQLEXPRESS;Initial Catalog=Biblioteka;Integrated Security=true"))
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
                if (bookNull)
                {
                    sqlCommand.Parameters.AddWithValue("@naziv", naziv);
                    sqlCommand.Parameters.AddWithValue("@autor", autor);
                    sqlCommand.Parameters.AddWithValue("@godinaIzdanja", godinaIzdanja);
                    sqlCommand.Parameters.AddWithValue("@clanId", member.ID);
                }
                else
                {
                    sqlCommand.Parameters.AddWithValue("@naziv", naziv);
                    sqlCommand.Parameters.AddWithValue("@autor", autor);
                    sqlCommand.Parameters.AddWithValue("@godinaIzdanja", godinaIzdanja);
                    sqlCommand.Parameters.AddWithValue("@clanId", DBNull.Value);
                }
                sqlCommand.ExecuteNonQuery();
            }
        }

        public void AddMember()
        {
            Console.Write("Unesite ime clana:");
            string ime = Console.ReadLine();

            Console.Write("Unesite prezime clana:");
            string prezime = Console.ReadLine();

            string command = "insert into Clanovi(ime,prezime)" +
                             "values(@ime,@prezime)";

            using (SqlConnection connection = new SqlConnection("Data Source=XANDRO\\SQLEXPRESS;Initial Catalog=Biblioteka;Integrated Security=true"))
            {
                SqlCommand cmd = new SqlCommand(command, connection);
                cmd.Parameters.AddWithValue("@ime", ime);
                cmd.Parameters.AddWithValue("@prezime", prezime);
                cmd.ExecuteNonQuery();
            }

            Console.Clear();
            Console.WriteLine("Clan je uspesno dodat u bazu podataka!");
        }

        public List<Book> LoadBooks(List<Member> listaClanova)
        {
            List<Book> listaKnjiga = new List<Book>();
            using (SqlConnection connection = new SqlConnection("Data Source=XANDRO\\SQLEXPRESS;Initial Catalog=Biblioteka;Integrated Security=true"))
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

        public List<Member> LoadMember()
        {
            List<Member> listaClanova = new List<Member>();
            using (SqlConnection connection = new SqlConnection("Data Source=XANDRO\\SQLEXPRESS;Initial Catalog=Biblioteka;Integrated Security=true"))
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
