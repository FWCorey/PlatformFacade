using System;

namespace PlatformFacade.Examples
{
    /// <summary>
    /// Example usage patterns for IPlatform interface demonstrating access to core services
    /// </summary>
    public class PlatformUsage
    {
        private readonly IPlatform _platform;

        public PlatformUsage(IPlatform platform)
        {
            _platform = platform ?? throw new ArgumentNullException(nameof(platform));
        }

        /// <summary>
        /// Example: Accessing user service through the platform interface
        /// </summary>
        public void AccessUserService()
        {
            var userService = _platform.UserService;
            var localUser = userService.GetLocalUser();
            // Use user service for authentication, friends, etc.
        }

        /// <summary>
        /// Example: Accessing multiplayer service through the platform interface
        /// </summary>
        public void AccessMultiplayerService()
        {
            var multiplayerService = _platform.MultiplayerService;
            // Use multiplayer service for matchmaking, lobbies, etc.
        }

        /// <summary>
        /// Example: Accessing storage service through the platform interface
        /// </summary>
        public void AccessStorageService()
        {
            var storage = _platform.Storage;
            // Use storage for save data, cloud saves, etc.
        }
    }
}