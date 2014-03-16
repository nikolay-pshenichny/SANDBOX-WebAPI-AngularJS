using System.IO;

namespace DemoProject.API.Processors
{
    /// <summary>
    /// IFileProcessor implementation responsible for the search of a longest line in a file
    /// </summary>
    public class LongestLineFileProcessor : IFileProcessor
    {
        /// <summary>
        /// Processes a file (in data stream) and returns the longest text line from it.
        /// </summary>
        /// <param name="data">File data (should be text)</param>
        /// <returns>Longest text line from the file</returns>
        public string Process(Stream data)
        {
            string longestLineOfText = null;
            int longestLineOfTextLength = -1;

            TextReader reader = new StreamReader(data);

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (longestLineOfTextLength < line.Length)
                {
                    longestLineOfText = line;
                    longestLineOfTextLength = line.Length;
                }
            }
            
            return longestLineOfText;
        }
    }
}