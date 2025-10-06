using UnityEngine;

namespace PlatformFacade
{
    /// <summary>
    /// Unity component that initializes the platform facade at runtime.
    /// Add this to a GameObject in your scene to automatically initialize the platform.
    /// </summary>
    public class RuntimePlatformInitializer : MonoBehaviour
    {
        [SerializeField] 
        [Tooltip("Initialize the platform when this component awakens")]
        private bool _initializeOnAwake = true;

        [SerializeField]
        [Tooltip("Make this GameObject persist across scene loads")]
        private bool _dontDestroyOnLoad = true;

        /// <summary>
        /// Gets whether the platform has been initialized
        /// </summary>
        public bool IsInitialized => PlatformManager.IsInitialized;

        private void Awake()
        {
            if (_initializeOnAwake)
            {
                InitializePlatform();
            }

            if (_dontDestroyOnLoad)
            {
                DontDestroyOnLoad(gameObject);
            }
        }

        /// <summary>
        /// Initializes the platform facade using the PlatformManager
        /// </summary>
        [ContextMenu("Initialize Platform")]
        public void InitializePlatform()
        {
            if (!PlatformManager.IsInitialized)
            {
                Debug.Log("RuntimePlatformInitializer: Initializing platform...");
                PlatformManager.Initialize();
            }
            else
            {
                Debug.Log("RuntimePlatformInitializer: Platform already initialized");
            }
        }

        /// <summary>
        /// Gets the current platform instance
        /// </summary>
        /// <returns>The current IPlatform instance, or null if not initialized</returns>
        public IPlatform GetPlatform()
        {
            return PlatformManager.Current;
        }
    }
}
