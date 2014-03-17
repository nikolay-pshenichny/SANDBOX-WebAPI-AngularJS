using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

using DemoProject.API.ActionResults;
using DemoProject.API.Models;
using DemoProject.API.Repositories;
using DemoProject.Common.Windsor;
using DemoProject.Model;

namespace DemoProject.API.Controllers
{
    using DemoProject.API.Calculators;
    using DemoProject.API.Processors;

    /// <summary>
    /// <para>This API Controller handles file Uploads.</para>
    /// <para>All uploded files will stored on a storage and processed. All collected file 
    /// metadata, including processing results, will be stored in DB</para>
    /// </summary>
    public class UploadController : ApiController
    {
        [Inject]
        public IStorageRepository StorageRepository { private get; set; }

        [Inject]
        public IFileProcessor FileProcessor { private get; set; }

        [Inject]
        public IMetadataRepository MetadataRepository { private get; set; }

        [Inject]
        public IChecksumCalculator ChecksumCalculator { private get; set; }

        /// <summary>
        /// <para>Main Handler for all uploads.</para>
        /// It expects multipart data and accepts only Text files.
        /// </summary>
        /// <returns><para>Collected file metadata (List&lt;MetadataInfo&gt;. <see cref="DemoProject.API.Models.MetadataInfo"/>) 
        /// for the uploaded files if everything was uploaded correctly.</para>
        /// <para>UnsupportedMediaType in case when content is not a multipart content</para>
        /// </returns>
        [HttpPost]
        public async Task<IHttpActionResult> Post()
        {
            if (!this.Request.Content.IsMimeMultipartContent())
            {
                return this.StatusCode(HttpStatusCode.UnsupportedMediaType);
            }

            var memoryStreamProvider = new MultipartMemoryStreamProvider();
            await this.Request.Content.ReadAsMultipartAsync(memoryStreamProvider);

            List<MetadataInfo> results = new List<MetadataInfo>();
            foreach (HttpContent content in memoryStreamProvider.Contents)
            {
                // TODO: Validate received files here
                Stream data = await content.ReadAsStreamAsync();

                // Put newly uploaded file on a storage. StorageRepository will return UID associated with the file.
                Guid fileUidOnStorage;
                this.StorageRepository.Put(data, out fileUidOnStorage);

                // Process newly uploaded file and get processing results
                data.Position = 0;
                string processingResult = this.FileProcessor.Process(data);

                // Calculate file's checksum
                data.Position = 0;
                string checksum = this.ChecksumCalculator.Calculate(data);

                // Prepare metadata object and save it.
                var metadata =
                    this.MetadataRepository.Save(
                        new Metadata
                            {
                                FileName = content.Headers.ContentDisposition.FileName,
                                AtUtc = DateTime.UtcNow,
                                ProcessingResult = processingResult,
                                FileStorageUid = fileUidOnStorage,
                                ChecksumType = this.ChecksumCalculator.ChecksumType,
                                Checksum = checksum
                            });

                results.Add(MetadataInfo.FromMetadata(metadata));
            }

            // Return file metadata for all uploaded files
            return new GenericValueResult<List<MetadataInfo>>(results, this.Request);
        }
    }
}
