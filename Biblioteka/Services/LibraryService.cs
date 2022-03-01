using Biblioteka.Enums;
using Biblioteka.Facades.Sql.Contracts;
using Biblioteka.Facades.Sql.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Biblioteka.Services
{
    public class LibraryService
    {
        private ISqlFacade _sqlFacade;

        public LibraryService(ISqlFacade sqlFacade)
        {
            _sqlFacade = sqlFacade;
        }

        public void Menu()
        {
            Options options;
            do
            {
                Console.Clear();
                MenuText();
                Enum.TryParse(Console.ReadLine(), out options);

                switch (options)
                {
                    case Options.WriteAllBooks:
                        Console.Clear();
                        DisplayAllBooks(_sqlFacade.GetAllBooks(_sqlFacade.GetAllMembers()));
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadLine();
                        Console.Clear();
                        break;

                    case Options.WriteAllMembers:
                        Console.Clear();
                        DisplayAllMembers(_sqlFacade.GetAllMembers());
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

                    case Options.RemoveBook:
                        Console.Clear();
                        RemoveBook();
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadLine();
                        Console.Clear();
                        break;

                    case Options.RemoveMember:
                        Console.Clear();
                        RemoveMember();
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadLine();
                        Console.Clear();
                        break;

                    case Options.CheckBook:
                        Console.Clear();
                        CheckBook();
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

        public void CheckBook()
        {
            List<Member> listaClanova = _sqlFacade.GetAllMembers();
            List<Book> listaKnjiga = _sqlFacade.GetAllBooks(listaClanova);
            Console.Write("Unesite ID knjige:");
            int.TryParse(Console.ReadLine(), out int idKnjige);

            Console.Clear();

            Book book = listaKnjiga.Where(x => x.ID == idKnjige).FirstOrDefault();
            if (book != null)
            {
                Member member = _sqlFacade.CheckBookSQL(book);
                Console.WriteLine("=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=");
                if (member == null)
                {
                    Console.WriteLine("ID:" + book.ID + "\nNaziv:" + book.Name + "\nAutor:" + book.Author + "\nGodina izdanja:" + book.ReleaseYear + "\nKnjigu ne poseduje ni jedan clan.");
                }
                else
                {
                    Console.WriteLine("ID:" + book.ID + "\nNaziv:" + book.Name + "\nAutor:" + book.Author + "\nGodina izdanja:" + book.ReleaseYear + "\nKnjiga je kod clana pod ID-om:" + book.Member.ID);
                }
                Console.WriteLine("=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=");
            }
            else
            {
                Console.WriteLine("Izabrana knjiga ne postoji!");
            }
        }

        public void AddMember()
        {
            Console.Write("Unesite ime clana:");
            string ime = Console.ReadLine();

            Console.Write("Unesite prezime clana:");
            string prezime = Console.ReadLine();

            Console.Clear();

            _sqlFacade.AddMemberSQL(new Member { Name = ime, Lastname = prezime });
            Console.WriteLine("Clan je uspesno dodat u bazu podataka!");
        }

        public void RemoveMember()
        {
            List<Member> listaClanova = _sqlFacade.GetAllMembers();
            DisplayAllMembers(listaClanova);
            Console.Write("Unesite ID clana:");
            int.TryParse(Console.ReadLine(), out int idClana);

            Console.Clear();

            Member member = listaClanova.Where(x => x.ID == idClana).FirstOrDefault();
            if (member != null)
            {
                _sqlFacade.RemoveMemberSQL(idClana);
                Console.WriteLine("Clan je uspesno izbrisan iz baze podataka!");
            }
            else
            {
                Console.WriteLine("Clan sa izabranim ID-om ne postoji!");
                return;
            }
        }

        public void RemoveBook()
        {
            List<Member> listaClanova = _sqlFacade.GetAllMembers();
            List<Book> listaKnjiga = _sqlFacade.GetAllBooks(listaClanova);

            DisplayAllBooks(listaKnjiga);
            Console.Write("Unesite ID knjigu koju zelite da obrisete:");
            int.TryParse(Console.ReadLine(), out int idKnjige);

            Console.Clear();

            Book book = listaKnjiga.Where(x => x.ID == idKnjige).FirstOrDefault();
            if (book != null)
            {
                _sqlFacade.RemoveBookSQL(idKnjige);
                Console.WriteLine("Knjiga je uspesno obrisana iz baze podataka!");
            }
            else
            {
                Console.WriteLine("Knjiga ne postoji!");
                return;
            }
        }

        public void AddBook()
        {
            List<Member> listaClanova = _sqlFacade.GetAllMembers();
            Console.Write("Unesite naziv knjige:");
            string naziv = Console.ReadLine();

            Console.Write("Unesite autora:");
            string autor = Console.ReadLine();

            Console.Write("Unesite godinu izdanja:");
            int.TryParse(Console.ReadLine(), out int godinaIzdanja);

            Console.Clear();
            DisplayAllMembers(listaClanova);
            Console.Write("Unesite ID clana koji poseduje knjigu,a ukoliko niko ne poseduje knjigu unesite 0:");
            int.TryParse(Console.ReadLine(), out int idClana);

            Console.Clear();

            Member member = null;
            if (idClana != 0)
            {
                member = listaClanova.Where(x => x.ID == idClana).FirstOrDefault();
                if (member == null)
                {
                    Console.WriteLine("Clan sa izabranim ID-om ne postoji!");
                    return;
                }
            }

            Book book = new Book { Name = naziv, Author = autor, ReleaseYear = godinaIzdanja, Member = member };
            _sqlFacade.AddBookSQL(book);
        }

        public void DisplayAllMembers(List<Member> listaClanova)
        {
            Console.WriteLine("=-=-=-=-=-=-=-=-=-=-=-=-=-=");
            foreach (Member member in listaClanova)
            {
                Console.WriteLine("ID:" + member.ID + "\nIme:" + member.Name + "\nPrezime:" + member.Lastname);
                Console.WriteLine("=-=-=-=-=-=-=-=-=-=-=-=-=-=");
            }
        }

        public void DisplayAllBooks(List<Book> listaKnjiga)
        {
            Console.WriteLine("=-=-=-=-=-=-=-=-=-=-=-=-=-=");
            foreach (Book book in listaKnjiga)
            {
                if (book.Member is null)
                {
                    Console.WriteLine("ID:" + book.ID + "\nNaziv:" + book.Name + "\nAutor:" + book.Author + "\nGodina izdanja:" + book.ReleaseYear + "\nKnjigu ne poseduje ni jedan clan.");
                }
                else
                {
                    Console.WriteLine("ID:" + book.ID + "\nNaziv:" + book.Name + "\nAutor:" + book.Author + "\nGodina izdanja:" + book.ReleaseYear + "\nKnjiga je kod clana pod ID-om:" + book.Member.ID);
                }
                Console.WriteLine("=-=-=-=-=-=-=-=-=-=-=-=-=-=");
            }
        }
    }
}
