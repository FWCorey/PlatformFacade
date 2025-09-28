using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PlatformFacade.Editor
{
    /// <summary>
    /// Editor implementation of ILeaderboard providing mock leaderboard functionality
    /// </summary>
    public class EditorLeaderboard : ILeaderboard
    {
        private readonly List<EditorLeaderboardEntry> _entries;
        private readonly ulong _leaderboardID;
        private readonly string _displayName;

        /// <summary>
        /// The unique identifier for this leaderboard
        /// </summary>
        public ulong LeaderboardID => _leaderboardID;

        /// <summary>
        /// The display name of this leaderboard
        /// </summary>
        public string DisplayName => _displayName;

        /// <summary>
        /// Gets the number of entries in the leaderboard
        /// </summary>
        public int Count => _entries.Count;

        /// <summary>
        /// Initializes a new instance of the EditorLeaderboard class
        /// </summary>
        /// <param name="leaderboardID">The unique identifier for this leaderboard</param>
        /// <param name="displayName">The display name of this leaderboard</param>
        public EditorLeaderboard(ulong leaderboardID, string displayName)
        {
            _leaderboardID = leaderboardID;
            _displayName = displayName ?? $"Leaderboard {leaderboardID}";
            _entries = new List<EditorLeaderboardEntry>();
        }

        /// <summary>
        /// Gets a leaderboard entry by rank position
        /// </summary>
        /// <param name="rank">The rank position (1-based)</param>
        /// <returns>The leaderboard entry at the specified rank, null if not found</returns>
        public ILeaderboardEntry GetEntryByRank(int rank)
        {
            if (rank < 1 || rank > _entries.Count)
                return null;

            return _entries[rank - 1];
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
        /// Adds an entry to the leaderboard
        /// </summary>
        /// <param name="entry">The entry to add</param>
        public void AddEntry(EditorLeaderboardEntry entry)
        {
            _entries.Add(entry);
            SortAndUpdateRanks();
        }

        /// <summary>
        /// Updates a user's score on the leaderboard
        /// </summary>
        /// <param name="userID">The user ID to update</param>
        /// <param name="newEntry">The new entry for the user</param>
        public void UpdateUserScore(ulong userID, EditorLeaderboardEntry newEntry)
        {
            // Remove existing entry if present
            for (int i = _entries.Count - 1; i >= 0; i--)
            {
                if (_entries[i].User.UserID == userID)
                {
                    _entries.RemoveAt(i);
                    break;
                }
            }

            // Add new entry
            AddEntry(newEntry);
        }

        /// <summary>
        /// Gets the top entries from the leaderboard
        /// </summary>
        /// <param name="maxEntries">Maximum number of entries to return</param>
        /// <returns>A new leaderboard with the top entries</returns>
        public EditorLeaderboard GetTopEntries(int maxEntries)
        {
            var topLeaderboard = new EditorLeaderboard(_leaderboardID, _displayName);
            var topEntries = _entries.Take(maxEntries);

            foreach (var entry in topEntries)
            {
                topLeaderboard._entries.Add(entry);
            }

            return topLeaderboard;
        }

        /// <summary>
        /// Gets entries around a specific rank
        /// </summary>
        /// <param name="centerRank">The rank to center around</param>
        /// <param name="maxEntries">Maximum number of entries to return</param>
        /// <returns>A new leaderboard with entries around the specified rank</returns>
        public EditorLeaderboard GetEntriesAroundRank(int centerRank, int maxEntries)
        {
            var aroundLeaderboard = new EditorLeaderboard(_leaderboardID, _displayName);
            
            int halfRange = maxEntries / 2;
            int startIndex = UnityEngine.Mathf.Max(0, centerRank - halfRange - 1);
            int endIndex = UnityEngine.Mathf.Min(_entries.Count - 1, startIndex + maxEntries - 1);
            
            // Adjust start if we hit the end boundary
            if (endIndex - startIndex + 1 < maxEntries)
            {
                startIndex = UnityEngine.Mathf.Max(0, endIndex - maxEntries + 1);
            }

            for (int i = startIndex; i <= endIndex; i++)
            {
                aroundLeaderboard._entries.Add(_entries[i]);
            }

            return aroundLeaderboard;
        }

        /// <summary>
        /// Gets entries for specific friends
        /// </summary>
        /// <param name="friendUserIDs">Array of friend user IDs</param>
        /// <param name="maxEntries">Maximum number of entries to return</param>
        /// <returns>A new leaderboard with friend entries</returns>
        public EditorLeaderboard GetFriendsEntries(ulong[] friendUserIDs, int maxEntries)
        {
            var friendsLeaderboard = new EditorLeaderboard(_leaderboardID, $"{_displayName} (Friends)");
            
            var friendEntries = _entries
                .Where(entry => friendUserIDs.Contains(entry.User.UserID))
                .Take(maxEntries);

            foreach (var entry in friendEntries)
            {
                friendsLeaderboard._entries.Add(entry);
            }

            return friendsLeaderboard;
        }

        /// <summary>
        /// Gets entries within a specific rank range
        /// </summary>
        /// <param name="startRank">The starting rank (1-based, inclusive)</param>
        /// <param name="endRank">The ending rank (1-based, inclusive)</param>
        /// <returns>A new leaderboard with entries in the specified range</returns>
        public EditorLeaderboard GetEntriesByRange(int startRank, int endRank)
        {
            var rangeLeaderboard = new EditorLeaderboard(_leaderboardID, _displayName);
            
            int startIndex = startRank - 1;
            int endIndex = UnityEngine.Mathf.Min(endRank - 1, _entries.Count - 1);

            if (startIndex >= 0 && startIndex <= endIndex)
            {
                for (int i = startIndex; i <= endIndex; i++)
                {
                    rangeLeaderboard._entries.Add(_entries[i]);
                }
            }

            return rangeLeaderboard;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the leaderboard entries
        /// </summary>
        /// <returns>An enumerator for the leaderboard entries</returns>
        public IEnumerator<ILeaderboardEntry> GetEnumerator()
        {
            return _entries.Cast<ILeaderboardEntry>().GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the leaderboard entries
        /// </summary>
        /// <returns>An enumerator for the leaderboard entries</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void SortAndUpdateRanks()
        {
            // Sort by score descending
            _entries.Sort((a, b) => b.Score.CompareTo(a.Score));

            // Update ranks
            for (int i = 0; i < _entries.Count; i++)
            {
                _entries[i].UpdateRank(i + 1);
            }
        }
    }
}