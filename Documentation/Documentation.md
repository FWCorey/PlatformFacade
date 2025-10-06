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

1. Scans all loaded assemblies for types implementing `IPlatform`
2. Filters out interfaces and abstract classes
3. Checks the count of implementations:
   - **0 implementations**: Logs an error (no platform available)
   - **1 implementation**: Creates an instance and initializes it
   - **2+ implementations**: Logs an error (ambiguous platform)

### Error Handling

The PlatformManager handles several error scenarios:

- **No Implementation Found**: Logs an error if no IPlatform implementation exists
- **Multiple Implementations**: Logs an error listing all found implementations
- **Constructor Failure**: Logs an error if the platform constructor throws an exception
- **Assembly Load Errors**: Gracefully handles assemblies that fail to load types

## Best Practices

1. **Single Implementation**: Ensure only one IPlatform implementation is active in your build
2. **Editor vs Runtime**: Use build constraints or assembly definitions to separate Editor and Runtime implementations
3. **Initialization Timing**: Initialize early (in Awake or Start) to ensure platform services are available
4. **Error Checking**: Always check if `PlatformManager.Current` is null before using services

## Platform Implementation Guidelines

When creating a new platform implementation:

1. Implement the `IPlatform` interface
2. Provide a parameterless constructor
3. Initialize all required services in the constructor
4. Use appropriate assembly definitions to control availability

Example:

```csharp
namespace MyGame.Platform
{
    public class MyPlatform : IPlatform
    {
        private readonly IUserService _userService;
        private readonly ILeaderboards _leaderboards;
        // ... other services

        // Parameterless constructor required for reflection
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
}
```

## Troubleshooting

### "No IPlatform implementation found"

This error occurs when no class implementing IPlatform is found. Solutions:
- Ensure you have a platform implementation in your project
- Check that the implementation is in a loaded assembly
- Verify the class is not abstract and has a parameterless constructor

### "Multiple IPlatform implementations found"

This error occurs when more than one IPlatform implementation is discovered. Solutions:
- Remove unused platform implementations
- Use assembly definitions to separate platforms by build target
- Use preprocessor directives to conditionally compile platforms

### Platform services are null

If `PlatformManager.Current` is null:
- Check the Unity console for initialization errors
- Verify a valid platform implementation exists
- Ensure the platform constructor doesn't throw exceptions
