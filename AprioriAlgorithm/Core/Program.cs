using System;
using System.Configuration;
using System.Diagnostics;
using AprioriAlgorithm.DataAccess.FilesHelper;

namespace AprioriAlgorithm.Core
{
    class Program
    {
        private static void RunAlgorithm(double minimalSupportByUser)
        {
            var dataSource
                = new DataSource(ConfigurationManager.AppSettings["dataSource"]);
            var resultDestination
                = new ResultDestination(Environment.SpecialFolder.Desktop.ToString());
            var minimalSupport = minimalSupportByUser;

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
                              $"\r\n[app for educational purposes]" +
                              $"\r\n\r\nThis app provides a simple approach to how Apriori may be used." +
                              $"\r\nIt ships its own dataset which is one & only for now." +
                              $"\r\nDataset is related to educational practice and the app" +
                              $"\r\nwas build to work with it. Please, mention this." +
                              $"\r\n\r\nPlease, provide the minimal support level in percents" +
                              $"\r\n(without the \"%\" sign)" +
                              $"\r\nin order to start the algorithm");

            Console.WriteLine("\r\n-> minimal support: ");
            var minimalSupport = Console.ReadLine();
            var watch = new Stopwatch();

            Console.WriteLine($"\r\nChosen minimal support level is: {minimalSupport}%" +
                              $"\r\nAlgorithm is running...\r\n" +
                              $"\r\nproduct1   product2   support" +
                              $"\r\n--------   --------   --------");

            watch.Start();
            RunAlgorithm(Convert.ToDouble(minimalSupport) / 100);
            watch.Stop();

            Console.WriteLine($"\r\n\r\n[time elapsed: {watch.Elapsed}]");
            Console.WriteLine($"\r\nAlgorithm is over." +
                              $"\r\nPress any button to close app...");
            Console.ReadKey();
        }
    }
}
