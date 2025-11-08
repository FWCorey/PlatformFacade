using System.Threading.Tasks;
using UnityEngine;

namespace PlatformFacade.Examples
{
    /// <summary>
    /// Manual test component for PlatformManager functionality.
    /// Add this to a GameObject in your scene to test runtime initialization.
    /// </summary>
    public class PlatformManagerTest : MonoBehaviour
    {
        [Header("Test Results")]
        [SerializeField] private bool _initializationSuccessful;
        [SerializeField] private string _platformTypeName;
        [SerializeField] private string _testStatus;

        private void Start()
        {
            RunTests();
        }

        /// <summary>
        /// Runs all PlatformManager tests
        /// </summary>
        [ContextMenu("Run Tests")]
        public void RunTests()
        {
            Debug.Log("=== PlatformManager Tests ===");
            
            TestAutoInitialization();
            TestPlatformAccess();
            TestManualInitialization();
            
            Debug.Log("=== Tests Complete ===");
        }

        /// <summary>
        /// Test 1: Verify automatic initialization works
        /// </summary>
        [ContextMenu("Test Auto Initialization")]
        private void TestAutoInitialization()
        {
            Debug.Log("Test 1: Auto Initialization");
            
            // Reset first to ensure clean state
            PlatformManager.Reset();
            
            // Access Current should trigger initialization
            var platform = PlatformManager.Current;
            
            if (PlatformManager.IsInitialized)
            {
                Debug.Log("✓ Platform initialized automatically");
                _initializationSuccessful = true;
            }
            else
            {
                Debug.LogError("✗ Platform failed to initialize automatically");
                _initializationSuccessful = false;
            }

            if (platform != null)
            {
                _platformTypeName = platform.GetType().FullName;
                Debug.Log($"✓ Platform type: {_platformTypeName}");
            }
            else
            {
                _platformTypeName = "null";
                Debug.Log("ℹ No platform implementation found (expected if no IPlatform implementation is in the project)");
            }
        }

        /// <summary>
        /// Test 2: Verify platform access and services
        /// </summary>
        [ContextMenu("Test Platform Access")]
        private void TestPlatformAccess()
        {
            Debug.Log("Test 2: Platform Access");
            
            var platform = PlatformManager.Current;
            
            if (platform == null)
            {
                Debug.Log("ℹ No platform available for service testing");
                _testStatus = "No platform implementation";
                return;
            }

            try
            {
                var userService = platform.UserService;
                var leaderboards = platform.Leaderboards;
                var storage = platform.Storage;
                var multiplayer = platform.MultiplayerService;
                var achievements = platform.Achievements;

                Debug.Log($"✓ UserService: {userService != null}");
                Debug.Log($"✓ Leaderboards: {leaderboards != null}");
                Debug.Log($"✓ Storage: {storage != null}");
                Debug.Log($"✓ MultiplayerService: {multiplayer != null}");
                Debug.Log($"✓ Achievements: {achievements != null}");
                
                _testStatus = "All services accessible";
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"✗ Service access failed: {ex.Message}");
                _testStatus = $"Error: {ex.Message}";
            }
        }

        /// <summary>
        /// Test 3: Verify manual initialization
        /// </summary>
        [ContextMenu("Test Manual Initialization")]
        private void TestManualInitialization()
        {
            Debug.Log("Test 3: Manual Initialization");
            
            // Reset and manually initialize
            PlatformManager.Reset();
            
            if (!PlatformManager.IsInitialized)
            {
                Debug.Log("✓ Reset successful");
            }
            
            PlatformManager.Initialize();
            _initializationSuccessful = PlatformManager.IsInitialized;
            if (PlatformManager.IsInitialized)
            {
                Debug.Log("✓ Manual initialization successful");
            }
            else
            {
                Debug.LogError("✗ Manual initialization failed");
            }
        }

        /// <summary>
        /// Test 4: Test async operations with platform
        /// </summary>
        [ContextMenu("Test Async Operations")]
        public async void TestAsyncOperations()
        {
            Debug.Log("Test 4: Async Operations");
            
            var platform = PlatformManager.Current;
            
            if (platform == null)
            {
                Debug.Log("ℹ No platform available for async testing");
                return;
            }

            try
            {
                // Test user authentication
                Debug.Log("Testing user authentication...");
                var authResult = await platform.UserService.AuthenticateLocalUserAsync();
                
                if (authResult.IsSuccess)
                {
                    Debug.Log($"✓ Authentication successful: {authResult.Value.Name}");
                    
                    // Test friends list
                    Debug.Log("Testing friends list...");
                    var friendsResult = await platform.UserService.GetFriendsListAsync();
                    
                    if (friendsResult.IsSuccess)
                    {
                        Debug.Log($"✓ Friends list loaded: {friendsResult.Value.Count} friends");
                    }
                    else
                    {
                        Debug.Log($"ℹ Friends list error: {friendsResult.Error}");
                    }
                }
                else
                {
                    Debug.Log($"ℹ Authentication error: {authResult.Error}");
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"✗ Async operation failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Test 5: Test SetPlatform functionality
        /// </summary>
        [ContextMenu("Test Set Platform")]
        private void TestSetPlatform()
        {
            Debug.Log("Test 5: Set Platform");
            
            // Create a mock platform for testing
            var mockPlatform = CreateMockPlatform();
            
            PlatformManager.SetPlatform(mockPlatform);
            
            var platform = PlatformManager.Current;
            
            if (platform == mockPlatform)
            {
                Debug.Log("✓ SetPlatform successful");
            }
            else
            {
                Debug.LogError("✗ SetPlatform failed");
            }
            
            // Reset for normal operation
            PlatformManager.Reset();
        }

        /// <summary>
        /// Creates a simple mock platform for testing (if EditorPlatform is available)
        /// </summary>
        private IPlatform CreateMockPlatform()
        {
            // Try to create EditorPlatform via its initializer if available
            var editorInitializerType = System.Type.GetType("PlatformFacade.Editor.EditorPlatformInitializer, PlatformFacade.Editor");
            if (editorInitializerType != null)
            {
                try
                {
                    var initializer = (IPlatformInitializer)System.Activator.CreateInstance(editorInitializerType);
                    return initializer.InitializePlatform();
                }
                catch
                {
                    // If EditorPlatformInitializer creation fails, return null
                }
            }
            
            return null;
        }
    }
}
