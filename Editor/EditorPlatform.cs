using UnityEngine;
using UnityEditor;

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
        private readonly EditorAchievements _achievements;

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
        /// Gets the achievements service for managing achievement functionality.
        /// Returns null if the platform does not support achievements.
        /// </summary>
        public IAchievements Achievements => _achievements;

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
            _achievements = new EditorAchievements(_settings);
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

            // Try to find existing settings asset using AssetDatabase
            var guids = AssetDatabase.FindAssets("t:EditorPlatformSettings", new[] { "Assets/Editor/Config" });
            if (guids.Length > 0)
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[0]);
                var settings = AssetDatabase.LoadAssetAtPath<EditorPlatformSettings>(path);
                if (settings != null)
                {
                    return settings;
                }
            }

            // Create and save default settings to Assets/Editor/Config
            var defaultSettings = ScriptableObject.CreateInstance<EditorPlatformSettings>();
            
            // Ensure directory exists
            var configPath = "Assets/Editor/Config";
            if (!AssetDatabase.IsValidFolder(configPath))
            {
                // Create Editor folder if it doesn't exist
                if (!AssetDatabase.IsValidFolder("Assets/Editor"))
                {
                    AssetDatabase.CreateFolder("Assets", "Editor");
                }
                // Create Config folder
                AssetDatabase.CreateFolder("Assets/Editor", "Config");
            }
            
            // Save the asset
            var assetPath = $"{configPath}/DefaultEditorPlatformSettings.asset";
            AssetDatabase.CreateAsset(defaultSettings, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
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
