using System;
using System.IO;

namespace AprioriAlgorithm.DataAccess.FilesHelper
{
    public class ResultDestination
    {
        public string FilePath { get; set; }

        public ResultDestination(string filePath)
        {
            FilePath = filePath;
        }
        
        public static void SaveResultToFile(string output, 
            double? minimalSupport, double? minimalConfidence)
        {
            using (var fileStream = new FileStream(
                Path.Combine($@"C:\Users\{Environment.UserName}\Desktop", 
                $"ted_result_" +
                $"{DateTime.Now.ToShortDateString().Replace('.', '_')}_" +
                $"{DateTime.Now.ToLongTimeString().Replace(':', '_')}.txt"), 
                FileMode.OpenOrCreate))
            {
                using (var streamWriter = new StreamWriter(fileStream))
                {
                    streamWriter.Write(output);
                }
            }
        }
    }
}
