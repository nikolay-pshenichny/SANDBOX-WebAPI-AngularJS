using System.IO;
using System.Linq;
using System.Web.Http;

using DemoProject.API.ActionResults;
using DemoProject.API.Repositories;
using DemoProject.Common.Windsor;

namespace DemoProject.API.Controllers
{
    /// <summary>
    /// This API Controller handles file Downloads.
    /// </summary>
    public class DownloadController : ApiController
    {
        [Inject]
        public IMetadataRepository MetadataRepository { private get; set; }

        [Inject]
        public IStorageRepository StorageRepository { private get; set; }

        /// <summary>
        /// Returns a file associated with the provided Metadata.Id.
        /// </summary>
        /// <param name="id">Metadata.Id associated with the file that should be downloaded</param>
        /// <returns>File content as an attachment</returns>
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            // Search for the requested metadata Id
            var metadata = this.MetadataRepository.GetAll().FirstOrDefault(x => x.Id == id);

            // If found, attach the file associated with metadata to the Response
            if (metadata != null)
            {
                Stream stream;
                if (this.StorageRepository.Get(metadata.FileStorageUid, out stream))
                {
                    return new Attachment(stream, metadata.FileName, this.Request);
                }
            }

            return this.NotFound();
        }
    }
}