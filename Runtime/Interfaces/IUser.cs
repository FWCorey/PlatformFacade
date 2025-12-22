using System;

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
        /// The unique identifier for the user (128-bit, supports all platforms)
        /// </summary>
        Guid UserID { get; }
    }

    public interface IAuthenticatedUser : IUser
    {
        /// <summary>
        /// The authentication status of the user
        /// </summary>
        UserAuthenticationStatus AuthenticationStatus { get; }
    }
}