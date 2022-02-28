using Biblioteka.Services;
namespace Biblioteka
{
    class Biblioteka
    {
        static void Main(string[] args)
        {
            LibraryService libraryService = new LibraryService();
            libraryService.Menu();
        }
    }
}