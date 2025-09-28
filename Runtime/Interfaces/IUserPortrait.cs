namespace PlatformFacade
{
    /// <summary>
    /// Represents a user portrait/avatar image across gaming platforms
    /// </summary>
    public interface IUserPortrait
    {
        /// <summary>
        /// The width of the portrait in pixels
        /// </summary>
        int Width { get; }
        
        /// <summary>
        /// The height of the portrait in pixels
        /// </summary>
        int Height { get; }
        
        /// <summary>
        /// The raw image data as bytes
        /// </summary>
        byte[] ImageData { get; }
        
        /// <summary>
        /// The image format (e.g., PNG, JPEG)
        /// </summary>
        string Format { get; }
        
        /// <summary>
        /// The URL of the image if available from the platform
        /// </summary>
        string ImageUrl { get; }
        
        /// <summary>
        /// Indicates if the portrait data has been loaded
        /// </summary>
        bool IsLoaded { get; }
    }
}