namespace PlatformFacade
{
    /// <summary>
    /// Interface for platform initialization factory.
    /// Implementations are responsible for creating and initializing IPlatform instances.
    /// </summary>
    public interface IPlatformInitializer
    {
        /// <summary>
        /// Creates and initializes a platform instance
        /// </summary>
        /// <returns>The initialized platform instance</returns>
        IPlatform InitializePlatform();
    }
}
