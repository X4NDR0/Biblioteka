using Biblioteka.Facades.Sql;
using Biblioteka.Facades.Sql.Contracts;
using Biblioteka.Services;
namespace Biblioteka
{
    public class Biblioteka
    {
        static void Main(string[] args)
        {
            ISqlFacade sqlFacade = new SqlFacade();
            LibraryService libraryService = new LibraryService(sqlFacade);
            libraryService.Menu();
        }
    }
}