using Biblioteka.Facades.Sql;
using Biblioteka.Facades.Sql.Contracts;
using Biblioteka.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace Biblioteka
{
    public class Biblioteka
    {
        static void Main(string[] args)
        {
            IServiceProvider container = SetupServices();
            LibraryService libraryService = container.GetRequiredService<LibraryService>();
            ISqlFacade sqlFacade = container.GetRequiredService<ISqlFacade>();
            libraryService.Menu();
        }
        public static IServiceProvider SetupServices()
        {
            ServiceProvider services = new ServiceCollection()
                .AddSingleton<ISqlFacade, SqlFacade>()
                .AddSingleton<LibraryService>()
                .BuildServiceProvider();
            return services;
        }
    }
}