using System;
using System.Threading.Tasks;

namespace PlatformFacade {

    /// <summary>
    /// Provides leaderboard services across gaming platforms including score submission,
    /// leaderboard retrieval, and user ranking queries
    /// </summary>
    public interface ILeaderboards {

        /// <summary>
        /// Event fired when a leaderboard is updated with new scores
        /// </summary>
        event Action<ILeaderboard> LeaderboardUpdated;

        /// <summary>
        /// Event fired when the local user's score is updated on any leaderboard
        /// </summary>
        event Action<ulong, ILeaderboardEntry> ScoreUpdated;

        /// <summary>
        /// Submits a score to the specified leaderboard
        /// </summary>
        /// <param name="leaderboardID">The unique identifier of the leaderboard</param>
        /// <param name="score">The score to submit</param>
        /// <returns>A task containing the result of the score submission</returns>
        Task<Result<bool, string>> SubmitScoreAsync(ulong leaderboardID, long score);

        /// <summary>
        /// Gets the global leaderboard entries
        /// </summary>
        /// <param name="leaderboardID">The unique identifier of the leaderboard</param>
        /// <param name="maxEntries">Maximum number of entries to retrieve (default: 10)</param>
        /// <returns>A task containing the leaderboard if available, error message otherwise</returns>
        Task<Result<ILeaderboard, string>> GetGlobalLeaderboardAsync(ulong leaderboardID, int maxEntries = 10);

        /// <summary>
        /// Gets leaderboard entries around the current user's rank
        /// </summary>
        /// <param name="leaderboardID">The unique identifier of the leaderboard</param>
        /// <param name="maxEntries">Maximum number of entries to retrieve (default: 10)</param>
        /// <returns>A task containing the leaderboard if available, error message otherwise</returns>
        Task<Result<ILeaderboard, string>> GetLeaderboardAroundUserAsync(ulong leaderboardID, int maxEntries = 10);

        /// <summary>
        /// Gets leaderboard entries for friends only
        /// </summary>
        /// <param name="leaderboardID">The unique identifier of the leaderboard</param>
        /// <param name="maxEntries">Maximum number of entries to retrieve (default: 10)</param>
        /// <returns>A task containing the friends leaderboard if available, error message otherwise</returns>
        Task<Result<ILeaderboard, string>> GetFriendsLeaderboardAsync(ulong leaderboardID, int maxEntries = 10);

        /// <summary>
        /// Gets the current user's rank and score for a specific leaderboard
        /// </summary>
        /// <param name="leaderboardID">The unique identifier of the leaderboard</param>
        /// <returns>A task containing the user's leaderboard entry if available, error message otherwise</returns>
        Task<Result<ILeaderboardEntry, string>> GetUserEntryAsync(ulong leaderboardID);

        /// <summary>
        /// Gets a specific user's rank and score for a leaderboard
        /// </summary>
        /// <param name="leaderboardID">The unique identifier of the leaderboard</param>
        /// <param name="userID">The unique identifier of the user</param>
        /// <returns>A task containing the user's leaderboard entry if available, error message otherwise</returns>
        Task<Result<ILeaderboardEntry, string>> GetUserEntryAsync(ulong leaderboardID, ulong userID);

        /// <summary>
        /// Gets leaderboard entries within a specific rank range
        /// </summary>
        /// <param name="leaderboardID">The unique identifier of the leaderboard</param>
        /// <param name="startRank">The starting rank (1-based, inclusive)</param>
        /// <param name="endRank">The ending rank (1-based, inclusive)</param>
        /// <returns>A task containing the leaderboard entries if available, error message otherwise</returns>
        Task<Result<ILeaderboard, string>> GetLeaderboardByRangeAsync(ulong leaderboardID, int startRank, int endRank);

        /// <summary>
        /// Retrieves the unique identifier for the specified leaderboard.
        /// </summary>
        /// <param name="leaderboardName">The name of the leaderboard whose identifier is to be retrieved. Cannot be null or empty.</param>
        /// <returns>A task containing the leaderboard id if available, error message otherwise</returns>
        Task<Result<ulong, string>> GetLeaderboardID(string leaderboardName);
    }
}