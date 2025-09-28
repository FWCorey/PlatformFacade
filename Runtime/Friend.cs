namespace PlatformFacade
{
    /// <summary>
    /// A concrete implementation of IFriend
    /// </summary>
    public struct Friend : IFriend
    {
        /// <summary>
        /// The display name of the friend
        /// </summary>
        public string Name { get; }
        
        /// <summary>
        /// The platform-specific gamertag of the friend
        /// </summary>
        public string GamerTag { get; }
        
        /// <summary>
        /// The unique identifier for the friend
        /// </summary>
        public ulong UserID { get; }
        
        /// <summary>
        /// Indicates if the friend is currently online
        /// </summary>
        public bool IsOnline { get; }
        
        /// <summary>
        /// The current status message or presence information of the friend
        /// </summary>
        public string Status { get; }

        /// <summary>
        /// Initializes a new instance of the Friend struct
        /// </summary>
        /// <param name="name">The display name of the friend</param>
        /// <param name="gamerTag">The platform-specific gamertag of the friend</param>
        /// <param name="userID">The unique identifier for the friend</param>
        /// <param name="isOnline">Indicates if the friend is currently online</param>
        /// <param name="status">The current status message or presence information</param>
        public Friend(string name, string gamerTag, ulong userID, bool isOnline, string status)
        {
            Name = name ?? string.Empty;
            GamerTag = gamerTag ?? string.Empty;
            UserID = userID;
            IsOnline = isOnline;
            Status = status ?? string.Empty;
        }
    }
}