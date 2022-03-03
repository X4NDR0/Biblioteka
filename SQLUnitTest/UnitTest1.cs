using Biblioteka.Facades.Sql;
using Biblioteka.Facades.Sql.Contracts;
using Biblioteka.Facades.Sql.Models;
using NUnit.Framework;
using System.Collections.Generic;

namespace SQLUnitTest
{
    public class Tests
    {
        private ISqlFacade _sqlFacade;
        public Tests()
        {
            _sqlFacade = new SqlFacade();
        }

        [Test]
        public void GetMemberByID_WhenRequestGoesThrough_ExpectingSameName()
        {
            //arrange
            int memberId = 16;
            string expectedMemberName = "Marko";

            //act
            Member result = _sqlFacade.GetMemberByID(memberId);

            //assert
            Assert.AreEqual(result.Name, expectedMemberName);
        }

        [Test]
        public void GetBookByID_WhenRequestGoesThrough_ExpectingSameName()
        {
            //arrange
            int bookId = 6;
            string expectedBookName = "Test5";

            //act
            Book result = _sqlFacade.GetBookByID(bookId);

            //assert
            Assert.AreEqual(result.Name, expectedBookName);
        }

        [Test]
        public void RemoveMemberByID_WhenRequestGoesThrough_ExpectingTrue()
        {
            //arrange
            int memberId = 18;
            bool expectedBool = true;

            //act
            bool result = _sqlFacade.RemoveMemberSQL(memberId);

            //assert
            Assert.AreEqual(expectedBool,result);
        }

        [Test]
        public void RemoveBookByID_WhenRequestGoesThrough_ExpectingTrue()
        {
            //arrange
            int bookId = 7;
            bool expectedBool = true;

            //act
            bool result = _sqlFacade.RemoveBookSQL(bookId);

            //assert
            Assert.AreEqual(expectedBool, result);
        }

        [Test]
        public void GetAllMembers_WhenRequestGoesThrough_ExpectingItems()
        {
            //arrange
            List<Member> listaClanova = new List<Member>();

            //act
            listaClanova = _sqlFacade.GetAllMembers();

            //assert
            Assert.IsTrue(listaClanova.Count >= 1);
        }

        [Test]
        public void GetAllBooks_WhenRequestGoesThrough_ExpectingItems()
        {
            //arrange
            List<Member> listaClanova = new List<Member>();
            List<Book> listaKnjiga = new List<Book>();

            //act
            listaClanova = _sqlFacade.GetAllMembers();
            listaKnjiga = _sqlFacade.GetAllBooks(listaClanova);

            //assert
            Assert.IsTrue(listaKnjiga.Count >= 1);
        }

        [Test]
        public void AddBook_WhenRequestGoesThrough_ExpectingTrue()
        {
            //arrange
            bool expectedBool = true;
            Member member = _sqlFacade.GetMemberByID(17);

            //act
            bool result = _sqlFacade.AddBookSQL(new Book { Name = "Test50",Author = "Neko",ReleaseYear = 2022,Member = member});

            //assert
            Assert.AreEqual(expectedBool,result);
        }    


        [Test]
        public void AddMember_WhenRequestGoesThrough_ExpectingTrue()
        {
            //arrange
            bool expectedBool = true;

            //act
            bool result = _sqlFacade.AddMemberSQL(new Member { Name = "Jovan",Lastname = "Jovic"});

            //assert
            Assert.AreEqual(expectedBool,result);
        }

        [Test]
        public void CheckBook_WhenRequestGoesThrough_ExpectingSameName()
        {
            //arrange
            Book book = _sqlFacade.GetBookByID(8);

            //act
            Member member = _sqlFacade.CheckBookSQL(book);

            //assert
            Assert.AreEqual(book.Member.Name,member.Name);
        }
    }
}