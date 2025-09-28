namespace Runtime.Interfaces
{
    public enum AuthenticationStatus
    {
        Unauthenticated,
        Authenticated,
        Guest,
        ExternalProvider,
        PlatformSpecific
    }
}