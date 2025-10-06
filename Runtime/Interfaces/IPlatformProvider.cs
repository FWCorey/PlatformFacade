namespace PlatformFacade
{
    /// <summary>
    /// Interface for providing access to an initialized IPlatform instance.
    /// Platform initializers should implement this interface to expose their platform.
    /// </summary>
    public interface IPlatformProvider
    {
        /// <summary>
        /// Gets the initialized platform instance
        /// </summary>
        IPlatform Platform { get; }
    }
}
