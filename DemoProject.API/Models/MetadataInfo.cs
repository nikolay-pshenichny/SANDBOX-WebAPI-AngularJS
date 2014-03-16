using System;
using DemoProject.Model;

namespace DemoProject.API.Models
{
    /// <summary>
    /// Metadata information container for communications with UI.
    /// </summary>
    public class MetadataInfo
    {
        public int Id { get; set; }

        public string FileName { get; set; }

        public DateTime AtUtc { get; set; }

        public string ProcessingResult { get; set; }

        public static MetadataInfo FromMetadata(Metadata metadata)
        {
            return new MetadataInfo
            {
                Id = metadata.Id,
                FileName = metadata.FileName,
                AtUtc = metadata.AtUtc,
                ProcessingResult = metadata.ProcessingResult
            };
        }
    }
}