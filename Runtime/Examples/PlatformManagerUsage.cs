using System.Threading.Tasks;
using UnityEngine;

namespace PlatformFacade.Examples
{
    /// <summary>
    /// Example usage patterns for PlatformManager demonstrating runtime initialization
    /// and access to the platform facade
    /// </summary>
    public class PlatformManagerUsage
    {
        /// <summary>
        /// Example: Simple platform access using the PlatformManager
        /// </summary>
        public void SimpleAccess()
        {
            // The PlatformManager automatically initializes when accessing Current
            var platform = PlatformManager.Current;
            
            if (platform != null)
            {
                Debug.Log("Platform initialized successfully");
                
                // Access platform services
                var userService = platform.UserService;
                var leaderboards = platform.Leaderboards;
                var storage = platform.Storage;
                var multiplayer = platform.MultiplayerService;
            }
            else
            {
                Debug.LogWarning("No platform implementation available");
            }
        }

        /// <summary>
        /// Example: Manual initialization with error handling
        /// </summary>
        public void ManualInitialization()
        {
            if (!PlatformManager.IsInitialized)
            {
                Debug.Log("Initializing platform...");
                PlatformManager.Initialize();
            }
            
            var platform = PlatformManager.Current;
            if (platform != null)
            {
                Debug.Log($"Platform initialized: {platform.GetType().Name}");
            }
        }

        /// <summary>
        /// Example: Using PlatformManager in an async workflow
        /// </summary>
        public async Task AuthenticateAndLoadDataAsync()
        {
            // Access platform through PlatformManager
            var platform = PlatformManager.Current;
            
            if (platform == null)
            {
                Debug.LogError("Platform not available");
                return;
            }

            // Use Railway Oriented Design with the platform services
            var authResult = await platform.UserService.AuthenticateLocalUserAsync();
            
            if (authResult.IsSuccess)
            {
                Debug.Log($"Authenticated as: {authResult.Value.Name}");
                
                // Chain additional operations
                var friendsResult = await platform.UserService.GetFriendsListAsync();
                if (friendsResult.IsSuccess)
                {
                    Debug.Log($"Loaded {friendsResult.Value.Count} friends");
                }
            }
            else
            {
                Debug.LogError($"Authentication failed: {authResult.Error}");
            }
        }

        /// <summary>
        /// Example: Testing with a custom platform implementation
        /// </summary>
        public void SetCustomPlatform(IPlatform customPlatform)
        {
            // Manually set a platform instance (useful for testing)
            PlatformManager.SetPlatform(customPlatform);
            
            Debug.Log("Custom platform configured");
        }

        /// <summary>
        /// Example: Reset for testing purposes
        /// </summary>
        public void ResetForTesting()
        {
            // Reset the platform manager
            PlatformManager.Reset();
            
            Debug.Log("Platform manager reset");
        }
    }
}
