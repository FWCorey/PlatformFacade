# Copilot Instructions for PlatformFacade

## Project Overview

PlatformFacade is a Unity package that provides a generic platform facade to abstract platform SDKs from Unity projects. This library implements the Facade design pattern to create a unified interface for various gaming platform services like user authentication, leaderboards, multiplayer, storage, and overlays.

The package uses Railway Oriented Design for async operations, which is a functional programming approach that chains operations together while gracefully handling success and failure paths. This pattern helps create more robust and composable async workflows by explicitly handling both happy path results and error conditions in a linear, readable manner.

## Project Structure

- **Runtime/**: Contains the main source code
  - **Interfaces/**: All interface definitions for platform services
  - **LocalUser.cs**: Local implementation of the IUser interface
  - **UserAuthenticationStatus.cs**: Enum for user authentication states
  - **PlatformFacade.asmdef**: Unity assembly definition

## Architecture Guidelines

### Design Patterns
- **Facade Pattern**: The core pattern used to provide simplified interfaces to complex platform SDKs
- **Interface Segregation**: Each service (user, leaderboards, multiplayer, etc.) has its own focused interface
- **Dependency Injection**: Interfaces should be designed to support DI patterns for testing and flexibility

### Interface Design
- All public interfaces should be in the `PlatformFacade` namespace
- Use clear, descriptive names that indicate the service's purpose
- Include comprehensive XML documentation for all public members
- Properties should be get-only when representing read-only data
- Use appropriate data types (e.g., `ulong` for user IDs, enums for status values)

## Code Style Guidelines

### C# Conventions
- Follow explicit C# naming conventions:
  - **PascalCase** for public members (methods, properties, classes, interfaces)
  - **_camelCase with underscore prefix** for private fields
  - **camelCase** for local variables and parameters
  - **ALL_CAPS** for constants
- All Unity inspector exposed serialized fields should be private
- Use explicit access modifiers
- Include XML documentation comments for all public APIs
- Use `var` only when the type is obvious from the right side of the assignment
- Prefer readonly fields and properties when data shouldn't change after initialization

### Unity-Specific Guidelines
- Design interfaces to be platform-agnostic
- Avoid Unity Package specific types in public interfaces when possible
- Consider async/await patterns for platform operations that may take time
- Use Railway Oriented Design for methods that may need to be chained
- Use Unity's serialization system appropriately for data structures

## Documentation Standards

### XML Documentation
- All public interfaces, properties, methods, and enums must have XML documentation
- Use `<summary>` for brief descriptions
- Use `<param>` for method parameters
- Use `<returns>` for method return values
- Reference related types using `<see cref="TypeName"/>`
- Provide usage examples in `<example>` tags for complex interfaces

### README Updates
- Keep the main README.md updated with:
  - Clear project description
  - Installation instructions
  - Basic usage examples
  - Supported platforms (when implementations are added)

## Interface Implementation Guidelines

### New Services
When adding new platform services:
1. Create a focused interface in `Runtime/Interfaces/`
2. Include comprehensive XML documentation
3. Consider async patterns for network operations
4. Design for testability and mocking
5. Keep interfaces minimal and focused on a single responsibility

### Service Properties
- Use appropriate data types for identifiers (ulong for user IDs, string for names)
- Implement status enums for state management
- Consider nullable types for optional data
- Use readonly properties for immutable data

## Testing Approach

- Design interfaces to be easily mockable
- Consider creating simple implementations for testing purposes
- Keep business logic separate from platform-specific implementations
- Write unit tests for any concrete implementations

## Common Tasks

### Adding a New Platform Service
1. Create a new interface file in `Runtime/Interfaces/`
2. Define the interface with appropriate methods and properties
3. Add comprehensive XML documentation
4. Consider creating supporting enums or data structures if needed
5. Update the main IPlatform interface if the service should be exposed there

### Modifying Existing Interfaces
1. Update XML documentation
2. Review all existing implementations (when they exist)
3. Add appropriate deprecation warnings if needed

## Package Management

- This is a Unity package defined by `package.json`
- Target Unity 2020.1+ as specified in package.json
- Keep the assembly definition file updated with appropriate references
- Avoid external dependencies when possible to maintain simplicity

### Assembly Organization
- The Editor implementation should not be referenced by the Runtime assembly and should have its own assembly
- Windows Standalone and Mac Standalone implementations should have their own separate assemblies
- Keep platform-specific implementations in separate assemblies from the core runtime interfaces

## Contribution Guidelines

- Make minimal, focused changes
- Maintain consistency with existing code style
- Add appropriate documentation for any new public APIs
- Consider the impact on package consumers
- Test changes thoroughly before committing

## Platform Considerations

This facade is designed to abstract differences between gaming platforms such as:
- Steam
- Epic Games Store  
- PlayStation Network
- Xbox Live
- Nintendo Switch Online
- Mobile platforms (iOS Game Center, Google Play Games)

Keep platform-specific details out of the interface definitions and focus on common functionality that exists across platforms.