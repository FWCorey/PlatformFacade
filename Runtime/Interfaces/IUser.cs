namespace PlatformFacade
{
    public interface IUser
    {
        /// <summary>
        /// The display name of the user
        /// </summary>
        string Name { get; }
        
        /// <summary>
        /// The platform-specific gamertag of the user
        /// </summary>
        string GamerTag { get; }
        
        /// <summary>
        /// The unique identifier for the user
        /// </summary>
        ulong UserID { get; }
        
        /// <summary>
        /// The authentication status of the user
        /// </summary>
        UserAuthenticationStatus AuthenticationStatus { get; }
    }
}