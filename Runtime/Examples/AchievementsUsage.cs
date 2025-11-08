using System;
using System.Threading.Tasks;
using UnityEngine;

namespace PlatformFacade.Examples
{
    /// <summary>
    /// Example usage patterns for IAchievements demonstrating common scenarios
    /// across gaming platforms (iOS, Android, Epic Store, Steamworks, Nintendo Switch, 
    /// Xbox GameServices, PlayStation, Windows Store) using Railway Oriented Design
    /// </summary>
    public class AchievementsUsage
    {
        private readonly IAchievements _achievements;

        public AchievementsUsage(IAchievements achievements)
        {
            _achievements = achievements ?? throw new ArgumentNullException(nameof(achievements));
            
            // Subscribe to achievement events
            _achievements.AchievementUnlocked += OnAchievementUnlocked;
            _achievements.AchievementProgressUpdated += OnAchievementProgressUpdated;
            _achievements.AchievementsSynced += OnAchievementsSynced;
        }

        /// <summary>
        /// Example: Unlock an achievement using Railway Oriented Design
        /// </summary>
        public async Task UnlockAchievementAsync(string achievementID)
        {
            var result = await _achievements.UnlockAchievementAsync(achievementID);
            
            if (result.IsSuccess)
            {
                Debug.Log($"Achievement {achievementID} unlocked successfully");
            }
            else
            {
                Debug.LogError($"Failed to unlock achievement: {result.Error}");
            }
        }

        /// <summary>
        /// Example: Update progress for an incremental achievement
        /// </summary>
        public async Task UpdateAchievementProgressAsync(string achievementID, float progress)
        {
            var result = await _achievements.SetAchievementProgressAsync(achievementID, progress);
            
            if (result.IsSuccess)
            {
                Debug.Log($"Achievement progress updated: {achievementID} - {progress * 100:F1}%");
            }
            else
            {
                Debug.LogError($"Failed to update achievement progress: {result.Error}");
            }
        }

        /// <summary>
        /// Example: Increment achievement progress by a specific amount
        /// </summary>
        public async Task IncrementAchievementProgressAsync(string achievementID, float increment)
        {
            var result = await _achievements.IncrementAchievementProgressAsync(achievementID, increment);
            
            if (result.IsSuccess)
            {
                Debug.Log($"Achievement progress incremented: {achievementID} by {increment * 100:F1}%");
            }
            else
            {
                Debug.LogError($"Failed to increment achievement progress: {result.Error}");
            }
        }

        /// <summary>
        /// Example: Load and display all achievements using Railway Oriented Design
        /// </summary>
        public async Task<Result<int, string>> LoadAllAchievementsAsync()
        {
            var result = await _achievements.GetAllAchievementsAsync();
            
            if (result.IsSuccess)
            {
                var achievements = result.Value;
                Debug.Log($"Loaded {achievements.Count} total achievements");
                
                foreach (var achievement in achievements)
                {
                    var status = achievement.IsUnlocked ? "Unlocked" : "Locked";
                    var hiddenTag = achievement.IsHidden && !achievement.IsUnlocked ? " [Hidden]" : "";
                    Debug.Log($"- {achievement.DisplayName} ({status}){hiddenTag} - Progress: {achievement.Progress * 100:F1}%");
                }
                
                return new Result<int, string>(achievements.Count);
            }
            else
            {
                Debug.LogError($"Failed to load achievements: {result.Error}");
                return new Result<int, string>(result.Error);
            }
        }

        /// <summary>
        /// Example: Load only unlocked achievements
        /// </summary>
        public async Task LoadUnlockedAchievementsAsync()
        {
            var result = await _achievements.GetUnlockedAchievementsAsync();
            
            if (result.IsSuccess)
            {
                var achievements = result.Value;
                Debug.Log($"Player has unlocked {achievements.Count} achievements:");
                
                foreach (var achievement in achievements)
                {
                    var unlockedTime = achievement.UnlockedTime?.ToString("g") ?? "Unknown";
                    Debug.Log($"- {achievement.DisplayName} (Unlocked: {unlockedTime})");
                }
            }
            else
            {
                Debug.LogError($"Failed to load unlocked achievements: {result.Error}");
            }
        }

        /// <summary>
        /// Example: Load achievements that are still locked
        /// </summary>
        public async Task LoadLockedAchievementsAsync()
        {
            var result = await _achievements.GetLockedAchievementsAsync();
            
            if (result.IsSuccess)
            {
                var achievements = result.Value;
                Debug.Log($"{achievements.Count} achievements remaining to unlock:");
                
                foreach (var achievement in achievements)
                {
                    if (!achievement.IsHidden)
                    {
                        Debug.Log($"- {achievement.DisplayName}: {achievement.Description} (Progress: {achievement.Progress * 100:F1}%)");
                    }
                    else
                    {
                        Debug.Log($"- ??? (Hidden achievement)");
                    }
                }
            }
            else
            {
                Debug.LogError($"Failed to load locked achievements: {result.Error}");
            }
        }

        /// <summary>
        /// Example: Get information about a specific achievement
        /// </summary>
        public async Task<Result<IAchievement, string>> GetAchievementDetailsAsync(string achievementID)
        {
            return await _achievements.GetAchievementAsync(achievementID);
        }

        /// <summary>
        /// Example: Chain multiple achievement operations using Railway Oriented Design
        /// </summary>
        public async Task CompleteAchievementWorkflowAsync(string achievementID, float progressIncrement)
        {
            await _achievements.IncrementAchievementProgressAsync(achievementID, progressIncrement)
                .Then(async success => 
                {
                    Debug.Log("Progress incremented successfully");
                    return await _achievements.GetAchievementAsync(achievementID);
                })
                .Then(async achievement => 
                {
                    Debug.Log($"Current progress: {achievement.Progress * 100:F1}%");
                    if (achievement.IsUnlocked)
                    {
                        Debug.Log($"Achievement unlocked: {achievement.DisplayName}!");
                    }
                    return await _achievements.GetUnlockedAchievementsAsync();
                });
        }

        /// <summary>
        /// Example: Reset all achievements (for testing purposes)
        /// </summary>
        public async Task ResetAllAchievementsAsync()
        {
            var result = await _achievements.ResetAchievementsAsync();
            
            if (result.IsSuccess)
            {
                Debug.Log("All achievements have been reset");
            }
            else
            {
                Debug.LogError($"Failed to reset achievements: {result.Error}");
            }
        }

        /// <summary>
        /// Event handler for achievement unlocked
        /// </summary>
        private void OnAchievementUnlocked(IAchievement achievement)
        {
            Debug.Log($"🏆 Achievement Unlocked: {achievement.DisplayName}");
            Debug.Log($"   {achievement.Description}");
        }

        /// <summary>
        /// Event handler for achievement progress updated
        /// </summary>
        private void OnAchievementProgressUpdated(IAchievement achievement)
        {
            Debug.Log($"Achievement progress: {achievement.DisplayName} - {achievement.Progress * 100:F1}%");
        }

        /// <summary>
        /// Event handler for achievements synced
        /// </summary>
        private void OnAchievementsSynced()
        {
            Debug.Log("Achievements synced and available for display");
        }
    }
}
