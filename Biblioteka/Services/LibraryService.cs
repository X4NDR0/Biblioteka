using Biblioteka.Enums;
using Biblioteka.Models;
using System.Data.SqlClient;

namespace Biblioteka.Services
{
    class LibraryService
    {

        #region Liste
        List<Book> listaKnjiga = new List<Book>();
        List<Member> listaClanova = new List<Member>();
        #endregion

        #region Enums
        Options options;
        #endregion

        private SqlConnection _connection;
        public void Menu()
        {
            _connection = SqlConnection(_connection);
            LoadData();

            do
            {
                MenuText();
                Enum.TryParse(Console.ReadLine(), out options);

                switch (options)
                {
                    case Options.WriteAllBooks:
                        Console.Clear();
                        WriteAllBooks();
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadLine();
                        Console.Clear();
                        break;

                    case Options.WriteAllMembers:
                        Console.Clear();
                        WriteAllMembers();
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadLine();
                        Console.Clear();
                        break;

                    case Options.AddBook:
                        Console.Clear();
                        AddBook();
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadLine();
                        Console.Clear();
                        break;

                    case Options.AddMembers:
                        Console.Clear();
                        AddMember();
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadLine();
                        Console.Clear();
                        break;

                    case Options.Exit:
                        Environment.Exit(1);
                        break;

                    default:
                        Console.WriteLine("Izabrana opcija ne postoji!");
                        break;
                }
            } while (options != Options.Exit);
        }

        public void MenuText()
        {
            Console.WriteLine("1.Write all books");
            Console.WriteLine("2.Write all members");
            Console.WriteLine("3.Add book");
            Console.WriteLine("4.Add member");
            Console.WriteLine("0.Exit");

            Console.Write("Option:");
        }

        public void AddBook()
        {
            Console.Write("Unesite naziv knjige:");
            string naziv = Console.ReadLine();

            Console.Write("Unesite autora:");
            string autor = Console.ReadLine();

            Console.Write("Unesite godinu izdanja:");
            int.TryParse(Console.ReadLine(), out int godinaIzdanja);

            Console.Clear();
            WriteAllMembers();
            Console.Write("Unesite ID clana:");
            int.TryParse(Console.ReadLine(), out int idClana);

            Console.Clear();

            Member member = listaClanova.Where(x => x.ID == idClana).FirstOrDefault();

            string command = "insert into Knjige(naziv,autor,godinaIzdanja,clanId)" +
                                "values(@naziv,@autor,@godinaIzdanja,@clanId)";

            if (member != null)
            {
                SqlCommand sqlCommand = new SqlCommand(command, _connection);
                sqlCommand.Parameters.AddWithValue("@naziv",naziv);
                sqlCommand.Parameters.AddWithValue("@autor",autor);
                sqlCommand.Parameters.AddWithValue("@godinaIzdanja",godinaIzdanja);
                sqlCommand.Parameters.AddWithValue("@clanId",member.ID);
                sqlCommand.ExecuteNonQuery();

                Console.Clear();
                Console.WriteLine("Knjiga je uspesno dodata u bazu podataka!");
            }
            else
            {
                Console.WriteLine("Clan sa izabranim ID-om ne postoji!");
                return;
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

            SqlCommand cmd = new SqlCommand(command, _connection);
            cmd.Parameters.AddWithValue("@ime", ime);
            cmd.Parameters.AddWithValue("@prezime", prezime);
            cmd.ExecuteNonQuery();

            Console.Clear();
            Console.WriteLine("Clan je uspesno dodat u bazu podataka!");
        }

        public SqlConnection SqlConnection(SqlConnection connection)
        {
            connection = new SqlConnection("Data Source=XANDRO\\SQLEXPRESS;Initial Catalog=Biblioteka;Integrated Security=true");
            connection.Open();
            return connection;
        }

        public void LoadData()
        {
            listaClanova.Clear();
            listaKnjiga.Clear();

            SqlCommand memberCommand = new SqlCommand("select * from Clanovi", _connection);
            SqlDataReader memberReader = memberCommand.ExecuteReader();

            while (memberReader.Read())
            {
                Member member = new Member { ID = memberReader.GetInt32(0), Name = memberReader.GetString(1), Lastname = memberReader.GetString(2) };
                listaClanova.Add(member);
            }
            memberReader.Close();

            SqlCommand bookCommand = new SqlCommand("select * from Knjige", _connection);
            SqlDataReader bookReader = bookCommand.ExecuteReader();

            while (bookReader.Read())
            {
                Member member = listaClanova.Where(x => x.ID == bookReader.GetInt32(3)).FirstOrDefault();

                if (member != null)
                {
                    Book book = new Book { Name = bookReader.GetString(0), Author = bookReader.GetString(1), ReleaseYear = bookReader.GetInt32(2), Member = member };
                    listaKnjiga.Add(book);
                }
            }
            bookReader.Close();
        }

        public void WriteAllMembers()
        {
            Console.WriteLine("=-=-=-=-=-=-=-=-=-=-=-=-=-=");
            foreach (Member member in listaClanova)
            {
                Console.WriteLine("ID:" + member.ID + "\nIme:" + member.Name + "\nPrezime:" + member.Lastname);
                Console.WriteLine("=-=-=-=-=-=-=-=-=-=-=-=-=-=");
            }
        }

        public void WriteAllBooks()
        {
            Console.WriteLine("=-=-=-=-=-=-=-=-=-=-=-=-=-=");
            foreach (Book book in listaKnjiga)
            {
                Console.WriteLine("Naziv:" + book.Name + "\nAutor:" + book.Author + "\nGodina izdanja:" + book.ReleaseYear + "\nKnjiga je kod clana pod ID-om:" + book.Member.ID);
                Console.WriteLine("=-=-=-=-=-=-=-=-=-=-=-=-=-=");
            }
        }

    }
}
