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
                Console.WriteLine($"User {localUser.Name} is already authenticated");
                return;
            }

            // Authenticate the user using Railway Oriented Design
            var authResult = await _userService.AuthenticateLocalUserAsync();
            if (authResult.IsSuccess)
            {
                Console.WriteLine($"Successfully authenticated user: {authResult.Value.Name}");
                
                // Chain operations using Railway Oriented Design
                await _userService.AuthenticateLocalUserAsync()
                    .Then(async user => await LoadFriendsListAsync())
                    .Then(async friendsList => await LoadUserPortraitAsync());
            }
            else
            {
                Console.WriteLine($"Authentication failed: {authResult.Error}");
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
                Console.WriteLine($"Loaded {friendsList.Count} friends");
                
                // Get online friends (authenticated users)
                var onlineFriends = friendsList.GetFriendsByStatus(true);
                Console.WriteLine($"Online friends: {string.Join(", ", onlineFriends.Select(f => f.Name))}");
                
                return new Result<IFriendsList, string>(friendsList);
            }
            else
            {
                Console.WriteLine($"Failed to load friends list: {friendsListResult.Error}");
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
                Console.WriteLine($"Loaded thumbnail: {thumbnail.width}x{thumbnail.height}");
            }

            // Get native resolution portrait
            var nativePortraitResult = await _userService.GetUserPortraitNativeAsync();
            if (nativePortraitResult.IsSuccess)
            {
                var nativePortrait = nativePortraitResult.Value;
                Console.WriteLine($"Loaded native portrait: {nativePortrait.width}x{nativePortrait.height}");
                return new Result<Texture2D, string>(nativePortrait);
            }
            else
            {
                Console.WriteLine($"Failed to load portrait: {nativePortraitResult.Error}");
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
                Console.WriteLine($"Loaded friend's thumbnail: {friendThumbnail.width}x{friendThumbnail.height}");
            }
            else
            {
                Console.WriteLine($"Failed to load friend's portrait: {friendThumbnailResult.Error}");
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
                    Console.WriteLine($"Authenticated as: {user.Name}");
                    return await _userService.GetFriendsListAsync();
                })
                .Then(async friendsList => 
                {
                    Console.WriteLine($"Loaded {friendsList.Count} friends");
                    return await _userService.GetUserPortraitThumbnailAsync();
                });
        }

        /// <summary>
        /// Event handler for authentication status changes
        /// </summary>
        private void OnAuthenticationStatusChanged(UserAuthenticationStatus status)
        {
            Console.WriteLine($"Authentication status changed: {status}");
            
            switch (status)
            {
                case UserAuthenticationStatus.Authenticated:
                    // User successfully authenticated - load additional data
                    _ = LoadFriendsListAsync();
                    _ = LoadUserPortraitAsync();
                    break;
                    
                case UserAuthenticationStatus.NotAuthenticated:
                    // User signed out - clear cached data
                    Console.WriteLine("User signed out, clearing cached data");
                    break;
                    
                case UserAuthenticationStatus.Authenticating:
                    // Show loading indicator
                    Console.WriteLine("Authentication in progress...");
                    break;
            }
        }

        /// <summary>
        /// Event handler for friendlist updates
        /// </summary>
        private void OnFriendsListUpdated(IFriendsList friendsList)
        {
            Console.WriteLine($"Friendlist updated: {friendsList.Count} friends");
            
            // Update UI with new friendlist
            var onlineFriends = friendsList.GetFriendsByStatus(true);
            Console.WriteLine($"{onlineFriends.Count()} friends are online");
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