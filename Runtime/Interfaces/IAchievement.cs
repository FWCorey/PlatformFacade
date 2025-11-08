namespace PlatformFacade
{
    /// <summary>
    /// Represents a single achievement in a gaming platform
    /// </summary>
    public interface IAchievement
    {
        /// <summary>
        /// Gets the unique identifier for this achievement
        /// </summary>
        string AchievementID { get; }
        
        /// <summary>
        /// Gets the display name of the achievement
        /// </summary>
        string DisplayName { get; }
        
        /// <summary>
        /// Gets the description of how to unlock this achievement
        /// </summary>
        string Description { get; }
        
        /// <summary>
        /// Gets whether this achievement has been unlocked by the current user
        /// </summary>
        bool IsUnlocked { get; }
        
        /// <summary>
        /// Gets whether this achievement is hidden until unlocked
        /// </summary>
        bool IsHidden { get; }
        
        /// <summary>
        /// Gets the current progress toward unlocking this achievement (0.0 to 1.0).
        /// Returns 0.0 for non-incremental achievements, or current progress for incremental ones.
        /// </summary>
        float Progress { get; }
        
        /// <summary>
        /// Gets the timestamp when this achievement was unlocked, or null if not yet unlocked
        /// </summary>
        System.DateTime? UnlockedTime { get; }
    }
}
