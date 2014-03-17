using System;
using DemoProject.Model;

namespace DemoProject.API.Models
{
    /// <summary>
    /// File Metadata information container for communications with UI.
    /// </summary>
    public class MetadataInfo
    {
        /// <summary>
        /// Id of this metadata in the database or other metadata storage.
        /// Can be Null in case when only ProcessingResult message is returned.
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Name of a file
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// File upload date\time (UTC)
        /// </summary>
        public DateTime AtUtc { get; set; }

        /// <summary>
        /// File processing result. Or other results (uploading results)
        /// </summary>
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