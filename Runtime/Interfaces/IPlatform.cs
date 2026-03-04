using System;

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

        /// <summary>
        /// Gets the achievements service for managing achievement functionality.
        /// </summary>
        IAchievements Achievements { get; }

        /// <summary>
        /// Returns true if the current platform is a handheld device (e.g. Steamdeck or Nintendo Switch in handheld mode).
        /// </summary>
        bool IsUsingHandheld { get; }

        /// <summary>
        /// Occurs when the handheld usage state changes (for example, when switching between handheld and docked mode).
        /// </summary>
        /// <remarks>
        /// Subscribers can use this event to respond to changes in whether the platform is currently being used in handheld
        /// mode, such as updating UI layout, adjusting performance profiles, or reconfiguring audio and display settings.
        /// The Boolean parameter passed to handlers is the new value of <see cref="IsUsingHandheld"/> (<see langword="true"/>
        /// if the platform is now using handheld mode, <see langword="false"/> otherwise).
        /// </remarks>
        public event Action<bool> OutputDeviceChanged;

        /// <summary>
        /// Occurs when the overlay is activated or deactivated, providing the activation state as a Boolean parameter.
        /// </summary>
        /// <remarks>Subscribe to this event to respond to changes in the overlay's visibility. The
        /// Boolean parameter indicates whether the overlay is now active (<see langword="true"/>) or inactive (<see
        /// langword="false"/>). This event can be used to update UI elements or trigger related actions based on the
        /// overlay's state.</remarks>
        public event Action<bool> OverlayActivated;
    }
}