using System;
using System.Threading.Tasks;
using UnityEngine;

namespace PlatformFacade
{
    /// <summary>
    /// Provides user services across gaming platforms including authentication, 
    /// friendlists, and user portraits
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Event fired when the local user's authentication status changes
        /// </summary>
        event Action<UserAuthenticationStatus> AuthenticationStatusChanged;
        
        /// <summary>
        /// Event fired when the local user's friendlist is updated
        /// </summary>
        event Action<IFriendsList> FriendsListUpdated;

        /// <summary>
        /// Gets the current local user
        /// </summary>
        /// <returns>The local user if available, null otherwise</returns>
        IUser GetLocalUser();
        
        /// <summary>
        /// Authenticates the local user asynchronously
        /// </summary>
        /// <returns>A task containing the authentication result</returns>
        Task<Result<IUser, string>> AuthenticateLocalUserAsync();
        
        /// <summary>
        /// Gets a readonly reference to the local user's friendlist
        /// </summary>
        /// <returns>A task containing the friendlist if available, error message otherwise</returns>
        Task<Result<IFriendsList, string>> GetFriendsListAsync();
        
        /// <summary>
        /// Gets the local user's portrait thumbnail at 64x64 pixels
        /// </summary>
        /// <returns>A task containing the thumbnail portrait as Texture2D</returns>
        Task<Result<Texture2D, string>> GetUserPortraitThumbnailAsync();
        
        /// <summary>
        /// Gets the local user's portrait thumbnail at 64x64 pixels for a specific user
        /// </summary>
        /// <param name="userID">The unique identifier of the user</param>
        /// <returns>A task containing the thumbnail portrait as Texture2D</returns>
        Task<Result<Texture2D, string>> GetUserPortraitThumbnailAsync(ulong userID);
        
        /// <summary>
        /// Gets the local user's portrait at native resolution for the current platform
        /// </summary>
        /// <returns>A task containing the native resolution portrait as Texture2D</returns>
        Task<Result<Texture2D, string>> GetUserPortraitNativeAsync();
        
        /// <summary>
        /// Gets a user's portrait at native resolution for the current platform
        /// </summary>
        /// <param name="userID">The unique identifier of the user</param>
        /// <returns>A task containing the native resolution portrait as Texture2D</returns>
        Task<Result<Texture2D, string>> GetUserPortraitNativeAsync(ulong userID);
        
        /// <summary>
        /// Refreshes the friendlist from the platform
        /// </summary>
        /// <returns>A task that completes when the friendlist is refreshed</returns>
        Task<Result<IFriendsList, string>> RefreshFriendsListAsync();
        
        /// <summary>
        /// Signs out the current local user
        /// </summary>
        /// <returns>A task that completes when the user is signed out</returns>
        Task<Result<bool, string>> SignOutAsync();
    }
}