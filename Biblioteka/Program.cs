using Biblioteka.Services;
namespace Biblioteka
{
    public class Biblioteka
    {
        static void Main(string[] args)
        {
            SQLService sqlService = new SQLService();
            LibraryService libraryService = new LibraryService(sqlService);
            libraryService.Menu();
        }
    }
}