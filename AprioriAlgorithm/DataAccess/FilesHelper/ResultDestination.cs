using System;

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
            float minimumSupport, TimeSpan timeElapsed)
        {

        }
    }
}
