using System.Linq;
using DemoProject.Model;

namespace DemoProject.API.Repositories
{
    /// <summary>
    /// Base interface for File Metadata repositories.
    /// </summary>
    public interface IMetadataRepository
    {
        /// <summary>
        /// Returns all available File Metadata records.
        /// </summary>
        /// <returns>File Metadata records</returns>
        IQueryable<Metadata> GetAll();

        /// <summary>
        /// Deletes a File Metadata record with the particular ID.
        /// </summary>
        /// <param name="id">Metadata Id to delete</param>
        void Delete(int id);

        /// <summary>
        /// Adds a new File Metadata information to a storage.
        /// </summary>
        /// <param name="metadata">File Metadata information</param>
        /// <returns>File Metadata information with calculated Id</returns>
        Metadata Save(Metadata metadata);
    }
}
