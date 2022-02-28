using Biblioteka.Enums;
using Biblioteka.Models;

namespace Biblioteka.Services
{
    class LibraryService
    {
        #region Enums
        Options options;
        #endregion

        #region Servisi
        private SQLService _sqlService = new SQLService();
        #endregion

        public void Menu()
        {
            do
            {
                Console.Clear();
                MenuText();
                Enum.TryParse(Console.ReadLine(), out options);

                switch (options)
                {
                    case Options.WriteAllBooks:
                        Console.Clear();
                        WriteAllBooks(_sqlService.LoadBooks(_sqlService.LoadMember()));
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadLine();
                        Console.Clear();
                        break;

                    case Options.WriteAllMembers:
                        Console.Clear();
                        WriteAllMembers(_sqlService.LoadMember());
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
                        _sqlService.AddMember();
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadLine();
                        Console.Clear();
                        break;

                    case Options.RemoveBook:
                        Console.Clear();
                        _sqlService.RemoveBook();
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadLine();
                        Console.Clear();
                        break;

                    case Options.RemoveMember:
                        Console.Clear();
                        _sqlService.RemoveMember();
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadLine();
                        Console.Clear();
                        break;

                    case Options.CheckBook:
                        Console.Clear();
                        CheckBook(_sqlService.LoadMember());
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
            Console.WriteLine("5.Remove book");
            Console.WriteLine("6.Remove member");
            Console.WriteLine("7.Provera knjige");
            Console.WriteLine("0.Exit");

            Console.Write("Option:");
        }

        public void AddBook()
        {
            List<Member> listaClanova = _sqlService.LoadMember();
            Console.Write("Unesite naziv knjige:");
            string naziv = Console.ReadLine();

            Console.Write("Unesite autora:");
            string autor = Console.ReadLine();

            Console.Write("Unesite godinu izdanja:");
            int.TryParse(Console.ReadLine(), out int godinaIzdanja);

            Console.Clear();
            WriteAllMembers(listaClanova);
            Console.Write("Unesite ID clana koji poseduje knjigu,a ukoliko niko ne poseduje knjigu unesite 0:");
            int.TryParse(Console.ReadLine(), out int idClana);

            Console.Clear();

            Member member = null;
            if (idClana != 0)
            {
                member = listaClanova.Where(x => x.ID == idClana).FirstOrDefault();
            }

            if (idClana != 0 && member != null)
            {
                _sqlService.AddBookSQL(true, naziv, autor, godinaIzdanja, member);
                Console.Clear();
                Console.WriteLine("Knjiga je uspesno dodata u bazu podataka!");
            }
            else if (member == null && idClana == 0)
            {
                _sqlService.AddBookSQL(false, naziv, autor, godinaIzdanja, member);
                Console.Clear();
                Console.WriteLine("Knjiga je uspesno dodata u bazu podataka!");
            }
            else
            {
                Console.WriteLine("Clan sa izabranim ID-om ne postoji!");
            }
        }

        public void WriteAllMembers(List<Member> listaClanova)
        {
            Console.WriteLine("=-=-=-=-=-=-=-=-=-=-=-=-=-=");
            foreach (Member member in listaClanova)
            {
                Console.WriteLine("ID:" + member.ID + "\nIme:" + member.Name + "\nPrezime:" + member.Lastname);
                Console.WriteLine("=-=-=-=-=-=-=-=-=-=-=-=-=-=");
            }
        }

        public void WriteAllBooks(List<Book> listaKnjiga)
        {
            Console.WriteLine("=-=-=-=-=-=-=-=-=-=-=-=-=-=");
            foreach (Book book in listaKnjiga)
            {
                if (book.Member is null)
                {
                    Console.WriteLine("ID:" + book.ID + "\nNaziv:" + book.Name + "\nAutor:" + book.Author + "\nGodina izdanja:" + book.ReleaseYear + "\nKnjigu ne poseduje ni jedan clan."); ;
                }
                else
                {
                    Console.WriteLine("ID:" + book.ID + "\nNaziv:" + book.Name + "\nAutor:" + book.Author + "\nGodina izdanja:" + book.ReleaseYear + "\nKnjiga je kod clana pod ID-om:" + book.Member.ID);
                }
                Console.WriteLine("=-=-=-=-=-=-=-=-=-=-=-=-=-=");
            }
        }

        public void CheckBook(List<Member> listaClanova)
        {
            List<Book> listaKnjiga = _sqlService.LoadBooks(listaClanova);

            Console.Write("Unesite ID knjige:");
            int.TryParse(Console.ReadLine(), out int idKnjige);

            Console.Clear();

            Console.WriteLine("=-=-=-=-=-=-=-=-=-=-=-=-=-=");
            foreach (Book book in listaKnjiga)
            {
                if (book.ID == idKnjige)
                {
                    Book knjiga = listaKnjiga.Where(x => x.ID == idKnjige).FirstOrDefault();
                    if (knjiga.Member != null)
                    {
                        Console.WriteLine("Knjigu poseduje clan:");
                        Console.WriteLine("ID:" + knjiga.Member.ID + "\nIme:" + knjiga.Member.Name + "\nPrezime:" + knjiga.Member.Lastname);
                    }
                    else
                    {
                        Console.WriteLine("Knjigu ne poseduje niko!");
                    }
                    Console.WriteLine("=-=-=-=-=-=-=-=-=-=-=-=-=-=");
                }
            }
        }
    }
}
