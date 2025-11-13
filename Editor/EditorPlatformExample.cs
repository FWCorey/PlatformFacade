using System.Threading.Tasks;
using UnityEngine;

namespace PlatformFacade.Editor
{
    /// <summary>
    /// Example usage of EditorPlatform for testing and development in Unity Editor.
    /// Note: This is a MonoBehaviour example and will not be discovered by PlatformManager's reflection.
    /// For automatic discovery, use EditorPlatformInitializer instead.
    /// </summary>
    public class EditorPlatformExample : MonoBehaviour
    {
        [SerializeField] private EditorPlatformSettings _settings;
        
        private EditorPlatform _platform;

        public bool PlatformInitialized => _platform != null;

        private void Start()
        {
            InitializePlatformInternal();
        }

        private void InitializePlatformInternal()
        {
            // Create platform with settings (or use default if null)
            _platform = _settings != null 
                ? EditorPlatform.CreateWithSettings(_settings)
                : EditorPlatform.CreateDefault();

            Debug.Log("EditorPlatform initialized successfully");
        }

        /// <summary>
        /// Example method to test user authentication
        /// </summary>
        [ContextMenu("Test User Authentication")]
        public async void TestUserAuthentication()
        {
            if (_platform?.UserService == null)
            {
                Debug.LogError("Platform not initialized");
                return;
            }

            var userService = _platform.UserService;
            
            Debug.Log("Starting authentication...");
            var authResult = await userService.AuthenticateLocalUserAsync();
            
            if (authResult.IsSuccess)
            {
                var user = authResult.Value;
                Debug.Log($"Authentication successful! User: {user.Name} (ID: {user.UserID})");
            }
            else
            {
                Debug.LogError($"Authentication failed: {authResult.Error}");
            }
        }

        /// <summary>
        /// Example method to test loading user portrait
        /// </summary>
        [ContextMenu("Test Load User Portrait")]
        public async void TestLoadUserPortrait()
        {
            if (_platform?.UserService == null)
            {
                Debug.LogError("Platform not initialized");
                return;
            }

            var userService = _platform.UserService;
            
            Debug.Log("Loading user portrait...");
            var portraitResult = await userService.GetUserPortraitThumbnailAsync();
            
            if (portraitResult.IsSuccess)
            {
                var texture = portraitResult.Value;
                Debug.Log($"Portrait loaded successfully: {texture.width}x{texture.height}");
            }
            else
            {
                Debug.LogError($"Failed to load portrait: {portraitResult.Error}");
            }
        }

        /// <summary>
        /// Example method to test leaderboard functionality
        /// </summary>
        [ContextMenu("Test Leaderboard Operations")]
        public async void TestLeaderboardOperations()
        {
            if (_platform?.Leaderboards == null)
            {
                Debug.LogError("Platform not initialized");
                return;
            }

            var leaderboards = _platform.Leaderboards;
            const ulong testLeaderboardID = 1001;

            // Submit a score
            Debug.Log("Submitting score...");
            var submitResult = await leaderboards.SubmitScoreAsync(testLeaderboardID, 8500);
            
            if (submitResult.IsSuccess)
            {
                Debug.Log("Score submitted successfully!");
            }
            else
            {
                Debug.LogError($"Score submission failed: {submitResult.Error}");
                return;
            }

            // Get global leaderboard
            Debug.Log("Loading global leaderboard...");
            var leaderboardResult = await leaderboards.GetGlobalLeaderboardAsync(testLeaderboardID, 5);
            
            if (leaderboardResult.IsSuccess)
            {
                var leaderboard = leaderboardResult.Value;
                Debug.Log($"Global leaderboard loaded: {leaderboard.DisplayName} ({leaderboard.Count} entries)");
                
                foreach (var entry in leaderboard)
                {
                    Debug.Log($"  Rank {entry.Rank}: {entry.User.Name} - {entry.Score} points");
                }
            }
            else
            {
                Debug.LogError($"Failed to load leaderboard: {leaderboardResult.Error}");
            }

            // Get user's rank
            Debug.Log("Getting user's rank...");
            var userEntryResult = await leaderboards.GetUserEntryAsync(testLeaderboardID);
            
            if (userEntryResult.IsSuccess)
            {
                var userEntry = userEntryResult.Value;
                Debug.Log($"User rank: {userEntry.Rank} with score {userEntry.Score}");
            }
            else
            {
                Debug.LogError($"Failed to get user rank: {userEntryResult.Error}");
            }
        }

        /// <summary>
        /// Example method to test friends list functionality
        /// </summary>
        [ContextMenu("Test Friends List")]
        public async void TestFriendsList()
        {
            if (_platform?.UserService == null)
            {
                Debug.LogError("Platform not initialized");
                return;
            }

            var userService = _platform.UserService;
            
            Debug.Log("Loading friends list...");
            var friendsResult = await userService.GetFriendsListAsync();
            
            if (friendsResult.IsSuccess)
            {
                var friends = friendsResult.Value;
                Debug.Log($"Friends list loaded: {friends.Count} friends");
                
                foreach (var friend in friends)
                {
                    Debug.Log($"  Friend: {friend.Name} ({friend.GamerTag})");
                }
            }
            else
            {
                Debug.LogError($"Failed to load friends list: {friendsResult.Error}");
            }
        }

        /// <summary>
        /// Example method to test complete workflow using Railway Oriented Design
        /// </summary>
        [ContextMenu("Test Complete Workflow")]
        public async void TestCompleteWorkflow()
        {
            if (_platform?.UserService == null || _platform?.Leaderboards == null)
            {
                Debug.LogError("Platform not initialized");
                return;
            }

            Debug.Log("Starting complete workflow test...");

            try
            {
                // Chain operations using Railway Oriented Design pattern
                var userService = _platform.UserService;
                var leaderboards = _platform.Leaderboards;

                // Authenticate -> Get Friends -> Submit Score -> Get Updated Leaderboard
                var authResult = await userService.AuthenticateLocalUserAsync();
                
                if (authResult.IsSuccess)
                {
                    Debug.Log($"✓ Authenticated as: {authResult.Value.Name}");

                    var friendsResult = await userService.GetFriendsListAsync();
                    if (friendsResult.IsSuccess)
                    {
                        Debug.Log($"✓ Loaded {friendsResult.Value.Count} friends");

                        var scoreResult = await leaderboards.SubmitScoreAsync(1001, 9999);
                        if (scoreResult.IsSuccess)
                        {
                            Debug.Log("✓ Score submitted successfully");

                            var leaderboardResult = await leaderboards.GetFriendsLeaderboardAsync(1001, 10);
                            if (leaderboardResult.IsSuccess)
                            {
                                Debug.Log($"✓ Complete workflow successful! Friends leaderboard has {leaderboardResult.Value.Count} entries");
                            }
                            else
                            {
                                Debug.LogError($"✗ Friends leaderboard failed: {leaderboardResult.Error}");
                            }
                        }
                        else
                        {
                            Debug.LogError($"✗ Score submission failed: {scoreResult.Error}");
                        }
                    }
                    else
                    {
                        Debug.LogError($"✗ Friends list failed: {friendsResult.Error}");
                    }
                }
                else
                {
                    Debug.LogError($"✗ Authentication failed: {authResult.Error}");
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Workflow exception: {ex.Message}");
            }
        }
    }
}
