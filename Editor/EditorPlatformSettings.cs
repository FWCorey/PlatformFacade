using UnityEngine;

namespace PlatformFacade.Editor
{
    /// <summary>
    /// ScriptableObject for configuring EditorPlatform settings such as user portrait and mock data
    /// </summary>
    [CreateAssetMenu(fileName = "EditorPlatformSettings", menuName = "Platform Facade/Editor Platform Settings")]
    public class EditorPlatformSettings : ScriptableObject
    {
        [SerializeField] private bool _enableEditorPlatform = true;
        [Header("User Settings")]
        [SerializeField] private string _userName = "EditorUser";
        [SerializeField] private string _gamerTag = "DevUser";
        [SerializeField] private ulong _userID = 12345;
        [SerializeField] private Texture2D _userPortrait;
        
        [Header("Mock Data Settings")]
        [SerializeField] private bool _simulateAuthentication = true;
        [SerializeField] private float _authenticationDelay = 1.0f;
        [SerializeField] private bool _simulateNetworkDelay = true;
        [SerializeField] private float _networkDelayMin = 0.1f;
        [SerializeField] private float _networkDelayMax = 0.5f;

        /// <summary>
        /// Gets the configured user name for the editor user
        /// </summary>
        public string UserName => _userName;

        /// <summary>
        /// Gets the configured gamer tag for the editor user
        /// </summary>
        public string GamerTag => _gamerTag;

        /// <summary>
        /// Gets the configured user ID for the editor user
        /// </summary>
        public ulong UserID => _userID;

        /// <summary>
        /// Gets the configured user portrait texture
        /// </summary>
        public Texture2D UserPortrait => _userPortrait;

        /// <summary>
        /// Gets whether authentication should be simulated
        /// </summary>
        public bool SimulateAuthentication => _simulateAuthentication;

        /// <summary>
        /// Gets the delay for authentication simulation
        /// </summary>
        public float AuthenticationDelay => _authenticationDelay;

        /// <summary>
        /// Gets whether network delay should be simulated
        /// </summary>
        public bool SimulateNetworkDelay => _simulateNetworkDelay;

        /// <summary>
        /// Gets the minimum network delay for simulation
        /// </summary>
        public float NetworkDelayMin => _networkDelayMin;

        /// <summary>
        /// Gets the maximum network delay for simulation
        /// </summary>
        public float NetworkDelayMax => _networkDelayMax;

        public bool EditorPlatformEnabled => _enableEditorPlatform;

        /// <summary>
        /// Gets a random network delay value within the configured range
        /// </summary>
        /// <returns>Random delay value in seconds</returns>
        public float GetRandomNetworkDelay()
        {
            if (!_simulateNetworkDelay)
                return 0f;
            
            return Random.Range(_networkDelayMin, _networkDelayMax);
        }
    }
}