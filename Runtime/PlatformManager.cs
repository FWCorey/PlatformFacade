using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace PlatformFacade
{
    /// <summary>
    /// Manages the runtime initialization and provides access to the platform facade.
    /// Uses reflection to automatically discover and initialize IPlatformInitializer implementations.
    /// </summary>
    public static class PlatformManager
    {
        private static IPlatform _currentPlatform;
        private static bool _isInitialized = false;

        /// <summary>
        /// Gets the current platform instance. Initializes automatically if not already initialized.
        /// </summary>
        public static IPlatform Current
        {
            get
            {
                if (!_isInitialized)
                {
                    Initialize();
                }
                return _currentPlatform;
            }
        }

        /// <summary>
        /// Gets whether the platform has been initialized.
        /// </summary>
        public static bool IsInitialized => _isInitialized;

        /// <summary>
        /// Initializes the platform facade by using reflection to find an IPlatformInitializer implementation.
        /// Logs an error if more than one implementation is found.
        /// </summary>
        public static void Initialize()
        {
            if (_isInitialized)
            {
                Debug.LogWarning("PlatformManager: Platform already initialized.");
                return;
            }

            try
            {
                // Find all types that implement IPlatformInitializer
                var initializerTypes = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(assembly => 
                    {
                        try
                        {
                            return assembly.GetTypes();
                        }
                        catch (ReflectionTypeLoadException)
                        {
                            // Some assemblies may fail to load all types, skip them
                            return Array.Empty<Type>();
                        }
                    })
                    .Where(type => 
                        typeof(IPlatformInitializer).IsAssignableFrom(type) && 
                        !type.IsInterface && 
                        !type.IsAbstract &&
                        !typeof(MonoBehaviour).IsAssignableFrom(type)) // Exclude MonoBehaviour types
                    .ToList();

                if (initializerTypes.Count == 0)
                {
                    Debug.LogError("PlatformManager: No IPlatformInitializer implementation found. Please ensure a platform initializer is available.");
                    return;
                } else {
                    foreach (var type in initializerTypes) {
                        Debug.Log($"PlatformManager: Found IPlatformInitializer implementation: {type.FullName}");
                    }
                }

                // Log a warning if multiple initializers are found, try all until we get a platform
                // User may want to handle optional platform initializers in their own way
                if (initializerTypes.Count > 1) {
                    var typeNames = string.Join(", ", initializerTypes.Select(t => t.FullName));
                    Debug.LogWarning($"PlatformManager: Multiple IPlatformInitializer implementations found: {typeNames}. Getting first successful initialization.");
                }
                for (int i = 0; i < initializerTypes.Count; i++) {
                    // Create an instance of the found initializer type
                    var initializerType = initializerTypes[i];

                    // Try to create instance using parameterless constructor and initialize platform
                    try {
                        var platformInitializer = (IPlatformInitializer)Activator.CreateInstance(initializerType);

                        // Initialize the platform and get the instance
                        _currentPlatform = platformInitializer.InitializePlatform();

                        if (_currentPlatform != null) {
                            Debug.Log($"PlatformManager: Successfully initialized platform via {initializerType.FullName}");
                            _isInitialized = true;
                            return;
                        } else {
                            Debug.Log($"PlatformManager: Initializer {initializerType.FullName} returned null platform instance.");
                        }
                    } catch (Exception ex) {
                        Debug.LogError($"PlatformManager: Failed to create or initialize {initializerType.FullName}. Error: {ex.Message}");
                    }
                }
                Debug.LogError("PlatformManager: All IPlatformInitializer implementations failed to initialize a platform instance.");
            }
            catch (Exception ex)
            {
                Debug.LogError($"PlatformManager: Initialization failed with exception: {ex.Message}");
            }
        }

        /// <summary>
        /// Manually sets the platform instance. Useful for testing or custom initialization scenarios.
        /// </summary>
        /// <param name="platform">The platform instance to use</param>
        public static void SetPlatform(IPlatform platform)
        {
            _currentPlatform = platform;
            _isInitialized = true;
            
            if (platform != null)
            {
                Debug.Log($"PlatformManager: Platform manually set to {platform.GetType().FullName}");
            }
        }

        /// <summary>
        /// Resets the platform manager, allowing reinitialization. Primarily for testing purposes.
        /// </summary>
        public static void Reset()
        {
            _currentPlatform = null;
            _isInitialized = false;
            Debug.Log("PlatformManager: Reset completed");
        }
    }
}
