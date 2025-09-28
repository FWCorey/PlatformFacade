using UnityEngine;

namespace PlatformFacade.Editor
{
    /// <summary>
    /// Editor implementation of IPlatform providing mock platform services for Unity Editor development and testing
    /// </summary>
    public class EditorPlatform : IPlatform
    {
        private readonly EditorPlatformSettings _settings;
        private readonly EditorUserService _userService;
        private readonly EditorMultiplayerService _multiplayerService;
        private readonly EditorStorage _storage;
        private readonly EditorLeaderboards _leaderboards;

        /// <summary>
        /// Gets the user service for managing authentication, friend lists, and user portraits
        /// </summary>
        public IUserService UserService => _userService;

        /// <summary>
        /// Gets the multiplayer service for managing multiplayer functionality
        /// </summary>
        public IMultiplayerService MultiplayerService => _multiplayerService;

        /// <summary>
        /// Gets the storage service for managing platform-specific data persistence
        /// </summary>
        public IStorage Storage => _storage;

        /// <summary>
        /// Gets the leaderboards service for managing leaderboard functionality
        /// </summary>
        public ILeaderboards Leaderboards => _leaderboards;

        /// <summary>
        /// Initializes a new instance of the EditorPlatform class
        /// </summary>
        /// <param name="settings">The editor platform settings to use for configuration</param>
        public EditorPlatform(EditorPlatformSettings settings = null)
        {
            // If no settings provided, try to find or create default settings
            _settings = settings ?? GetOrCreateDefaultSettings();

            // Initialize all services
            _userService = new EditorUserService(_settings);
            _multiplayerService = new EditorMultiplayerService(_settings);
            _storage = new EditorStorage(_settings);
            _leaderboards = new EditorLeaderboards(_settings);
        }

        /// <summary>
        /// Creates a new EditorPlatform instance with default settings
        /// </summary>
        /// <returns>A new EditorPlatform instance</returns>
        public static EditorPlatform CreateDefault()
        {
            return new EditorPlatform();
        }

        /// <summary>
        /// Creates a new EditorPlatform instance with specific settings
        /// </summary>
        /// <param name="settings">The settings to use</param>
        /// <returns>A new EditorPlatform instance</returns>
        public static EditorPlatform CreateWithSettings(EditorPlatformSettings settings)
        {
            return new EditorPlatform(settings);
        }

        private EditorPlatformSettings GetOrCreateDefaultSettings()
        {
            // Try to find existing settings asset
            var settingsAssets = Resources.LoadAll<EditorPlatformSettings>("");
            if (settingsAssets.Length > 0)
            {
                return settingsAssets[0];
            }

            // Create default settings in memory
            var defaultSettings = ScriptableObject.CreateInstance<EditorPlatformSettings>();
            
            // Note: In a real Unity environment, you might want to save this to the project
            // For now, we'll just use the in-memory instance with default values
            
            return defaultSettings;
        }

        /// <summary>
        /// Gets the current settings used by this EditorPlatform instance
        /// </summary>
        /// <returns>The current EditorPlatformSettings</returns>
        public EditorPlatformSettings GetSettings()
        {
            return _settings;
        }
    }
}