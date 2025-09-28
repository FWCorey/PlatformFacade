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
            _friends = new List<IUser>(friends ?? Enumerable.Empty<IUser>());
        }

        /// <summary>
        /// Gets a friend by their user ID
        /// </summary>
        /// <param name="userID">The unique identifier of the friend</param>
        /// <returns>The friend if found, null otherwise</returns>
        public IUser GetFriend(ulong userID)
        {
            return _friends.FirstOrDefault(f => f.UserID == userID);
        }

        /// <summary>
        /// Gets friends by their online status (based on authentication status)
        /// </summary>
        /// <param name="isOnline">True to get authenticated friends, false for unauthenticated friends</param>
        /// <returns>Collection of friends matching the online status</returns>
        public IEnumerable<IUser> GetFriendsByStatus(bool isOnline)
        {
            var targetStatus = isOnline ? UserAuthenticationStatus.Authenticated : UserAuthenticationStatus.NotAuthenticated;
            var result = new List<IUser>();
            
            for (int i = 0; i < _friends.Count; i++)
            {
                if (_friends[i].AuthenticationStatus == targetStatus)
                {
                    result.Add(_friends[i]);
                }
            }
            
            return result;
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