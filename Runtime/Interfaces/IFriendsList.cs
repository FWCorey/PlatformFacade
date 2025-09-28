using System.Collections.Generic;

namespace PlatformFacade
{
    /// <summary>
    /// Represents a readonly collection of friends for a user
    /// </summary>
    public interface IFriendsList : IReadOnlyCollection<IFriend>
    {
        /// <summary>
        /// Gets a friend by their user ID
        /// </summary>
        /// <param name="userID">The unique identifier of the friend</param>
        /// <returns>The friend if found, null otherwise</returns>
        IFriend GetFriend(ulong userID);
        
        /// <summary>
        /// Gets friends by their online status
        /// </summary>
        /// <param name="isOnline">True to get online friends, false for offline friends</param>
        /// <returns>Collection of friends matching the online status</returns>
        IEnumerable<IFriend> GetFriendsByStatus(bool isOnline);
    }
}