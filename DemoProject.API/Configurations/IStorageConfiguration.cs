namespace DemoProject.API.Configurations
{
    /// <summary>
    /// Base interface for the  Storage Configuration.
    /// Primarily created to ease Mocking in Unit Tests
    /// </summary>
    public interface IStorageConfiguration
    {
        /// <summary>
        /// Gets a Path to where StorageRepositories should save files
        /// </summary>
        string Path { get; }
    }
}
