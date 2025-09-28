using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace PlatformFacade.Examples
{
    /// <summary>
    /// Example usage patterns for IUserService demonstrating common scenarios
    /// across gaming platforms (iOS, Android, Epic Store, Steamworks, Nintendo Switch, 
    /// Xbox GameServices, PlayStation, Windows Store) using Railway Oriented Design
    /// </summary>
    public class UserServiceUsage
    {
        private readonly IUserService _userService;

        public UserServiceUsage(IUserService userService)
        {
            _userService = userService;
            
            // Subscribe to authentication status changes
            _userService.AuthenticationStatusChanged += OnAuthenticationStatusChanged;
            
            // Subscribe to friendlist updates
            _userService.FriendsListUpdated += OnFriendsListUpdated;
        }

        /// <summary>
        /// Example: Complete user authentication flow using Railway Oriented Design
        /// </summary>
        public async Task AuthenticateUserAsync()
        {
            // Check if user is already authenticated
            var localUser = _userService.GetLocalUser();
            if (localUser?.AuthenticationStatus == UserAuthenticationStatus.Authenticated)
            {
                Debug.Log($"User {localUser.Name} is already authenticated");
                return;
            }

            // Authenticate the user using Railway Oriented Design
            var authResult = await _userService.AuthenticateLocalUserAsync();
            if (authResult.IsSuccess)
            {
                Debug.Log($"Successfully authenticated user: {authResult.Value.Name}");
                
                // Chain operations using Railway Oriented Design
                await _userService.AuthenticateLocalUserAsync()
                    .Then(async user => await LoadFriendsListAsync())
                    .Then(async friendsList => await LoadUserPortraitAsync());
            }
            else
            {
                Debug.Log($"Authentication failed: {authResult.Error}");
            }
        }

        /// <summary>
        /// Example: Load and display user's friendlist using Railway Oriented Design
        /// </summary>
        public async Task<Result<IFriendsList, string>> LoadFriendsListAsync()
        {
            var friendsListResult = await _userService.GetFriendsListAsync();
            if (friendsListResult.IsSuccess)
            {
                var friendsList = friendsListResult.Value;
                Debug.Log($"Loaded {friendsList.Count} friends");
                
                // Get online friends (authenticated users)
                var onlineFriends = friendsList.GetFriendsByStatus(true);
                Debug.Log($"Online friends: {string.Join(", ", onlineFriends.Select(f => f.Name))}");
                
                return new Result<IFriendsList, string>(friendsList);
            }
            else
            {
                Debug.Log($"Failed to load friends list: {friendsListResult.Error}");
                return new Result<IFriendsList, string>(friendsListResult.Error);
            }
        }

        /// <summary>
        /// Example: Load user portraits using Railway Oriented Design
        /// </summary>
        public async Task<Result<Texture2D, string>> LoadUserPortraitAsync()
        {
            // Get thumbnail portrait (64x64)
            var thumbnailResult = await _userService.GetUserPortraitThumbnailAsync();
            if (thumbnailResult.IsSuccess)
            {
                var thumbnail = thumbnailResult.Value;
                Debug.Log($"Loaded thumbnail: {thumbnail.width}x{thumbnail.height}");
            }

            // Get native resolution portrait
            var nativePortraitResult = await _userService.GetUserPortraitNativeAsync();
            if (nativePortraitResult.IsSuccess)
            {
                var nativePortrait = nativePortraitResult.Value;
                Debug.Log($"Loaded native portrait: {nativePortrait.width}x{nativePortrait.height}");
                return new Result<Texture2D, string>(nativePortrait);
            }
            else
            {
                Debug.Log($"Failed to load portrait: {nativePortraitResult.Error}");
                return new Result<Texture2D, string>(nativePortraitResult.Error);
            }
        }

        /// <summary>
        /// Example: Load a specific friend's portrait using Railway Oriented Design
        /// </summary>
        public async Task LoadFriendPortraitAsync(ulong friendUserID)
        {
            var friendThumbnailResult = await _userService.GetUserPortraitThumbnailAsync(friendUserID);
            if (friendThumbnailResult.IsSuccess)
            {
                var friendThumbnail = friendThumbnailResult.Value;
                Debug.Log($"Loaded friend's thumbnail: {friendThumbnail.width}x{friendThumbnail.height}");
            }
            else
            {
                Debug.Log($"Failed to load friend's portrait: {friendThumbnailResult.Error}");
            }
        }

        /// <summary>
        /// Example: Chain multiple operations using Railway Oriented Design
        /// </summary>
        public async Task CompleteUserSetupAsync()
        {
            await _userService.AuthenticateLocalUserAsync()
                .Then(async user => 
                {
                    Debug.Log($"Authenticated as: {user.Name}");
                    return await _userService.GetFriendsListAsync();
                })
                .Then(async friendsList => 
                {
                    Debug.Log($"Loaded {friendsList.Count} friends");
                    return await _userService.GetUserPortraitThumbnailAsync();
                });
        }

        /// <summary>
        /// Event handler for authentication status changes
        /// </summary>
        private void OnAuthenticationStatusChanged(UserAuthenticationStatus status)
        {
            Debug.Log($"Authentication status changed: {status}");
            
            switch (status)
            {
                case UserAuthenticationStatus.Authenticated:
                    // User successfully authenticated - load additional data
                    _ = Task.Run(async () =>
                    {
                        await LoadFriendsListAsync();
                        await LoadUserPortraitAsync();
                    });
                    break;
                    
                case UserAuthenticationStatus.NotAuthenticated:
                    // User signed out - clear cached data
                    Debug.Log("User signed out, clearing cached data");
                    break;
                    
                case UserAuthenticationStatus.Authenticating:
                    // Show loading indicator
                    Debug.Log("Authentication in progress...");
                    break;
            }
        }

        /// <summary>
        /// Event handler for friendlist updates
        /// </summary>
        private void OnFriendsListUpdated(IFriendsList friendsList)
        {
            Debug.Log($"Friendlist updated: {friendsList.Count} friends");
            
            // Update UI with new friendlist
            var onlineFriends = friendsList.GetFriendsByStatus(true);
            Debug.Log($"{onlineFriends.Count} friends are online");
        }

        /// <summary>
        /// Clean up event subscriptions
        /// </summary>
        public void Dispose()
        {
            _userService.AuthenticationStatusChanged -= OnAuthenticationStatusChanged;
            _userService.FriendsListUpdated -= OnFriendsListUpdated;
        }
    }
}