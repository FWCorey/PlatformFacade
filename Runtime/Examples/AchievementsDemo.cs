using System.Threading.Tasks;
using UnityEngine;

namespace PlatformFacade.Examples
{
    /// <summary>
    /// Simple demo component showing how to use the Achievements service.
    /// Add this to a GameObject to test achievement functionality.
    /// </summary>
    public class AchievementsDemo : MonoBehaviour
    {
        private IPlatform _platform;
        private IAchievements _achievements;

        private async void Start()
        {
            // Get platform instance (automatically initialized by PlatformManager)
            _platform = PlatformManager.Current;

            if (_platform == null)
            {
                Debug.LogError("Platform not initialized. Make sure PlatformManager is set up.");
                return;
            }

            // Get achievements service (may be null on platforms without achievement support)
            _achievements = _platform.Achievements;

            if (_achievements == null)
            {
                Debug.LogWarning("Achievements not supported on this platform.");
                return;
            }

            // Subscribe to achievement events
            _achievements.AchievementUnlocked += OnAchievementUnlocked;
            _achievements.AchievementProgressUpdated += OnAchievementProgressUpdated;

            // Demo: Load and display all achievements
            await DemoLoadAllAchievements();

            // Demo: Unlock an achievement
            await DemoUnlockAchievement("first_steps");

            // Demo: Update progress for an incremental achievement
            await DemoUpdateProgress("veteran", 0.5f);
        }

        private async Task DemoLoadAllAchievements()
        {
            Debug.Log("=== Loading All Achievements ===");
            var result = await _achievements.GetAllAchievementsAsync();

            if (result.IsSuccess)
            {
                var achievements = result.Value;
                Debug.Log($"Found {achievements.Count} achievements:");
                
                foreach (var achievement in achievements)
                {
                    string status = achievement.IsUnlocked ? "✓ Unlocked" : "○ Locked";
                    Debug.Log($"{status} {achievement.DisplayName} - {achievement.Description} (Progress: {achievement.Progress * 100:F0}%)");
                }
            }
            else
            {
                Debug.LogError($"Failed to load achievements: {result.Error}");
            }
        }

        private async Task DemoUnlockAchievement(string achievementID)
        {
            Debug.Log($"\n=== Unlocking Achievement: {achievementID} ===");
            var result = await _achievements.UnlockAchievementAsync(achievementID);

            if (result.IsSuccess)
            {
                Debug.Log($"Successfully unlocked achievement: {achievementID}");
            }
            else
            {
                Debug.LogError($"Failed to unlock achievement: {result.Error}");
            }
        }

        private async Task DemoUpdateProgress(string achievementID, float progress)
        {
            Debug.Log($"\n=== Updating Achievement Progress: {achievementID} to {progress * 100:F0}% ===");
            var result = await _achievements.SetAchievementProgressAsync(achievementID, progress);

            if (result.IsSuccess)
            {
                Debug.Log($"Successfully updated progress for achievement: {achievementID}");
            }
            else
            {
                Debug.LogError($"Failed to update progress: {result.Error}");
            }
        }

        private void OnAchievementUnlocked(IAchievement achievement)
        {
            Debug.Log($"🏆 ACHIEVEMENT UNLOCKED: {achievement.DisplayName}!");
        }

        private void OnAchievementProgressUpdated(IAchievement achievement)
        {
            Debug.Log($"📊 Achievement Progress: {achievement.DisplayName} - {achievement.Progress * 100:F0}%");
        }

        private void OnDestroy()
        {
            // Unsubscribe from events
            if (_achievements != null)
            {
                _achievements.AchievementUnlocked -= OnAchievementUnlocked;
                _achievements.AchievementProgressUpdated -= OnAchievementProgressUpdated;
            }
        }
    }
}
