namespace PlatformFacade
{
    public interface IPlatformInitializer
    {
        public bool PlatformInitialized { get; }
        
        public void InitializePlatform();
    }
}
