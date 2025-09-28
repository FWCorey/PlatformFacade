namespace PlatformFacade
{
    /// <summary>
    /// Represents the authentication status of a user across gaming platforms
    /// </summary>
    public enum UserAuthenticationStatus
    {
        /// <summary>
        /// User is not authenticated
        /// </summary>
        NotAuthenticated,
        
        /// <summary>
        /// User authentication is in progress
        /// </summary>
        Authenticating,
        
        /// <summary>
        /// User is successfully authenticated
        /// </summary>
        Authenticated
    }
}