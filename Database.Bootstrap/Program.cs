using DbUp;
using System;
using System.Linq;
using System.Reflection;

namespace Database.Bootstrap
{
    class Program
    {
        // DbUp
        // https://dbup.readthedocs.io/en/latest/

        // Need to install .net5 runtime at target machine deploy
        // powershell cmd >Invoke-WebRequest "https://dot.net/v1/dotnet-install.ps1" -OutFile "dotnet-install.ps1"
        // powershell cmd >./dotnet-install.ps1 -Runtime dotnet -Version 5.0.6 -InstallDir "C:\Program Files\dotnet"

        static void Main(string[] args)
        {
            var connectionString = args.FirstOrDefault()
                    ?? "Fix connectionString here Ex. Initial Catalog = xxxxxxx";
            
            // If you want your application to create the database for you, add the following line after the connection string:
            EnsureDatabase.For.SqlDatabase(connectionString);

            var upgrader =
                DeployChanges.To
                    .SqlDatabase(connectionString)
                    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                    .LogToConsole()
                    .WithTransaction()
                    .Build();

            var result = upgrader.PerformUpgrade();

            if (!result.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Error);
                Console.ResetColor();
#if DEBUG
                Console.ReadLine();
#endif
                return;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success!");
            Console.ResetColor();
        }
    }
}
