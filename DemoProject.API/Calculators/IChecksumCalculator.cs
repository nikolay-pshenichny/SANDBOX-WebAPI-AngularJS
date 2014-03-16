using System.IO;

using DemoProject.Model.Enums;

namespace DemoProject.API.Calculators
{
    /// <summary>
    /// Base interface for file checksum calculators
    /// </summary>
    public interface IChecksumCalculator
    {
        /// <summary>
        /// Gets the checksum type which current calculator calculates.
        /// </summary>
        ChecksumType ChecksumType { get; }

        /// <summary>
        /// Performs checksum calculation over the data in the stream.
        /// Callers must ensure that the position inside of the stream is correct, before calling this method.
        /// </summary>
        /// <param name="data">Data stream</param>
        /// <returns>Data's checksum</returns>
        string Calculate(Stream data);
    }
}
