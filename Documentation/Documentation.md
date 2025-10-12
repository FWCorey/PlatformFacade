# Runtime Initialization Guide

## Overview

PlatformFacade provides automatic runtime initialization through the `PlatformManager` class, which uses reflection to discover and initialize IPlatform implementations at runtime.

## Features

- **Automatic Discovery**: Uses reflection to find IPlatform implementations
- **Singleton Access**: Provides a static access point to the platform instance
- **Error Handling**: Logs errors when multiple implementations are found
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
2. Filters out interfaces, abstract classes, and MonoBehaviour types
3. Checks the count of implementations:
   - **0 implementations**: Logs an error and does not set initialized flag
   - **1 implementation**: Creates an instance and calls `InitializePlatform()` which returns the platform
   - **2+ implementations**: Logs an error and does not set initialized flag
4. Sets the initialized flag only on successful platform creation

Note: The PlatformManager always uses reflection for discovery, even if a DLL is loaded later. This ensures newly loaded assemblies are discovered on the next initialization attempt.

### Error Handling

The PlatformManager handles several error scenarios:

- **No Implementation Found**: Logs an error if no IPlatformInitializer implementation exists, keeps initialized flag false
- **Multiple Implementations**: Logs an error listing all found implementations, keeps initialized flag false
- **Constructor Failure**: Logs an error if the initializer constructor throws an exception, keeps initialized flag false
- **Initialization Failure**: Logs an error if `InitializePlatform()` throws an exception, keeps initialized flag false
- **Null Platform**: Logs an error if `InitializePlatform()` returns null, keeps initialized flag false
- **Assembly Load Errors**: Gracefully handles assemblies that fail to load types

## Best Practices

1. **Single Implementation**: Ensure only one IPlatformInitializer implementation is active in your build
2. **Editor vs Runtime**: Use build constraints or assembly definitions to separate Editor and Runtime implementations
3. **Initialization Timing**: Initialize early (in Awake or Start) to ensure platform services are available
4. **Error Checking**: Always check if `PlatformManager.Current` is null before using services
5. **Single Responsibility**: Keep platform creation logic in the initializer, separate from service implementation

## Platform Implementation Guidelines

When creating a new platform implementation:

1. Create an `IPlatform` implementation with all required services
2. Create an `IPlatformInitializer` factory implementation
3. The initializer should be a pure factory with no platform-specific fields
4. The initializer should have a parameterless constructor for reflection
5. Use appropriate assembly definitions to control availability

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

This error occurs when no class implementing IPlatformInitializer is found. Solutions:
- Ensure you have a platform initializer in your project
- Check that the initializer is in a loaded assembly
- Verify the class is not abstract, not a MonoBehaviour, and has a parameterless constructor
- Ensure the initializer implements `IPlatformInitializer`

### "Multiple IPlatformInitializer implementations found"

This error occurs when more than one IPlatformInitializer implementation is discovered. Solutions:
- Remove unused platform initializers
- Use assembly definitions to separate platforms by build target
- Use preprocessor directives to conditionally compile platforms

### "Initializer returned null platform instance"

This error occurs when `InitializePlatform()` returns null. Solutions:
- Ensure your initializer's `InitializePlatform()` method creates and returns a valid platform instance
- Check that platform construction doesn't fail silently

### Platform services are null

If `PlatformManager.Current` is null:
- Check the Unity console for initialization errors
- Verify a valid platform initializer exists
- Ensure the initializer's `InitializePlatform()` method properly creates and returns the platform
- Try calling `PlatformManager.Reset()` and then `PlatformManager.Initialize()` to retry initialization
