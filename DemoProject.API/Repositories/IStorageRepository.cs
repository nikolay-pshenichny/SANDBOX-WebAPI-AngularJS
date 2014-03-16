using System;
using System.IO;

namespace DemoProject.API.Repositories
{
    /// <summary>
    /// Base interface for all StorageRepositories
    /// </summary>
    public interface IStorageRepository
    {
        /// <summary>
        /// Saves data from the stream and returns an UID associated with the saved data.
        /// </summary>
        /// <param name="data">Data to save</param>
        /// <param name="uid">UID associated with the saved data</param>
        void Put(Stream data, out Guid uid);

        /// <summary>
        /// Gets data by UID associated with It.
        /// </summary>
        /// <param name="uid">Uid associated with the data that is requested</param>
        /// <param name="data">Loaded Data</param>
        /// <returns>True if data was loaded successfully</returns>
        bool Get(Guid uid, out Stream data);

        /// <summary>
        /// Deletes data from a storage by the Uid associated with the data.
        /// </summary>
        /// <param name="uid">Data's Uid</param>
        void Delete(Guid uid);
    }
}
