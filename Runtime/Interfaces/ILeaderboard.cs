using System.Collections.Generic;

namespace PlatformFacade
{
    /// <summary>
    /// Represents a leaderboard containing ranked entries
    /// </summary>
    public interface ILeaderboard : IReadOnlyCollection<ILeaderboardEntry>
    {
        /// <summary>
        /// The unique identifier for this leaderboard
        /// </summary>
        string LeaderboardID { get; }
        
        /// <summary>
        /// The display name of this leaderboard
        /// </summary>
        string DisplayName { get; }
        
        /// <summary>
        /// The sort order used for this leaderboard
        /// </summary>
        LeaderboardSortOrder SortOrder { get; }
        
        /// <summary>
        /// Gets a leaderboard entry by rank position
        /// </summary>
        /// <param name="rank">The rank position (1-based)</param>
        /// <returns>The leaderboard entry at the specified rank, null if not found</returns>
        ILeaderboardEntry GetEntryByRank(int rank);
        
        /// <summary>
        /// Gets a leaderboard entry for a specific user
        /// </summary>
        /// <param name="userID">The unique identifier of the user</param>
        /// <returns>The leaderboard entry for the user, null if not found</returns>
        ILeaderboardEntry GetEntryByUser(ulong userID);
    }
}