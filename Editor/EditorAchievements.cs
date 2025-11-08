using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace PlatformFacade.Editor
{
    /// <summary>
    /// Editor implementation of IAchievements providing mock achievement services for Unity Editor development and testing
    /// </summary>
    public class EditorAchievements : IAchievements
    {
        private readonly EditorPlatformSettings _settings;
        private readonly Dictionary<string, EditorAchievement> _achievements;

        /// <summary>
        /// Gets whether achievements are supported on the current platform
        /// </summary>
        public bool IsSupported => true;

        /// <summary>
        /// Event fired when an achievement is unlocked
        /// </summary>
        public event Action<IAchievement> AchievementUnlocked;
        
        /// <summary>
        /// Event fired when achievement progress is updated
        /// </summary>
        public event Action<IAchievement> AchievementProgressUpdated;
        
        /// <summary>
        /// Event fired when achievements are synced and available locally for display
        /// </summary>
        public event Action AchievementsSynced;

        /// <summary>
        /// Initializes a new instance of the EditorAchievements class
        /// </summary>
        /// <param name="settings">The editor platform settings to use for configuration</param>
        public EditorAchievements(EditorPlatformSettings settings)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _achievements = new Dictionary<string, EditorAchievement>();
            InitializeDefaultAchievements();
            
            // Fire synced event as achievements are immediately available in editor
            AchievementsSynced?.Invoke();
        }

        /// <summary>
        /// Initializes default sample achievements for testing
        /// </summary>
        private void InitializeDefaultAchievements()
        {
            _achievements.Add("first_steps", new EditorAchievement(
                "first_steps",
                "First Steps",
                "Complete the tutorial",
                false
            ));
            
            _achievements.Add("veteran", new EditorAchievement(
                "veteran",
                "Veteran Player",
                "Play 100 matches",
                false
            ));
            
            _achievements.Add("perfectionist", new EditorAchievement(
                "perfectionist",
                "Perfectionist",
                "Achieve a perfect score",
                false
            ));
            
            _achievements.Add("secret_finder", new EditorAchievement(
                "secret_finder",
                "???",
                "Find the hidden secret",
                true
            ));
            
            _achievements.Add("collector", new EditorAchievement(
                "collector",
                "Collector",
                "Collect all items",
                false
            ));
        }

        /// <summary>
        /// Helper method to apply simulated network delay if configured
        /// </summary>
        private async Task ApplySimulatedDelayAsync()
        {
            
            if (_settings?.SimulateNetworkDelay ?? false)
            {
                int delayMs = (int)(_settings.GetRandomNetworkDelay() * 1000);
                await Task.Delay(delayMs);
            }
        }

        /// <summary>
        /// Unlocks the specified achievement for the current user
        /// </summary>
        /// <param name="achievementID">The unique identifier of the achievement</param>
        /// <returns>A task containing the result of the unlock operation</returns>
        public async Task<Result<bool, string>> UnlockAchievementAsync(string achievementID)
        {
            await ApplySimulatedDelayAsync();

            if (string.IsNullOrEmpty(achievementID))
            {
                return new Result<bool, string>("Achievement ID cannot be null or empty");
            }

            if (!_achievements.TryGetValue(achievementID, out var achievement))
            {
                return new Result<bool, string>($"Achievement '{achievementID}' not found");
            }

            if (achievement.IsUnlocked)
            {
                Debug.Log($"Achievement '{achievementID}' is already unlocked");
                return new Result<bool, string>(true);
            }

            achievement.IsUnlocked = true;
            achievement.Progress = 1f;
            achievement.UnlockedTime = DateTime.UtcNow;

            Debug.Log($"Achievement unlocked: {achievement.DisplayName}");
            AchievementUnlocked?.Invoke(achievement);

            return new Result<bool, string>(true);
        }

        /// <summary>
        /// Updates progress toward an incremental achievement
        /// </summary>
        /// <param name="achievementID">The unique identifier of the achievement</param>
        /// <param name="progress">The progress value (0.0 to 1.0)</param>
        /// <returns>A task containing the result of the progress update</returns>
        public async Task<Result<bool, string>> SetAchievementProgressAsync(string achievementID, float progress)
        {
            await ApplySimulatedDelayAsync();

            if (string.IsNullOrEmpty(achievementID))
            {
                return new Result<bool, string>("Achievement ID cannot be null or empty");
            }

            if (progress < 0f || progress > 1f)
            {
                return new Result<bool, string>("Progress must be between 0.0 and 1.0");
            }

            if (!_achievements.TryGetValue(achievementID, out var achievement))
            {
                return new Result<bool, string>($"Achievement '{achievementID}' not found");
            }

            if (achievement.IsUnlocked)
            {
                Debug.Log($"Achievement '{achievementID}' is already unlocked");
                return new Result<bool, string>(true);
            }

            achievement.Progress = progress;
            Debug.Log($"Achievement progress updated: {achievement.DisplayName} - {progress * 100:F1}%");
            AchievementProgressUpdated?.Invoke(achievement);

            // Auto-unlock if progress reaches 100%
            if (progress >= 1f)
            {
                achievement.IsUnlocked = true;
                achievement.UnlockedTime = DateTime.UtcNow;
                Debug.Log($"Achievement unlocked: {achievement.DisplayName}");
                AchievementUnlocked?.Invoke(achievement);
            }

            return new Result<bool, string>(true);
        }

        /// <summary>
        /// Increments progress toward an incremental achievement by a specific amount
        /// </summary>
        /// <param name="achievementID">The unique identifier of the achievement</param>
        /// <param name="increment">The amount to increment (e.g., 0.1 for 10%)</param>
        /// <returns>A task containing the result of the increment operation</returns>
        public async Task<Result<bool, string>> IncrementAchievementProgressAsync(string achievementID, float increment)
        {
            await ApplySimulatedDelayAsync();

            if (string.IsNullOrEmpty(achievementID))
            {
                return new Result<bool, string>("Achievement ID cannot be null or empty");
            }

            if (!_achievements.TryGetValue(achievementID, out var achievement))
            {
                return new Result<bool, string>($"Achievement '{achievementID}' not found");
            }

            if (achievement.IsUnlocked)
            {
                Debug.Log($"Achievement '{achievementID}' is already unlocked");
                return new Result<bool, string>(true);
            }

            float newProgress = Mathf.Clamp01(achievement.Progress + increment);
            return await SetAchievementProgressAsync(achievementID, newProgress);
        }

        /// <summary>
        /// Gets information about a specific achievement
        /// </summary>
        /// <param name="achievementID">The unique identifier of the achievement</param>
        /// <returns>A task containing the achievement if available, error message otherwise</returns>
        public async Task<Result<IAchievement, string>> GetAchievementAsync(string achievementID)
        {
            await ApplySimulatedDelayAsync();

            if (string.IsNullOrEmpty(achievementID))
            {
                return new Result<IAchievement, string>("Achievement ID cannot be null or empty");
            }

            if (!_achievements.TryGetValue(achievementID, out var achievement))
            {
                return new Result<IAchievement, string>($"Achievement '{achievementID}' not found");
            }

            return new Result<IAchievement, string>(achievement);
        }

        /// <summary>
        /// Gets all achievements for the current platform
        /// </summary>
        /// <returns>A task containing the list of all achievements if available, error message otherwise</returns>
        public async Task<Result<IReadOnlyList<IAchievement>, string>> GetAllAchievementsAsync()
        {
            await ApplySimulatedDelayAsync();

            var allAchievements = _achievements.Values.Cast<IAchievement>().ToList().AsReadOnly();
            return new Result<IReadOnlyList<IAchievement>, string>(allAchievements);
        }

        /// <summary>
        /// Gets all unlocked achievements for the current user
        /// </summary>
        /// <returns>A task containing the list of unlocked achievements if available, error message otherwise</returns>
        public async Task<Result<IReadOnlyList<IAchievement>, string>> GetUnlockedAchievementsAsync()
        {
            await ApplySimulatedDelayAsync();

            var unlockedAchievements = _achievements.Values
                .Where(a => a.IsUnlocked)
                .Cast<IAchievement>()
                .ToList()
                .AsReadOnly();
            
            return new Result<IReadOnlyList<IAchievement>, string>(unlockedAchievements);
        }

        /// <summary>
        /// Gets all locked (not yet unlocked) achievements for the current user
        /// </summary>
        /// <returns>A task containing the list of locked achievements if available, error message otherwise</returns>
        public async Task<Result<IReadOnlyList<IAchievement>, string>> GetLockedAchievementsAsync()
        {
            await ApplySimulatedDelayAsync();

            var lockedAchievements = _achievements.Values
                .Where(a => !a.IsUnlocked)
                .Cast<IAchievement>()
                .ToList()
                .AsReadOnly();
            
            return new Result<IReadOnlyList<IAchievement>, string>(lockedAchievements);
        }

        /// <summary>
        /// Resets all achievement progress for the current user (typically for testing purposes only)
        /// </summary>
        /// <returns>A task containing the result of the reset operation</returns>
        public async Task<Result<bool, string>> ResetAchievementsAsync()
        {
            await ApplySimulatedDelayAsync();

            foreach (var achievement in _achievements.Values)
            {
                achievement.IsUnlocked = false;
                achievement.Progress = 0f;
                achievement.UnlockedTime = null;
            }

            Debug.Log("All achievements have been reset");
            return new Result<bool, string>(true);
        }
    }
}
