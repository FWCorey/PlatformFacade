using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PlatformFacade
{
    /// <summary>
    /// A concrete implementation of IFriendsList providing readonly access to friends
    /// </summary>
    public class FriendsList : IFriendsList
    {
        private readonly List<IFriend> _friends;

        /// <summary>
        /// Gets the number of friends in the list
        /// </summary>
        public int Count => _friends.Count;

        /// <summary>
        /// Initializes a new instance of the FriendsList class
        /// </summary>
        /// <param name="friends">The collection of friends</param>
        public FriendsList(IEnumerable<IFriend> friends = null)
        {
            _friends = new List<IFriend>(friends ?? Enumerable.Empty<IFriend>());
        }

        /// <summary>
        /// Gets a friend by their user ID
        /// </summary>
        /// <param name="userID">The unique identifier of the friend</param>
        /// <returns>The friend if found, null otherwise</returns>
        public IFriend GetFriend(ulong userID)
        {
            return _friends.FirstOrDefault(f => f.UserID == userID);
        }

        /// <summary>
        /// Gets friends by their online status
        /// </summary>
        /// <param name="isOnline">True to get online friends, false for offline friends</param>
        /// <returns>Collection of friends matching the online status</returns>
        public IEnumerable<IFriend> GetFriendsByStatus(bool isOnline)
        {
            return _friends.Where(f => f.IsOnline == isOnline);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the friends collection
        /// </summary>
        /// <returns>An enumerator for the friends collection</returns>
        public IEnumerator<IFriend> GetEnumerator()
        {
            return _friends.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the friends collection
        /// </summary>
        /// <returns>An enumerator for the friends collection</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}