using System;
using System.Threading.Tasks;
using UnityEngine;

namespace PlatformFacade.Editor
{
    /// <summary>
    /// Editor implementation of IUserService providing mock user functionality for Unity Editor
    /// </summary>
    public class EditorUserService : IUserService
    {
        private readonly EditorPlatformSettings _settings;
        private LocalUser _localUser;
        private FriendsList _friendsList;
        private bool _isInitialized;

        /// <summary>
        /// Event fired when the local user's authentication status changes
        /// </summary>
        public event Action<UserAuthenticationStatus> AuthenticationStatusChanged;
        
        /// <summary>
        /// Event fired when the local user's friendlist is updated
        /// </summary>
        public event Action<IFriendsList> FriendsListUpdated;

        /// <summary>
        /// Initializes a new instance of the EditorUserService class
        /// </summary>
        /// <param name="settings">The editor platform settings to use</param>
        public EditorUserService(EditorPlatformSettings settings)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            InitializeLocalUser();
            InitializeFriendsList();
        }

        /// <summary>
        /// Gets the current local user
        /// </summary>
        /// <returns>The local user if available, null otherwise</returns>
        public IAuthenticatedUser GetLocalUser()
        {
            return _localUser;
        }

        /// <summary>
        /// Authenticates the local user asynchronously
        /// </summary>
        /// <returns>A task containing the authentication result</returns>
        public async Task<Result<IAuthenticatedUser, string>> AuthenticateLocalUserAsync()
        {
            if (!_settings.SimulateAuthentication)
            {
                var authenticatedUser = new LocalUser(_localUser.Name, _localUser.GamerTag, _localUser.UserID, UserAuthenticationStatus.Authenticated);
                _localUser = authenticatedUser;
                AuthenticationStatusChanged?.Invoke(UserAuthenticationStatus.Authenticated);
                return new Result<IAuthenticatedUser, string>(authenticatedUser);
            }

            // Simulate authentication process
            var authenticatingUser = new LocalUser(_localUser.Name, _localUser.GamerTag, _localUser.UserID, UserAuthenticationStatus.Authenticating);
            _localUser = authenticatingUser;
            AuthenticationStatusChanged?.Invoke(UserAuthenticationStatus.Authenticating);

            // Wait for authentication delay
            await Task.Delay(Mathf.RoundToInt(_settings.AuthenticationDelay * 1000));

            // Complete authentication
            var successUser = new LocalUser(_localUser.Name, _localUser.GamerTag, _localUser.UserID, UserAuthenticationStatus.Authenticated);
            _localUser = successUser;
            AuthenticationStatusChanged?.Invoke(UserAuthenticationStatus.Authenticated);

            return new Result<IAuthenticatedUser, string>(successUser);
        }

        /// <summary>
        /// Gets a readonly reference to the local user's friendlist
        /// </summary>
        /// <returns>A task containing the friendlist if available, error message otherwise</returns>
        public async Task<Result<IFriendsList, string>> GetFriendsListAsync()
        {
            await SimulateNetworkDelay();
            return new Result<IFriendsList, string>(_friendsList);
        }

        /// <summary>
        /// Gets the local user's portrait thumbnail at 64x64 pixels
        /// </summary>
        /// <returns>A task containing the thumbnail portrait as Texture2D</returns>
        public async Task<Result<Texture2D, string>> GetUserPortraitThumbnailAsync()
        {
            await SimulateNetworkDelay();
            
            if (_settings.UserPortrait == null)
            {
                return new Result<Texture2D, string>("No user portrait configured in EditorPlatformSettings");
            }

            var thumbnail = CreateThumbnail(_settings.UserPortrait, 64);
            return new Result<Texture2D, string>(thumbnail);
        }

        /// <summary>
        /// Gets the local user's portrait thumbnail at 64x64 pixels for a specific user
        /// </summary>
        /// <param name="userID">The unique identifier of the user</param>
        /// <returns>A task containing the thumbnail portrait as Texture2D</returns>
        public async Task<Result<Texture2D, string>> GetUserPortraitThumbnailAsync(ulong userID)
        {
            await SimulateNetworkDelay();
            
            if (userID == _localUser.UserID)
            {
                return await GetUserPortraitThumbnailAsync();
            }

            // For other users, return a default placeholder or error
            return new Result<Texture2D, string>($"Portrait not available for user {userID} in editor mode");
        }

