using System;
using System.Threading.Tasks;
using UnityEngine;

namespace PlatformFacade.Examples
{
    /// <summary>
    /// Example usage patterns for ILeaderboards demonstrating common scenarios
    /// across gaming platforms (iOS, Android, Epic Store, Steamworks, Nintendo Switch, 
    /// Xbox GameServices, PlayStation, Windows Store) using Railway Oriented Design
    /// </summary>
    public class LeaderboardsUsage
    {
        private readonly ILeaderboards _leaderboards;

        public LeaderboardsUsage(ILeaderboards leaderboards)
        {
            _leaderboards = leaderboards ?? throw new ArgumentNullException(nameof(leaderboards));
            
            // Subscribe to leaderboard events
            _leaderboards.LeaderboardUpdated += OnLeaderboardUpdated;
            _leaderboards.ScoreUpdated += OnScoreUpdated;
        }

        /// <summary>
        /// Example: Submit a high score using Railway Oriented Design
        /// </summary>
        public async Task SubmitHighScoreAsync(string leaderboardID, long score)
        {
            var result = await _leaderboards.SubmitScoreAsync(leaderboardID, score);
            
            if (result.IsSuccess)
            {
                Debug.Log($"Score {score} submitted successfully to leaderboard {leaderboardID}");
            }
            else
            {
                Debug.LogError($"Failed to submit score: {result.Error}");
            }
        }

        /// <summary>
        /// Example: Load and display global leaderboard using Railway Oriented Design
        /// </summary>
        public async Task<Result<ILeaderboard, string>> LoadGlobalLeaderboardAsync(string leaderboardID)
        {
            return await _leaderboards.GetGlobalLeaderboardAsync(leaderboardID, 10);
        }

        /// <summary>
        /// Example: Get user's current rank and score using Railway Oriented Design
        /// </summary>
        public async Task<Result<ILeaderboardEntry, string>> GetUserRankAsync(string leaderboardID)
        {
            return await _leaderboards.GetUserEntryAsync(leaderboardID);
        }

        /// <summary>
        /// Example: Load friends leaderboard using Railway Oriented Design
        /// </summary>
        public async Task LoadFriendsLeaderboardAsync(string leaderboardID)
        {
            var result = await _leaderboards.GetFriendsLeaderboardAsync(leaderboardID, 20);
            
            if (result.IsSuccess)
            {
                var leaderboard = result.Value;
                Debug.Log($"Loaded friends leaderboard '{leaderboard.DisplayName}' with {leaderboard.Count} entries");
                
                foreach (var entry in leaderboard)
                {
                    Debug.Log($"Rank {entry.Rank}: {entry.User.Name} - {entry.Score} points");
                }
            }
            else
            {
                Debug.LogError($"Failed to load friends leaderboard: {result.Error}");
            }
        }

        /// <summary>
        /// Example: Chain multiple leaderboard operations using Railway Oriented Design
        /// </summary>
        public async Task CompleteLeaderboardWorkflowAsync(string leaderboardID, long newScore)
        {
            await _leaderboards.SubmitScoreAsync(leaderboardID, newScore)
                .Then(async success => 
                {
                    Debug.Log("Score submitted successfully");
                    return await _leaderboards.GetUserEntryAsync(leaderboardID);
                })
                .Then(async userEntry => 
                {
                    Debug.Log($"User rank: {userEntry.Rank}, Score: {userEntry.Score}");
                    return await _leaderboards.GetLeaderboardAroundUserAsync(leaderboardID, 5);
                });
        }

        /// <summary>
        /// Example: Load leaderboard entries within a specific rank range
        /// </summary>
        public async Task LoadTopPlayersAsync(string leaderboardID)
        {
            var result = await _leaderboards.GetLeaderboardByRangeAsync(leaderboardID, 1, 10);
            
            if (result.IsSuccess)
            {
                var leaderboard = result.Value;
                Debug.Log($"Top 10 players in '{leaderboard.DisplayName}':");
                
                foreach (var entry in leaderboard)
                {
                    Debug.Log($"#{entry.Rank}: {entry.User.Name} - {entry.Score}");
                }
            }
            else
            {
                Debug.LogError($"Failed to load top players: {result.Error}");
            }
        }

        /// <summary>
        /// Event handler for leaderboard updates
        /// </summary>
        private void OnLeaderboardUpdated(string leaderboardID)
        {
            Debug.Log($"Leaderboard {leaderboardID} has been updated");
        }

        /// <summary>
        /// Event handler for score updates
        /// </summary>
        private void OnScoreUpdated(string leaderboardID, long newScore)
        {
            Debug.Log($"Your score on {leaderboardID} has been updated to {newScore}");
        }
    }
}