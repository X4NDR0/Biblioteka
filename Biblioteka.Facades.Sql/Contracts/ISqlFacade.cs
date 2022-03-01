using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biblioteka.Facades.Sql.Models;

namespace Biblioteka.Facades.Sql.Contracts
{
    public interface ISqlFacade
    {
        void RemoveBookSQL(int memberId);
        void RemoveMemberSQL(int memberId);
        void AddBookSQL(Book book);
        Member CheckBookSQL(Book book);
        void AddMemberSQL(Member member);
        List<Book> GetAllBooks(List<Member> listaClanova);
        List<Member> GetAllMembers();
    }
}
