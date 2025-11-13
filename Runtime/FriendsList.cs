using System.Collections;
using System.Collections.Generic;

namespace PlatformFacade
{
    /// <summary>
    /// A concrete dummy implementation of IFriendsList providing readonly access to friends
    /// </summary>
    public class FriendsList : IFriendsList
    {
        private readonly List<IUser> _friends;

        /// <summary>
        /// Gets the number of friends in the list
        /// </summary>
        public int Count => _friends.Count;

        /// <summary>
        /// Initializes a new instance of the FriendsList class
        /// </summary>
        /// <param name="friends">The collection of friends</param>
        public FriendsList(IEnumerable<IUser> friends = null)
        {
            _friends = new List<IUser>();
            if (friends != null)
            {
                foreach (var friend in friends)
                {
                    _friends.Add(friend);
                }
            }
        }

        /// <summary>
        /// Gets a friend by their user ID
        /// </summary>
        /// <param name="userID">The unique identifier of the friend</param>
        /// <returns>The friend if found, null otherwise</returns>
        public IUser GetFriend(ulong userID)
        {
            for (int i = 0; i < _friends.Count; i++)
            {
                if (_friends[i].UserID == userID)
                {
                    return _friends[i];
                }
            }
            return null;
        }

        /// <summary>
        /// Gets friends by their online status (based on authentication status)
        /// </summary>
        /// <param name="isOnline">True to get authenticated friends, false for unauthenticated friends</param>
        /// <param name="results">List to populate with matching friends (cleared before use)</param>
        public void GetFriendsByStatus(bool isOnline, List<IUser> results)
        {
            results.Clear();
            
        }

        /// <summary>
        /// Returns an enumerator that iterates through the friends collection
        /// </summary>
        /// <returns>An enumerator for the friends collection</returns>
        public IEnumerator<IUser> GetEnumerator()
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