using System.IO;

namespace DemoProject.API.Processors
{
    /// <summary>
    /// Base interface for all file processors.
    /// </summary>
    public interface IFileProcessor
    {
        /// <summary>
        /// Processes the file (data stream) and returns some (depends on implementation) results.
        /// Callers must ensure that the position inside of the stream is correct, before calling this method.
        /// </summary>
        /// <param name="data">Data stream</param>
        /// <returns>Processing results</returns>
        string Process(Stream data);
    }
}
