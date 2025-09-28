namespace PlatformFacade
{
    /// <summary>
    /// A struct representing a local user implementation of IUser
    /// </summary>
    public struct LocalUser : IUser
    {
        /// <summary>
        /// The display name of the user
        /// </summary>
        public string Name { get; }
        
        /// <summary>
        /// The platform-specific gamertag of the user
        /// </summary>
        public string GamerTag { get; }
        
        /// <summary>
        /// The unique identifier for the user
        /// </summary>
        public ulong UserID { get; }

        /// <summary>
        /// Initializes a new instance of the LocalUser struct
        /// </summary>
        /// <param name="name">The display name of the user</param>
        /// <param name="gamerTag">The platform-specific gamertag of the user</param>
        /// <param name="userID">The unique identifier for the user</param>
        public LocalUser(string name, string gamerTag, ulong userID)
        {
            Name = name;
            GamerTag = gamerTag;
            UserID = userID;
        }
    }
}