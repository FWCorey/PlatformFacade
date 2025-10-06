namespace PlatformFacade.Editor
{
    /// <summary>
    /// Runtime initializer for EditorPlatform that can be discovered via reflection.
    /// This class follows the Single Responsibility Principle by separating initialization logic
    /// from the platform implementation.
    /// </summary>
    public class EditorPlatformInitializer : IPlatformInitializer, IPlatformProvider
    {
        private EditorPlatform _platform;

        /// <summary>
        /// Gets whether the platform has been initialized
        /// </summary>
        public bool PlatformInitialized => _platform != null;

        /// <summary>
        /// Gets the initialized platform instance
        /// </summary>
        public IPlatform Platform => _platform;

        /// <summary>
        /// Initializes the EditorPlatform with default settings
        /// </summary>
        public void InitializePlatform()
        {
            if (_platform == null)
            {
                _platform = EditorPlatform.CreateDefault();
            }
        }
    }
}
