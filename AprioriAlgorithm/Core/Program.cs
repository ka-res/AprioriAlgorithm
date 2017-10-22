using System;
using System.Configuration;
using System.Diagnostics;
using AprioriAlgorithm.DataAccess.FilesHelper;

namespace AprioriAlgorithm.Core
{
    class Program
    {
        private static void RunAlgorithm()
        {
            var dataSource
                = new DataSource(ConfigurationManager.AppSettings["dataSource"]);
            var resultDestination
                = new ResultDestination(Environment.SpecialFolder.Desktop.ToString());
            var minimalSupport = 0.2;

            var apriori = new Apriori(
                dataSource, 
                resultDestination, 
                minimalSupport);

            apriori.GetStrongTwoProductsItems(
                apriori.GetStrongItems(DataSource.TransactionsSet), 
                DataSource.TransactionsSet);
        }

        static void Main(string[] args)
        {
            Console.WriteLine($"KaReS 2017" +
                              $"\r\nApriori algorithm" +
                              $"\r\n[app for educational purposes]");

            var watch = new Stopwatch();

            watch.Start();
            RunAlgorithm();
            watch.Stop();

            Console.WriteLine($"\r\n[time elapsed: {watch.Elapsed}]");
            Console.WriteLine($"\r\nAlgorithm is over." +
                              $"\r\nPress any button to close app...");
            Console.ReadKey();
        }
    }
}
