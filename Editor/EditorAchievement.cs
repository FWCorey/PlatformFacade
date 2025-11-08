using System;

namespace PlatformFacade.Editor
{
    /// <summary>
    /// Editor implementation of IAchievement for Unity Editor development and testing
    /// </summary>
    public class EditorAchievement : IAchievement
    {
        /// <summary>
        /// Gets the unique identifier for this achievement
        /// </summary>
        public string AchievementID { get; private set; }
        
        /// <summary>
        /// Gets the display name of the achievement
        /// </summary>
        public string DisplayName { get; private set; }
        
        /// <summary>
        /// Gets the description of how to unlock this achievement
        /// </summary>
        public string Description { get; private set; }
        
        /// <summary>
        /// Gets whether this achievement has been unlocked by the current user
        /// </summary>
        public bool IsUnlocked { get; internal set; }
        
        /// <summary>
        /// Gets whether this achievement is hidden until unlocked
        /// </summary>
        public bool IsHidden { get; private set; }
        
        /// <summary>
        /// Gets the current progress toward unlocking this achievement (0.0 to 1.0).
        /// Returns 0.0 for non-incremental achievements, or current progress for incremental ones.
        /// </summary>
        public float Progress { get; internal set; }
        
        /// <summary>
        /// Gets the timestamp when this achievement was unlocked, or null if not yet unlocked
        /// </summary>
        public DateTime? UnlockedTime { get; internal set; }

        /// <summary>
        /// Attempts to get the icon for this achievement and write it to the provided texture
        /// </summary>
        /// <param name="writeableTexture">The texture to write the icon data to</param>
        /// <returns>True if the icon was successfully retrieved and written, false otherwise</returns>
        public bool TryGetIcon(UnityEngine.Texture2D writeableTexture)
        {
            // Editor implementation returns false as we don't have actual achievement icons
            return false;
        }

        /// <summary>
        /// Initializes a new instance of the EditorAchievement class
        /// </summary>
        /// <param name="achievementID">The unique identifier for this achievement</param>
        /// <param name="displayName">The display name of the achievement</param>
        /// <param name="description">The description of how to unlock this achievement</param>
        /// <param name="isHidden">Whether this achievement is hidden until unlocked</param>
        public EditorAchievement(string achievementID, string displayName, string description, bool isHidden = false)
        {
            AchievementID = achievementID ?? throw new ArgumentNullException(nameof(achievementID));
            DisplayName = displayName ?? throw new ArgumentNullException(nameof(displayName));
            Description = description ?? throw new ArgumentNullException(nameof(description));
            IsHidden = isHidden;
            IsUnlocked = false;
            Progress = 0f;
            UnlockedTime = null;
        }
    }
}
