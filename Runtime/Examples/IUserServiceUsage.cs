using System;
using System.Linq;
using System.Threading.Tasks;

namespace PlatformFacade.Examples
{
    /// <summary>
    /// Example usage patterns for IUserService demonstrating common scenarios
    /// across gaming platforms (iOS, Android, Epic Store, Steamworks, Nintendo Switch, 
    /// Xbox GameServices, PlayStation, Windows Store)
    /// </summary>
    public class IUserServiceUsage
    {
        private readonly IUserService _userService;

        public IUserServiceUsage(IUserService userService)
        {
            _userService = userService;
            
            // Subscribe to authentication status changes
            _userService.AuthenticationStatusChanged += OnAuthenticationStatusChanged;
            
            // Subscribe to friendlist updates
            _userService.FriendsListUpdated += OnFriendsListUpdated;
        }

        /// <summary>
        /// Example: Complete user authentication flow
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

            // Authenticate the user
            var authResult = await _userService.AuthenticateLocalUserAsync();
            if (authResult.IsSuccess)
            {
                Console.WriteLine($"Successfully authenticated user: {authResult.User.Name}");
                
                // Load user's friendlist after authentication
                await LoadFriendsListAsync();
                
                // Load user portrait
                await LoadUserPortraitAsync();
            }
            else
            {
                Console.WriteLine($"Authentication failed: {authResult.ErrorMessage}");
            }
        }

        /// <summary>
        /// Example: Load and display user's friendlist
        /// </summary>
        public async Task LoadFriendsListAsync()
        {
            var friendsList = await _userService.GetFriendsListAsync();
            if (friendsList != null)
            {
                Console.WriteLine($"Loaded {friendsList.Count} friends");
                
                // Get online friends
                var onlineFriends = friendsList.GetFriendsByStatus(true);
                Console.WriteLine($"Online friends: {string.Join(", ", onlineFriends)}");
            }
        }

        /// <summary>
        /// Example: Load user portraits (thumbnail and native resolution)
        /// </summary>
        public async Task LoadUserPortraitAsync()
        {
            // Get thumbnail portrait (64x64)
            var thumbnail = await _userService.GetUserPortraitThumbnailAsync();
            if (thumbnail != null && thumbnail.IsLoaded)
            {
                Console.WriteLine($"Loaded thumbnail: {thumbnail.Width}x{thumbnail.Height} ({thumbnail.Format})");
            }

            // Get native resolution portrait
            var nativePortrait = await _userService.GetUserPortraitNativeAsync();
            if (nativePortrait != null && nativePortrait.IsLoaded)
            {
                Console.WriteLine($"Loaded native portrait: {nativePortrait.Width}x{nativePortrait.Height} ({nativePortrait.Format})");
            }
        }

        /// <summary>
        /// Example: Load a specific friend's portrait
        /// </summary>
        public async Task LoadFriendPortraitAsync(ulong friendUserID)
        {
            var friendThumbnail = await _userService.GetUserPortraitThumbnailAsync(friendUserID);
            if (friendThumbnail != null && friendThumbnail.IsLoaded)
            {
                Console.WriteLine($"Loaded friend's thumbnail: {friendThumbnail.Width}x{friendThumbnail.Height}");
            }
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