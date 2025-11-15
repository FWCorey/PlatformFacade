# PlatformFacade
A generic Platform facade to abstract platform SDKs from your Unity projects

## Overview

PlatformFacade implements the Facade design pattern to create a unified interface for various gaming platform services like user authentication, leaderboards, achievements, multiplayer, storage, and overlays. The package uses Railway Oriented Design for async operations, creating robust and composable async workflows.

## Features

- **Unified API**: Single interface for multiple gaming platforms
- **Railway Oriented Design**: Functional programming approach for handling success/failure paths
- **Editor Support**: Complete mock implementation for Unity Editor development and testing
- **Extensible**: Easy to add new platform implementations
- **Type Safe**: Full C# type safety with comprehensive interfaces

## Platform Support

### Currently Implemented
- **EditorPlatform**: Mock implementation for Unity Editor development and testing

## Installation

1. Add the package to your Unity project via Package Manager
2. Import the `PlatformFacade` namespace in your scripts
3. Create platform-specific implementations or use the provided `EditorPlatform` for testing

## Quick Start

### Runtime Initialization (Automatic)

The easiest way to use PlatformFacade is through the automatic runtime initialization:

```csharp
using PlatformFacade;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    async void Start()
    {
        // PlatformManager automatically discovers and initializes the platform
        var platform = PlatformManager.Current;
        
        if (platform != null)
        {
            // Authenticate user
            var result = await platform.UserService.AuthenticateLocalUserAsync();
            
            if (result.IsSuccess)
            {
                Debug.Log($"Authenticated as: {result.Value.Name}");
            }
        }
    }
}
```

You can also use the `RuntimePlatformInitializer` component:
1. Add the `RuntimePlatformInitializer` component to a GameObject in your scene
2. It will automatically initialize the platform on Awake
3. Access the platform via `PlatformManager.Current` from anywhere in your code

The `PlatformManager` uses reflection to automatically find and instantiate the appropriate `IPlatform` implementation for your build target. It will log an error if multiple implementations are found.

For more details on runtime initialization, see the [Runtime Initialization Documentation](Documentation/Documentation.md).

### Using EditorPlatform for Development

```csharp
using PlatformFacade;
using PlatformFacade.Editor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private IPlatform _platform;
    
    void Start()
    {
        // Create EditorPlatform for development/testing
        _platform = EditorPlatform.CreateDefault();
        
        // Authenticate user
        AuthenticateUser();
    }
    
    async void AuthenticateUser()
    {
        var result = await _platform.UserService.AuthenticateLocalUserAsync();
        
        if (result.IsSuccess)
        {
            Debug.Log($"Authenticated as: {result.Value.Name}");
        }
        else
        {
            Debug.LogError($"Authentication failed: {result.Error}");
        }
    }
}
```

### Configuring EditorPlatform

1. Create an `EditorPlatformSettings` asset:
   - Right-click in Project window
   - Choose "Create > Platform Facade > Editor Platform Settings"
   - Configure user name, portrait, and simulation settings

2. Use the settings in your code:
```csharp
public EditorPlatformSettings editorSettings;

void Start()
{
    var platform = EditorPlatform.CreateWithSettings(editorSettings);
    // Use platform...
}
```

## Core Interfaces

### IPlatform
Main entry point providing access to all platform services.

### IUserService
- User authentication
- Friends list management
- User portrait retrieval

### ILeaderboards
- Score submission
- Leaderboard retrieval (global, friends, around user)
- Rank queries

### IAchievements
- Achievement unlocking
- Progress tracking for incremental achievements
- Query all, unlocked, or locked achievements
- Event notifications for unlocks and progress updates
- Optional service (returns null on platforms without achievement support)

### IStorage
Platform-specific data persistence (interface ready for implementation).

### IMultiplayerService
Multiplayer functionality (interface ready for implementation).

## Railway Oriented Design

The package uses Railway Oriented Design for async operations:

```csharp
// Chain operations together
await userService.AuthenticateLocalUserAsync()
    .Then(async user => 
    {
        Debug.Log($"Authenticated as: {user.Name}");
        return await userService.GetFriendsListAsync();
    })
    .Then(async friendsList => 
    {
        Debug.Log($"Loaded {friendsList.Count} friends");
        return await userService.GetUserPortraitThumbnailAsync();
    });
```

## Development & Testing

The `EditorPlatform` provides a complete mock implementation perfect for:
- Unity Editor development
- Unit testing
- Integration testing
- Prototyping

### Features of EditorPlatform:
- Configurable mock user data
- Simulated network delays
- Sample leaderboard data
- Sample achievement data
- Mock friends list
- Portrait configuration via ScriptableObject

## Contributing

1. Follow the existing code style and patterns
2. Add comprehensive XML documentation
3. Use Railway Oriented Design for async operations
4. Keep interfaces platform-agnostic
5. Add appropriate unit tests

## License

MIT License - see LICENSE file for details.
