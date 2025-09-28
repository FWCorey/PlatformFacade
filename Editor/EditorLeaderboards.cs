using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace PlatformFacade.Editor
{
    /// <summary>
    /// Editor implementation of ILeaderboards providing mock leaderboard functionality for Unity Editor
    /// </summary>
    public class EditorLeaderboards : ILeaderboards
    {
        private readonly EditorPlatformSettings _settings;
        private volatile Dictionary<ulong, EditorLeaderboard> _leaderboards;

        /// <summary>
        /// Event fired when a leaderboard is updated with new scores
        /// </summary>
        public event Action<ILeaderboard> LeaderboardUpdated;
        
        /// <summary>
        /// Event fired when the local user's score is updated on any leaderboard
        /// </summary>
        public event Action<ulong, ILeaderboardEntry> ScoreUpdated;

        /// <summary>
        /// Initializes a new instance of the EditorLeaderboards class
        /// </summary>
        /// <param name="settings">The editor platform settings to use</param>
        public EditorLeaderboards(EditorPlatformSettings settings)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            InitializeMockLeaderboards();
        }

        /// <summary>
        /// Submits a score to the specified leaderboard
        /// </summary>
        /// <param name="leaderboardID">The unique identifier of the leaderboard</param>
        /// <param name="score">The score to submit</param>
        /// <returns>A task containing the result of the score submission</returns>
        public async Task<Result<bool, string>> SubmitScoreAsync(ulong leaderboardID, long score)
        {
            await SimulateNetworkDelay();

            if (!_leaderboards.TryGetValue(leaderboardID, out var leaderboard))
            {
                return new Result<bool, string>($"Leaderboard {leaderboardID} not found");
            }

            // Update or add user's score
            var userID = _settings.UserID;
            var entry = new EditorLeaderboardEntry(
                new LocalUser(_settings.UserName, _settings.GamerTag, userID, UserAuthenticationStatus.Authenticated),
                1, // Rank will be recalculated
                score
            );

            leaderboard.UpdateUserScore(userID, entry);
            
            ScoreUpdated?.Invoke(leaderboardID, entry);
            LeaderboardUpdated?.Invoke(leaderboard);

            return new Result<bool, string>(true);
        }

        /// <summary>
        /// Gets the global leaderboard entries
        /// </summary>
        /// <param name="leaderboardID">The unique identifier of the leaderboard</param>
        /// <param name="maxEntries">Maximum number of entries to retrieve (default: 10)</param>
        /// <returns>A task containing the leaderboard if available, error message otherwise</returns>
        public async Task<Result<ILeaderboard, string>> GetGlobalLeaderboardAsync(ulong leaderboardID, int maxEntries = 10)
        {
            await SimulateNetworkDelay();

            if (!_leaderboards.TryGetValue(leaderboardID, out var leaderboard))
            {
                return new Result<ILeaderboard, string>($"Leaderboard {leaderboardID} not found");
            }

            var limitedLeaderboard = leaderboard.GetTopEntries(maxEntries);
            return new Result<ILeaderboard, string>(limitedLeaderboard);
        }

        /// <summary>
        /// Gets leaderboard entries around the current user's rank
        /// </summary>
        /// <param name="leaderboardID">The unique identifier of the leaderboard</param>
        /// <param name="maxEntries">Maximum number of entries to retrieve (default: 10)</param>
        /// <returns>A task containing the leaderboard if available, error message otherwise</returns>
        public async Task<Result<ILeaderboard, string>> GetLeaderboardAroundUserAsync(ulong leaderboardID, int maxEntries = 10)
        {
            await SimulateNetworkDelay();

            if (!_leaderboards.TryGetValue(leaderboardID, out var leaderboard))
            {
                return new Result<ILeaderboard, string>($"Leaderboard {leaderboardID} not found");
            }

            var userEntry = leaderboard.GetEntryByUser(_settings.UserID);
            if (userEntry == null)
            {
                // If user not on leaderboard, return top entries
                return await GetGlobalLeaderboardAsync(leaderboardID, maxEntries);
            }

            var aroundUserLeaderboard = leaderboard.GetEntriesAroundRank(userEntry.Rank, maxEntries);
            return new Result<ILeaderboard, string>(aroundUserLeaderboard);
        }

        /// <summary>
        /// Gets leaderboard entries for friends only
        /// </summary>
        /// <param name="leaderboardID">The unique identifier of the leaderboard</param>
        /// <param name="maxEntries">Maximum number of entries to retrieve (default: 10)</param>
        /// <returns>A task containing the friends leaderboard if available, error message otherwise</returns>
        public async Task<Result<ILeaderboard, string>> GetFriendsLeaderboardAsync(ulong leaderboardID, int maxEntries = 10)
        {
            await SimulateNetworkDelay();

            if (!_leaderboards.TryGetValue(leaderboardID, out var leaderboard))
            {
                return new Result<ILeaderboard, string>($"Leaderboard {leaderboardID} not found");
            }

            // Mock friends list - in editor we'll just return a subset
            var friendsLeaderboard = leaderboard.GetFriendsEntries(new ulong[] { _settings.UserID, 11111, 22222 }, maxEntries);
            return new Result<ILeaderboard, string>(friendsLeaderboard);
        }

        /// <summary>
        /// Gets the current user's rank and score for a specific leaderboard
        /// </summary>
        /// <param name="leaderboardID">The unique identifier of the leaderboard</param>
        /// <returns>A task containing the user's leaderboard entry if available, error message otherwise</returns>
        public async Task<Result<ILeaderboardEntry, string>> GetUserEntryAsync(ulong leaderboardID)
        {
            return await GetUserEntryAsync(leaderboardID, _settings.UserID);
        }

        /// <summary>
        /// Gets a specific user's rank and score for a leaderboard
        /// </summary>
        /// <param name="leaderboardID">The unique identifier of the leaderboard</param>
        /// <param name="userID">The unique identifier of the user</param>
        /// <returns>A task containing the user's leaderboard entry if available, error message otherwise</returns>
        public async Task<Result<ILeaderboardEntry, string>> GetUserEntryAsync(ulong leaderboardID, ulong userID)
        {
            await SimulateNetworkDelay();

            if (!_leaderboards.TryGetValue(leaderboardID, out var leaderboard))
            {
                return new Result<ILeaderboardEntry, string>($"Leaderboard {leaderboardID} not found");
            }

            var entry = leaderboard.GetEntryByUser(userID);
            if (entry == null)
            {
                return new Result<ILeaderboardEntry, string>($"User {userID} not found on leaderboard {leaderboardID}");
            }

            return new Result<ILeaderboardEntry, string>(entry);
        }

        /// <summary>
        /// Gets leaderboard entries within a specific rank range
        /// </summary>
        /// <param name="leaderboardID">The unique identifier of the leaderboard</param>
        /// <param name="startRank">The starting rank (1-based, inclusive)</param>
        /// <param name="endRank">The ending rank (1-based, inclusive)</param>
        /// <returns>A task containing the leaderboard entries if available, error message otherwise</returns>
        public async Task<Result<ILeaderboard, string>> GetLeaderboardByRangeAsync(ulong leaderboardID, int startRank, int endRank)
        {
            await SimulateNetworkDelay();

            if (!_leaderboards.TryGetValue(leaderboardID, out var leaderboard))
            {
                return new Result<ILeaderboard, string>($"Leaderboard {leaderboardID} not found");
            }

            if (startRank < 1 || endRank < startRank)
            {
                return new Result<ILeaderboard, string>("Invalid rank range");
            }

            var rangeLeaderboard = leaderboard.GetEntriesByRange(startRank, endRank);
            return new Result<ILeaderboard, string>(rangeLeaderboard);
        }

        private void InitializeMockLeaderboards()
        {
            _leaderboards = new Dictionary<ulong, EditorLeaderboard>();

            // Create a sample leaderboard
            var sampleLeaderboard = new EditorLeaderboard(1001, "High Scores");
            
            // Add mock entries
            var mockEntries = new[]
            {
                new EditorLeaderboardEntry(new LocalUser("TopPlayer", "Pro1", 99999, UserAuthenticationStatus.Authenticated), 1, 10000),
                new EditorLeaderboardEntry(new LocalUser("SecondPlace", "Pro2", 88888, UserAuthenticationStatus.Authenticated), 2, 9500),
                new EditorLeaderboardEntry(new LocalUser("ThirdPlace", "Pro3", 77777, UserAuthenticationStatus.Authenticated), 3, 9000),
                new EditorLeaderboardEntry(new LocalUser("Friend One", "Friend1", 11111, UserAuthenticationStatus.Authenticated), 4, 8500),
                new EditorLeaderboardEntry(new LocalUser("Friend Two", "Friend2", 22222, UserAuthenticationStatus.Authenticated), 5, 8000),
                new EditorLeaderboardEntry(new LocalUser(_settings.UserName, _settings.GamerTag, _settings.UserID, UserAuthenticationStatus.Authenticated), 6, 7500),
            };

            foreach (var entry in mockEntries)
            {
                sampleLeaderboard.AddEntry(entry);
            }

            _leaderboards[1001] = sampleLeaderboard;
        }

        private async Task SimulateNetworkDelay()
        {
            if (_settings.SimulateNetworkDelay)
            {
                var delay = _settings.GetRandomNetworkDelay();
                if (delay > 0)
                {
                    await Task.Delay(Mathf.RoundToInt(delay * 1000));
                }
            }
        }
    }
}