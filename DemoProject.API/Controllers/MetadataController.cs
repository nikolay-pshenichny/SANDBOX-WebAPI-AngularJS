using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

using DemoProject.API.ActionResults;
using DemoProject.API.Models;
using DemoProject.API.Repositories;
using DemoProject.Common.Windsor;

namespace DemoProject.API.Controllers
{
    /// <summary>
    /// <para>This API Controller handles all file Metadata manipulations.</para>
    /// <para>File metadata information includes: Checksum, FileName, DateTime when uploaded and Processing Results.</para>
    /// </summary>
    public class MetadataController : ApiController
    {
        [Inject]
        public IMetadataRepository MetadataRepository { private get; set; }

        [Inject]
        public IStorageRepository StorageRepository { private get; set; }

        /// <summary>
        /// Returns all available file metadata, including processing results
        /// which were calculated during file Upload operation.
        /// </summary>
        /// <returns>List of MetadataInfo objects (IEnumerable&lt;MetadataInfo&gt;)</returns>
        [HttpGet]
        public IHttpActionResult Get()
        {
            IEnumerable<MetadataInfo> result = this.MetadataRepository.GetAll().AsEnumerable().Select(MetadataInfo.FromMetadata);
            return new GenericValueResult<IEnumerable<MetadataInfo>>(result, this.Request);
        }

        /// <summary>
        /// Returns a file metadata object (MetadataInfo) that corresponds to the requested ID.
        /// </summary>
        /// <param name="id">Metadata Id</param>
        /// <returns>Single file metadata object (<see cref="DemoProject.API.Models.MetadataInfo"/>) or NotFound()</returns>
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var metadata = this.MetadataRepository.GetAll().FirstOrDefault(x => x.Id == id);
            if (metadata != null)
            {
                return new GenericValueResult<MetadataInfo>(MetadataInfo.FromMetadata(metadata), this.Request);
            }

            return this.NotFound();
        }

        /// <summary>
        /// Deletes the file metadata from the database and removes the file associates with it from a Storage
        /// </summary>
        /// <param name="id">Metadata Id</param>
        /// <returns>OK if metadata was deleted or NotFound if it was not found</returns>
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            var metadata = this.MetadataRepository.GetAll().FirstOrDefault(x => x.Id == id);
            if (metadata != null)
            {
                this.StorageRepository.Delete(metadata.FileStorageUid);
                
                this.MetadataRepository.Delete(id);

                return this.Ok();
            }

            return this.NotFound();
        }
    }
}
