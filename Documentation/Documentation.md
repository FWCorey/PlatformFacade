# Runtime Initialization Guide

## Overview

PlatformFacade provides automatic runtime initialization through the `PlatformManager` class, which uses reflection to discover and invoke `IPlatformInitializer` implementations that create `IPlatform` instances at runtime.

## Features

- **Automatic Discovery**: Uses reflection to find `IPlatformInitializer` implementations
- **Singleton Access**: Provides a static access point to the platform instance via `PlatformManager.Current`
- **Resilient Initialization**: Tries multiple initializers and succeeds on the first valid platform instance
- **Manual Control**: Supports manual initialization and platform setting

## Usage

### Automatic Initialization

The simplest way to use the platform is through automatic initialization:

```csharp
using PlatformFacade;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    async void Start()
    {
        // Accessing Current automatically initializes the platform
        var platform = PlatformManager.Current;
        
        if (platform != null)
        {
            // Use platform services
            var result = await platform.UserService.AuthenticateLocalUserAsync();
            if (result.IsSuccess)
            {
                Debug.Log($"Authenticated: {result.Value.Name}");
            }
        }
    }
}
```

### Using RuntimePlatformInitializer Component

For scene-based initialization, use the `RuntimePlatformInitializer` component:

1. Create a new GameObject in your scene (e.g., "PlatformManager")
2. Add the `RuntimePlatformInitializer` component
3. Configure initialization options:
   - **Initialize On Awake**: Initialize when the component awakens (default: true)
   - **Don't Destroy On Load**: Persist across scene loads (default: true)

The component will automatically initialize the platform and make it available through `PlatformManager.Current`.

### Manual Initialization

For more control over when initialization occurs:

```csharp
// Check if already initialized
if (!PlatformManager.IsInitialized)
{
    // Manually trigger initialization
    PlatformManager.Initialize();
}

// Access the platform
var platform = PlatformManager.Current;
```

### Setting a Custom Platform

For testing or custom scenarios, you can manually set the platform instance:

```csharp
// Create a custom platform instance
var customPlatform = new MyCustomPlatform();

// Set it as the current platform
PlatformManager.SetPlatform(customPlatform);

// Now PlatformManager.Current will return your custom instance
```

### Resetting the Platform

To reset the platform manager (primarily for testing):

```csharp
PlatformManager.Reset();

// Now you can initialize again
PlatformManager.Initialize();
```

## How It Works

### Reflection-Based Discovery

When `Initialize()` is called, the PlatformManager:

1. Scans all loaded assemblies for types implementing `IPlatformInitializer`
2. Filters out interfaces, abstract classes, and `MonoBehaviour` types
3. Logs all discovered initializer types
4. If multiple implementations are found, logs a warning and tries each initializer in an unspecified order:
   - Creates an instance via the parameterless constructor
   - Calls `InitializePlatform()` and checks the result
   - The first non-null `IPlatform` returned is accepted, sets the initialized flag, and stops
5. If no implementations produce a platform instance, logs an error and leaves the initialized flag false

Notes:
- The order of trying initializers is not guaranteed. Each initializer should self-filter (e.g., by checking settings, platform availability, or SDK presence) and return null when not applicable.
- Discovery happens on each initialization attempt. After a successful initialization, new assemblies are not considered unless you call `PlatformManager.Reset()`.

### Error Handling

The PlatformManager handles several error scenarios:

- **No Implementation Found**: Logs an error if no `IPlatformInitializer` implementation exists; keeps initialized flag false
- **Multiple Implementations**: Logs a warning listing all found implementations, then iterates through them; succeeds on the first non-null platform, otherwise logs a final error
- **Constructor/Initialization Failure**: Logs an error if the initializer constructor or `InitializePlatform()` throws an exception; continues trying remaining implementations
- **Null Platform**: Logs when an initializer returns null; continues trying remaining implementations
- **Assembly Load Errors**: Gracefully handles assemblies that fail to load types

## Best Practices

1. **Have Only One Effective Initializer**: Ensure that, for a given build/run, only one initializer actually returns a platform (others should return null when not applicable)
2. **Editor vs Runtime**: Use assembly definitions, scripting defines, or settings to separate Editor and Runtime implementations
3. **Initialization Timing**: Initialize early (in Awake or Start) to ensure platform services are available
4. **Error Checking**: Always check if `PlatformManager.Current` is null before using services
5. **Single Responsibility**: Keep platform creation logic in the initializer, separate from service implementation
6. **Self-Filtering Initializers**: Inside `InitializePlatform()`, perform environment checks (e.g., SDK present, feature enabled) and return null if not applicable

## Platform Implementation Guidelines

When creating a new platform implementation:

1. Create an `IPlatform` implementation with all required services
2. Create an `IPlatformInitializer` factory implementation
3. The initializer should be a pure factory with no platform-specific fields
4. The initializer should have a parameterless constructor for reflection
5. Perform applicability checks (settings/SDK/defines) and return null when not applicable
6. Use appropriate assembly definitions to control availability

Example:

```csharp
namespace MyGame.Platform
{
    // Platform implementation
    public class MyPlatform : IPlatform
    {
        private readonly IUserService _userService;
        private readonly ILeaderboards _leaderboards;
        // ... other services

        public MyPlatform()
        {
            _userService = new MyUserService();
            _leaderboards = new MyLeaderboards();
            // ... initialize other services
        }

        public IUserService UserService => _userService;
        public ILeaderboards Leaderboards => _leaderboards;
        // ... other service properties
    }

    // Platform initializer factory (discovered by reflection)
    // Should be a pure factory with no platform-specific fields
    public class MyPlatformInitializer : IPlatformInitializer
    {
        public IPlatform InitializePlatform()
        {
            return new MyPlatform();
        }
    }
}
```

## Troubleshooting

### "No IPlatformInitializer implementation found"

This error occurs when no class implementing `IPlatformInitializer` is found. Solutions:
- Ensure you have a platform initializer in your project
- Check that the initializer is in a loaded assembly
- Verify the class is not abstract, not a `MonoBehaviour`, and has a parameterless constructor
- Ensure the initializer implements `IPlatformInitializer`

### "Multiple IPlatformInitializer implementations found"

This message indicates more than one initializer was discovered. Behavior:
- PlatformManager logs a warning and iterates through all implementations
- Initialization succeeds on the first initializer that returns a non-null `IPlatform`

Solutions:
- Gate initializers by build target, scripting defines, or runtime checks so only one returns a platform
- Use settings (e.g., an "Enabled" flag) to disable non-target initializers
- Review logs to confirm which initializer succeeded

### "Initializer returned null platform instance"

This occurs when `InitializePlatform()` returns null. Null can be expected when an initializer is not applicable. If no platform is initialized:
- Ensure at least one initializer is applicable and returns a constructed platform
- Check SDK availability or settings used by your initializer
- Review console logs for which initializers were tried

### Platform services are null

If `PlatformManager.Current` is null:
- Check the Unity console for initialization logs and errors
- Verify a valid and applicable platform initializer exists
- Ensure the initializer's `InitializePlatform()` method creates and returns the platform
- Try calling `PlatformManager.Reset()` and then `PlatformManager.Initialize()` to retry initialization
