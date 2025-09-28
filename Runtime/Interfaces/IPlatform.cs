namespace PlatformFacade
{
    /// <summary>
    /// Main manager interface for platform implementations providing access to core platform services.
    /// This interface serves as a unified entry point to platform-specific functionality while maintaining
    /// a lean design to avoid creating a god class.
    /// </summary>
    public interface IPlatform
    {
        /// <summary>
        /// Gets the user service for managing authentication, friend lists, and user portraits
        /// </summary>
        IUserService UserService { get; }
        
        /// <summary>
        /// Gets the multiplayer service for managing multiplayer functionality
        /// </summary>
        IMultiplayerService MultiplayerService { get; }
        
        /// <summary>
        /// Gets the storage service for managing platform-specific data persistence
        /// </summary>
        IStorage Storage { get; }
        
        /// <summary>
        /// Gets the leaderboards service for managing leaderboard functionality
        /// </summary>
        ILeaderboards Leaderboards { get; }
    }
}