using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using RTTE.Library.Process;

namespace RTTE.CLI {
    public class Program {
        private static CancellationTokenSource CancellationToken { get; } = new();

        // @TODO: add support for args
        public static void Main(string[] args) {
            string assemblyVersion = Assembly
                .GetExecutingAssembly()
                .GetCustomAttribute<AssemblyFileVersionAttribute>()
                .Version;

            Console.Title = $"Real-Time Text Extractor v{assemblyVersion}";
            Console.WriteLine("Welcome to Real-Time Text Extractor CLI version {0}!", assemblyVersion);
            Console.WriteLine("\nType /help for commands.");

            do {
                Console.Write("\n> ");
                string command = Console.ReadLine();

                switch (command.ToLower()) {
                    case "quit":
                    case "exit":
                        CancellationToken.Cancel();
                        break;
                    default:
                        Console.WriteLine("Unknown command {0}\nType /help for commands.", command);
                        break;
                }
            } while (!CancellationToken.IsCancellationRequested);
        }
    }
}
