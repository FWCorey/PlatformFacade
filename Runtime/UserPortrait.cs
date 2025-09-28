namespace PlatformFacade
{
    /// <summary>
    /// A concrete implementation of IUserPortrait
    /// </summary>
    public struct UserPortrait : IUserPortrait
    {
        /// <summary>
        /// The width of the portrait in pixels
        /// </summary>
        public int Width { get; }
        
        /// <summary>
        /// The height of the portrait in pixels
        /// </summary>
        public int Height { get; }
        
        /// <summary>
        /// The raw image data as bytes
        /// </summary>
        public byte[] ImageData { get; }
        
        /// <summary>
        /// The image format (e.g., PNG, JPEG)
        /// </summary>
        public string Format { get; }
        
        /// <summary>
        /// The URL of the image if available from the platform
        /// </summary>
        public string ImageUrl { get; }
        
        /// <summary>
        /// Indicates if the portrait data has been loaded
        /// </summary>
        public bool IsLoaded { get; }

        /// <summary>
        /// Initializes a new instance of the UserPortrait struct
        /// </summary>
        /// <param name="width">The width of the portrait in pixels</param>
        /// <param name="height">The height of the portrait in pixels</param>
        /// <param name="imageData">The raw image data as bytes</param>
        /// <param name="format">The image format</param>
        /// <param name="imageUrl">The URL of the image if available</param>
        /// <param name="isLoaded">Indicates if the portrait data has been loaded</param>
        public UserPortrait(int width, int height, byte[] imageData, string format, string imageUrl, bool isLoaded)
        {
            Width = width;
            Height = height;
            ImageData = imageData;
            Format = format ?? string.Empty;
            ImageUrl = imageUrl ?? string.Empty;
            IsLoaded = isLoaded;
        }
        
        /// <summary>
        /// Creates a UserPortrait with thumbnail dimensions (64x64)
        /// </summary>
        /// <param name="imageData">The raw image data as bytes</param>
        /// <param name="format">The image format</param>
        /// <param name="imageUrl">The URL of the image if available</param>
        /// <returns>A UserPortrait configured as a thumbnail</returns>
        public static UserPortrait CreateThumbnail(byte[] imageData, string format, string imageUrl = null)
        {
            return new UserPortrait(64, 64, imageData, format, imageUrl, imageData != null);
        }
    }
}