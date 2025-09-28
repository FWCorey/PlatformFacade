namespace PlatformFacade
{
    /// <summary>
    /// Represents a friend in the user's friendlist across gaming platforms
    /// </summary>
    public interface IFriend
    {
        /// <summary>
        /// The display name of the friend
        /// </summary>
        string Name { get; }
        
        /// <summary>
        /// The platform-specific gamertag of the friend
        /// </summary>
        string GamerTag { get; }
        
        /// <summary>
        /// The unique identifier for the friend
        /// </summary>
        ulong UserID { get; }
        
        /// <summary>
        /// Indicates if the friend is currently online
        /// </summary>
        bool IsOnline { get; }
        
        /// <summary>
        /// The current status message or presence information of the friend
        /// </summary>
        string Status { get; }
    }
}