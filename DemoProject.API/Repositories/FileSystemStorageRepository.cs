using System;
using System.IO;

using DemoProject.API.Configurations;
using DemoProject.Common.Windsor;

namespace DemoProject.API.Repositories
{
    /// <summary>
    /// Storage repository accosiated with the file system.
    /// Saves\Reads files to\from the file system in accordance with the StorageConfiguration
    /// </summary>
    public class FileSystemStorageRepository : IStorageRepository
    {
        [Inject]
        public IStorageConfiguration StorageConfiguration { private get; set; }

        /// <summary>
        /// Save data to a file
        /// </summary>
        /// <param name="data">Data to save</param>
        /// <param name="uid">Name, given to the saved file</param>
        public void Put(Stream data, out Guid uid)
        {
            uid = Guid.NewGuid();
            string fullFilePath = Path.Combine(this.StorageConfiguration.Path, uid.ToString());

            // Create output directory, if not exists
            if (!Directory.Exists(this.StorageConfiguration.Path))
            {
                Directory.CreateDirectory(this.StorageConfiguration.Path);
            }

            // Save data from the stream to a file with GUID as a name
            var fileStream = File.Create(fullFilePath);
            data.CopyTo(fileStream);
            fileStream.Close();
        }

        /// <summary>
        /// Reads data from a file.
        /// </summary>
        /// <param name="uid">File name</param>
        /// <param name="data">Data from the file</param>
        /// <returns>False if file not exists</returns>
        public bool Get(Guid uid, out Stream data)
        {
            string fullFilePath = Path.Combine(this.StorageConfiguration.Path, uid.ToString());

            if (File.Exists(fullFilePath))
            {
                data = File.OpenRead(fullFilePath);
                return true;
            }

            data = null;
            return false;
        }

        /// <summary>
        /// Deletes file from the file system
        /// </summary>
        /// <param name="uid">File name</param>
        public void Delete(Guid uid)
        {
            string fullFilePath = Path.Combine(this.StorageConfiguration.Path, uid.ToString());

            File.Delete(fullFilePath);
        }
    }
}