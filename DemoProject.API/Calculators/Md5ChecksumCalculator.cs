using System;
using System.IO;
using System.Security.Cryptography;

using DemoProject.Model.Enums;

namespace DemoProject.API.Calculators
{
    /// <summary>
    /// IChecksumCalculator implementation responsible for MD5 calculations
    /// </summary>
    public class Md5ChecksumCalculator : IChecksumCalculator
    {
        public ChecksumType ChecksumType
        {
            get
            {
                return ChecksumType.Md5;
            }
        }

        /// <summary>
        /// Calculates MD5 checksum of the data in the stream.
        /// </summary>
        /// <param name="data">Data fow which MD5 should be calculated</param>
        /// <returns>MD5 checksum of the data</returns>
        public string Calculate(Stream data)
        {
            using (var md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(data);
                return BitConverter.ToString(hash);
            }
        }
    }
}