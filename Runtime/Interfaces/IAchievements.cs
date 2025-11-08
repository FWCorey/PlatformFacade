using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlatformFacade
{
    /// <summary>
    /// Provides achievement services across gaming platforms including achievement unlocking,
    /// progress tracking, and achievement queries
    /// </summary>
    public interface IAchievements
    {
        /// <summary>
        /// Event fired when an achievement is unlocked
        /// </summary>
        event Action<IAchievement> AchievementUnlocked;
        
        /// <summary>
        /// Event fired when achievement progress is updated
        /// </summary>
        event Action<IAchievement> AchievementProgressUpdated;

        /// <summary>
        /// Unlocks the specified achievement for the current user
        /// </summary>
        /// <param name="achievementID">The unique identifier of the achievement</param>
        /// <returns>A task containing the result of the unlock operation</returns>
        Task<Result<bool, string>> UnlockAchievementAsync(string achievementID);
        
        /// <summary>
        /// Updates progress toward an incremental achievement
        /// </summary>
        /// <param name="achievementID">The unique identifier of the achievement</param>
        /// <param name="progress">The progress value (0.0 to 1.0)</param>
        /// <returns>A task containing the result of the progress update</returns>
        Task<Result<bool, string>> SetAchievementProgressAsync(string achievementID, float progress);
        
        /// <summary>
        /// Increments progress toward an incremental achievement by a specific amount
        /// </summary>
        /// <param name="achievementID">The unique identifier of the achievement</param>
        /// <param name="increment">The amount to increment (e.g., 0.1 for 10%)</param>
        /// <returns>A task containing the result of the increment operation</returns>
        Task<Result<bool, string>> IncrementAchievementProgressAsync(string achievementID, float increment);
        
        /// <summary>
        /// Gets information about a specific achievement
        /// </summary>
        /// <param name="achievementID">The unique identifier of the achievement</param>
        /// <returns>A task containing the achievement if available, error message otherwise</returns>
        Task<Result<IAchievement, string>> GetAchievementAsync(string achievementID);
        
        /// <summary>
        /// Gets all achievements for the current platform
        /// </summary>
        /// <returns>A task containing the list of all achievements if available, error message otherwise</returns>
        Task<Result<IReadOnlyList<IAchievement>, string>> GetAllAchievementsAsync();
        
        /// <summary>
        /// Gets all unlocked achievements for the current user
        /// </summary>
        /// <returns>A task containing the list of unlocked achievements if available, error message otherwise</returns>
        Task<Result<IReadOnlyList<IAchievement>, string>> GetUnlockedAchievementsAsync();
        
        /// <summary>
        /// Gets all locked (not yet unlocked) achievements for the current user
        /// </summary>
        /// <returns>A task containing the list of locked achievements if available, error message otherwise</returns>
        Task<Result<IReadOnlyList<IAchievement>, string>> GetLockedAchievementsAsync();
        
        /// <summary>
        /// Resets all achievement progress for the current user (typically for testing purposes only)
        /// </summary>
        /// <returns>A task containing the result of the reset operation</returns>
        Task<Result<bool, string>> ResetAchievementsAsync();
    }
}
