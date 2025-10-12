namespace PlatformFacade.Editor
{
    /// <summary>
    /// Factory for creating and initializing EditorPlatform instances.
    /// This class follows the Single Responsibility Principle by separating initialization logic
    /// from the platform implementation. It has no platform-specific fields and only constructs
    /// the platform.
    /// </summary>
    public class EditorPlatformInitializer : IPlatformInitializer
    {
        /// <summary>
        /// Creates and initializes the EditorPlatform with default settings
        /// </summary>
        /// <returns>The initialized EditorPlatform instance</returns>
        public IPlatform InitializePlatform()
        {
            return EditorPlatform.CreateDefault();
        }
    }
}
