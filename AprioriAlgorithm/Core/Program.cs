using System;
using System.Configuration;
using System.Diagnostics;
using AprioriAlgorithm.DataAccess.FilesHelper;

namespace AprioriAlgorithm.Core
{
    class Program
    {
        private static void MainSequence()
        {
            Console.WriteLine($"KaReS 2017" +
                              $"\r\nApriori algorithm" +
                              $"\r\n[app for educational purposes]" +
                              $"\r\n\r\nThis app provides a simple approach to how Apriori may be used." +
                              $"\r\nIt ships its own dataset which is one & only for now." +
                              $"\r\nDataset is related to educational practice and the app" +
                              $"\r\nwas build to work with it. Please, mention this." +
                              $"\r\n\r\nPlease, provide the minimal support level in percents" +
                              $"\r\n(numeric value without the \"%\" sign)");

            Console.WriteLine("\r\n-> minimal support expected: ");
            var minimalSupport = "";
            var minimalSupportValue = double.NaN;
            do
            {
                minimalSupport = Console.ReadLine();
            } while (!double.TryParse(minimalSupport, out minimalSupportValue));
            ;
            Console.WriteLine($"\r\n\r\nPlease, provide the minimal confidence level in percents" +
                              $"\r\n(numeric value without the \"%\" sign)");

            Console.WriteLine("\r\n-> minimal confidence expected: ");
            var minimalConfidence = "";
            var minimalConfidenceValue = double.NaN;
            do
            {
                minimalConfidence = Console.ReadLine();
            } while (!double.TryParse(minimalConfidence, out minimalConfidenceValue));

            Console.WriteLine($"\r\nDo you want to save your results on desktop? (y/n)");

            var enableSave = "";
            do
            {
                enableSave = Console.ReadLine();
            } while (enableSave != "y" && enableSave != "n");

            var ifSaveEnabled = enableSave == "y" ? true : false;

            Console.WriteLine($"\r\nChosen minimal support level is: {minimalSupport}% " +
                              $"& minimal confidence level is: {minimalConfidence}%" +
                              $"\r\nAlgorithm is running...\r\n" +
                              $"\r\nproduct1   product2   support   confidence" +
                              $"\r\n--------   --------   -------   ----------");

            var watch = new Stopwatch();

            watch.Start();
            RunAlgorithm(minimalSupportValue / 100, minimalConfidenceValue / 100, ifSaveEnabled);
            watch.Stop();            
        }

        private static void RunAlgorithm(double minimalSupportByUser, double minimalConfidenceByUser, bool ifSaveEnabled)
        {
            var dataSource
                = new DataSource(ConfigurationManager.AppSettings["dataSource"]);
            var resultDestination
                = new ResultDestination(Environment.SpecialFolder.Desktop.ToString());

            var apriori = new Apriori(
                dataSource, 
                resultDestination, 
                minimalSupportByUser,
                minimalConfidenceByUser);

            apriori.GetStrongTwoProductsItems(
                apriori.GetStrongItems(DataSource.TransactionsSet), 
                DataSource.TransactionsSet,
                ifSaveEnabled);
        }

        static void Main(string[] args)
        {
            var repeatProcedure = "";
            do
            {
                MainSequence();

                Console.WriteLine("\r\n\r\nDo you want to repeat? (y)");
                repeatProcedure = Console.ReadLine();                
            } while (repeatProcedure == "y");            
        }
    }
}
