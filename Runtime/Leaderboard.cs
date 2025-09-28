using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PlatformFacade
{
    /// <summary>
    /// A concrete implementation of ILeaderboard providing readonly access to leaderboard entries
    /// </summary>
    public class Leaderboard : ILeaderboard
    {
        private readonly List<ILeaderboardEntry> _entries;

        /// <summary>
        /// The unique identifier for this leaderboard
        /// </summary>
        public string LeaderboardID { get; }
        
        /// <summary>
        /// The display name of this leaderboard
        /// </summary>
        public string DisplayName { get; }
        
        /// <summary>
        /// The sort order used for this leaderboard
        /// </summary>
        public LeaderboardSortOrder SortOrder { get; }
        
        /// <summary>
        /// Gets the number of entries in the leaderboard
        /// </summary>
        public int Count => _entries.Count;

        /// <summary>
        /// Initializes a new instance of the Leaderboard class
        /// </summary>
        /// <param name="leaderboardID">The unique identifier for the leaderboard</param>
        /// <param name="displayName">The display name of the leaderboard</param>
        /// <param name="sortOrder">The sort order for the leaderboard</param>
        /// <param name="entries">The collection of leaderboard entries</param>
        public Leaderboard(string leaderboardID, string displayName, LeaderboardSortOrder sortOrder, IEnumerable<ILeaderboardEntry> entries = null)
        {
            LeaderboardID = leaderboardID ?? throw new ArgumentNullException(nameof(leaderboardID));
            DisplayName = displayName ?? throw new ArgumentNullException(nameof(displayName));
            SortOrder = sortOrder;
            _entries = new List<ILeaderboardEntry>();
            
            if (entries != null)
            {
                foreach (var entry in entries)
                {
                    _entries.Add(entry);
                }
            }
        }

        /// <summary>
        /// Gets a leaderboard entry by rank position
        /// </summary>
        /// <param name="rank">The rank position (1-based)</param>
        /// <returns>The leaderboard entry at the specified rank, null if not found</returns>
        public ILeaderboardEntry GetEntryByRank(int rank)
        {
            return _entries.FirstOrDefault(entry => entry.Rank == rank);
        }

        /// <summary>
        /// Gets a leaderboard entry for a specific user
        /// </summary>
        /// <param name="userID">The unique identifier of the user</param>
        /// <returns>The leaderboard entry for the user, null if not found</returns>
        public ILeaderboardEntry GetEntryByUser(ulong userID)
        {
            return _entries.FirstOrDefault(entry => entry.User.UserID == userID);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the leaderboard entries
        /// </summary>
        /// <returns>An enumerator for the leaderboard entries</returns>
        public IEnumerator<ILeaderboardEntry> GetEnumerator()
        {
            return _entries.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the leaderboard entries
        /// </summary>
        /// <returns>An enumerator for the leaderboard entries</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}