using Biblioteka.Facades.Sql.Models;
using System.Collections.Generic;

namespace Biblioteka.Facades.Sql.Contracts
{
    public interface ISqlFacade
    {
        bool RemoveBookSQL(int memberId);
        bool RemoveMemberSQL(int memberId);
        bool AddBookSQL(Book book);
        Member CheckBookSQL(Book book);
        bool AddMemberSQL(Member member);
        List<Book> GetAllBooks(List<Member> listaClanova);
        List<Member> GetAllMembers();
        Member GetMemberByID(int memberId);
        Book GetBookByID(int bookId);
    }
}
