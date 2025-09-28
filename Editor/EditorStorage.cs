namespace PlatformFacade.Editor
{
    /// <summary>
    /// Editor implementation of IStorage providing mock storage functionality for Unity Editor
    /// </summary>
    public class EditorStorage : IStorage
    {
        private readonly EditorPlatformSettings _settings;

        /// <summary>
        /// Initializes a new instance of the EditorStorage class
        /// </summary>
        /// <param name="settings">The editor platform settings to use</param>
        public EditorStorage(EditorPlatformSettings settings)
        {
            _settings = settings ?? throw new System.ArgumentNullException(nameof(settings));
        }

        // Note: IStorage interface is currently empty, but this class is ready for future expansion
    }
}