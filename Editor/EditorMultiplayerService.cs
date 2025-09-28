namespace PlatformFacade.Editor
{
    /// <summary>
    /// Editor implementation of IMultiplayerService providing mock multiplayer functionality for Unity Editor
    /// </summary>
    public class EditorMultiplayerService : IMultiplayerService
    {
        private readonly EditorPlatformSettings _settings;

        /// <summary>
        /// Initializes a new instance of the EditorMultiplayerService class
        /// </summary>
        /// <param name="settings">The editor platform settings to use</param>
        public EditorMultiplayerService(EditorPlatformSettings settings)
        {
            _settings = settings ?? throw new System.ArgumentNullException(nameof(settings));
        }

        // Note: IMultiplayerService interface is currently empty, but this class is ready for future expansion
    }
}