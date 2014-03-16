using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using DemoProject.API.Models;
using DemoProject.API.Repositories;
using DemoProject.Common.Windsor;

namespace DemoProject.API.Controllers
{
    /// <summary>
    /// This API Controller is responsible for Metadata manipulations.
    /// (CRUD, but without Create and Update. Uploads should be used to create a new metadata)
    /// </summary>
    public class MetadataController : ApiController
    {
        [Inject]
        public IMetadataRepository MetadataRepository { private get; set; }

        [Inject]
        public IStorageRepository StorageRepository { private get; set; }

        /// <summary>
        /// Returns all available file metadata, including processing results
        /// which were calculated during Upload
        /// </summary>
        /// <returns>File metadata</returns>
        [HttpGet]
        public IEnumerable<MetadataInfo> Get()
        {
            return this.MetadataRepository.GetAll().AsEnumerable().Select(MetadataInfo.FromMetadata);
        }

        /// <summary>
        /// Returns file metadata with the particular ID, or nothing if not found
        /// </summary>
        /// <param name="id">Metadata Id</param>
        /// <returns>File metadata</returns>
        [HttpGet]
        public MetadataInfo Get(int id)
        {
            var metadata = this.MetadataRepository.GetAll().FirstOrDefault(x => x.Id == id);
            return metadata != null ? MetadataInfo.FromMetadata(metadata) : null;
        }

        /// <summary>
        /// Deletes the metadata from the database and removes associates with it file from Storage
        /// </summary>
        /// <param name="id">Metadata Id</param>
        [HttpDelete]
        public void Delete(int id)
        {
            var metadata = this.MetadataRepository.GetAll().FirstOrDefault(x => x.Id == id);
            if (metadata != null)
            {
                this.StorageRepository.Delete(metadata.FileStorageUid);
                
                this.MetadataRepository.Delete(id);
            }
        }
    }
}
