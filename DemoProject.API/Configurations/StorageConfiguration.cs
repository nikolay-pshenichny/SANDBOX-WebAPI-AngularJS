using DemoProject.API.Properties;

namespace DemoProject.API.Configurations
{
    /// <summary>
    /// Main IStorageConfiguration implementation.
    /// Associated with application settings (applicationSettings in web.config).
    /// </summary>
    public class StorageConfiguration : IStorageConfiguration
    {
        public string Path
        {
            get
            {
                return Settings.Default.StoragePath;
            }
        }
    }
}