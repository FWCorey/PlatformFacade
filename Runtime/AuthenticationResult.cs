namespace PlatformFacade
{
    /// <summary>
    /// Represents the result of an authentication operation
    /// </summary>
    public struct AuthenticationResult
    {
        /// <summary>
        /// Indicates if the authentication was successful
        /// </summary>
        public bool IsSuccess { get; }
        
        /// <summary>
        /// The authenticated user if successful, null otherwise
        /// </summary>
        public IUser User { get; }
        
        /// <summary>
        /// Error message if authentication failed
        /// </summary>
        public string ErrorMessage { get; }
        
        /// <summary>
        /// Platform-specific error code if available
        /// </summary>
        public int ErrorCode { get; }

        /// <summary>
        /// Initializes a successful authentication result
        /// </summary>
        /// <param name="user">The authenticated user</param>
        public AuthenticationResult(IUser user)
        {
            IsSuccess = true;
            User = user;
            ErrorMessage = null;
            ErrorCode = 0;
        }
        
        /// <summary>
        /// Initializes a failed authentication result
        /// </summary>
        /// <param name="errorMessage">The error message</param>
        /// <param name="errorCode">The platform-specific error code</param>
        public AuthenticationResult(string errorMessage, int errorCode = -1)
        {
            IsSuccess = false;
            User = null;
            ErrorMessage = errorMessage;
            ErrorCode = errorCode;
        }
    }
}