        /// <summary>
        /// Gets the local user's portrait at native resolution for the current platform
        /// </summary>
        /// <returns>A task containing the native resolution portrait as Texture2D</returns>
        public async Task<Result<Texture2D, string>> GetUserPortraitNativeAsync()
        {
            await SimulateNetworkDelay();
            
            if (_settings.UserPortrait == null)
            {
                return new Result<Texture2D, string>("No user portrait configured in EditorPlatformSettings");
            }

            return new Result<Texture2D, string>(_settings.UserPortrait);
        }

        /// <summary>
        /// Gets a user's portrait at native resolution for the current platform
        /// </summary>
        /// <param name="userID">The unique identifier of the user</param>
        /// <returns>A task containing the native resolution portrait as Texture2D</returns>
        public async Task<Result<Texture2D, string>> GetUserPortraitNativeAsync(ulong userID)
        {
            await SimulateNetworkDelay();
            
            if (userID == _localUser.UserID)
            {
                return await GetUserPortraitNativeAsync();
            }

            // For other users, return a default placeholder or error
            return new Result<Texture2D, string>($"Portrait not available for user {userID} in editor mode");
        }

        /// <summary>
        /// Refreshes the friendlist from the platform
        /// </summary>
        /// <returns>A task that completes when the friendlist is refreshed</returns>
        public async Task<Result<IFriendsList, string>> RefreshFriendsListAsync()
        {
            await SimulateNetworkDelay();
            
            // In editor mode, we just return the same friends list
            FriendsListUpdated?.Invoke(_friendsList);
            return new Result<IFriendsList, string>(_friendsList);
        }

        /// <summary>
        /// Signs out the current local user
        /// </summary>
        /// <returns>A task that completes when the user is signed out</returns>
        public async Task<Result<bool, string>> SignOutAsync()
        {
            await SimulateNetworkDelay();
            
            var signedOutUser = new LocalUser(_localUser.Name, _localUser.GamerTag, _localUser.UserID, UserAuthenticationStatus.NotAuthenticated);
            _localUser = signedOutUser;
            AuthenticationStatusChanged?.Invoke(UserAuthenticationStatus.NotAuthenticated);
            
            return new Result<bool, string>(true);
        }

        private void InitializeLocalUser()
        {
            _localUser = new LocalUser(
                _settings.UserName,
                _settings.GamerTag,
                _settings.UserID,
                UserAuthenticationStatus.NotAuthenticated
            );
        }

        private void InitializeFriendsList()
        {
            // Create some mock friends for editor testing
            IUser[] mockFriends = new IUser[]
            {
                new LocalUser("Friend One", "Friend1", 11111, UserAuthenticationStatus.Authenticated),
                new LocalUser("Friend Two", "Friend2", 22222, UserAuthenticationStatus.NotAuthenticated),
                new LocalUser("Friend Three", "Friend3", 33333, UserAuthenticationStatus.Authenticated)
            };

            _friendsList = new FriendsList(mockFriends);
        }

        private async Task SimulateNetworkDelay()
        {
            if (_settings.SimulateNetworkDelay)
            {
                var delay = _settings.GetRandomNetworkDelay();
                if (delay > 0)
                {
                    await Task.Delay(Mathf.RoundToInt(delay * 1000));
                }
            }
        }

        private Texture2D CreateThumbnail(Texture2D source, int size)
        {
            if (source == null) return null;

            var thumbnail = new Texture2D(size, size);
            
            // Simple scaling - in a real implementation you might want more sophisticated scaling
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float u = (float)x / size;
                    float v = (float)y / size;
                    var pixel = source.GetPixelBilinear(u, v);
                    thumbnail.SetPixel(x, y, pixel);
                }
            }
            
            thumbnail.Apply();
            return thumbnail;
        }
    }
}