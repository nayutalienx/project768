# Project768

A 2D platformer game built with Godot 4.3 featuring unique time rewind mechanics and spacetime manipulation.

## Overview

Project768 is an innovative 2D platformer that allows players to rewind time and manipulate the flow of gameplay. The game features multiple worlds, complex enemy AI, interactive objects, and a sophisticated save/load system. Players can reverse time to undo mistakes, solve puzzles, and explore temporal mechanics.

## Key Features

### 🕰️ Time Rewind System
- **Real-time rewind**: Hold Tab to rewind the game state
- **Variable speed**: Use Up/Down arrows during rewind to adjust speed
- **Audio synchronization**: Music plays forward and backward matching the time direction
- **State preservation**: All game objects maintain their historical states

### 🌌 Spacetime Mechanics
- **Directional gameplay**: Some elements behave differently when time flows backward
- **Spacetime entities**: Special enemies and objects with unique temporal behaviors
- **Dynamic audio**: Background music adapts to the current time flow direction

### 🎮 Gameplay Elements
- **Multiple worlds**: Explore different levels with unique challenges
- **Interactive objects**: Keys, locked doors, switches, boxes, and platforms
- **Enemy variety**: Regular enemies, spacetime enemies, and jumping enemies
- **Collectible system**: Items persist across game sessions
- **Environmental puzzles**: Use time manipulation to solve complex challenges

### 💾 Persistence System
- **Save/Load functionality**: Progress automatically saves
- **Cross-session persistence**: Collected items remain across play sessions
- **Scene-based saves**: Each level maintains independent progress

## Controls

| Action | Key | Description |
|--------|-----|-------------|
| Move | Arrow Keys | Move left/right, climb ladders |
| Jump | Space/Enter | Jump and interact |
| Rewind | Tab | Hold to rewind time |
| Speed Up | Up Arrow (during rewind) | Increase rewind speed |
| Slow Down | Down Arrow (during rewind) | Decrease rewind speed |
| Reload Scene | F5 | Restart current level |
| Quit | Escape | Exit the game |
| Interact | Space | Interact with objects |

## Technical Requirements

### Engine
- **Godot 4.3** or later
- **.NET 6.0** runtime
- **OpenGL Compatibility** renderer

### System Requirements
- **OS**: Windows, macOS, Linux
- **RAM**: 512 MB minimum
- **Storage**: 100 MB available space
- **Graphics**: OpenGL 3.3 compatible

## Building from Source

### Prerequisites
1. Install [Godot 4.3](https://godotengine.org/download) or later
2. Install [.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0) or later

### Build Steps
1. Clone the repository:
   ```bash
   git clone https://github.com/nayutalienx/project768.git
   cd project768
   ```

2. Open the project in Godot:
   - Launch Godot
   - Click "Import"
   - Navigate to the project folder
   - Select `project.godot`

3. Build the C# solution:
   - In Godot, go to Project → Tools → C# → Create C# solution
   - Or use command line:
     ```bash
     dotnet build project768.sln
     ```

4. Run the game:
   - Press F5 in Godot editor
   - Or export to your target platform

## Project Structure

```
project768/
├── scenes/           # Godot scene files
│   ├── level/       # Game levels and worlds
│   ├── ui/          # User interface scenes
│   ├── common/      # Shared scene components
│   └── npc/         # Non-player character scenes
├── scripts/         # C# source code
│   ├── common/      # Shared game systems
│   ├── game_entity/ # Entity-specific logic
│   └── rewind/      # Time rewind system
├── sprites/         # Game artwork and assets
├── music/           # Audio files
├── shaders/         # Custom shader programs
└── tile_sets/       # Tilemap resources
```

## Architecture

### Core Systems

#### Rewind System
- **RewindPlayer**: Main controller for time manipulation
- **SpacetimeRewindPlayer**: Enhanced rewind with spacetime mechanics
- **IRewindable Interface**: Contract for objects that can be rewound
- **RewindDataSource**: Manages all rewindable entities in the scene

#### State Management
- **State Machine Pattern**: Used for complex entity behaviors
- **Save System**: Handles game progress persistence
- **Scene Management**: Smooth transitions between levels

#### Audio System
- **RewindAudioPlayer**: Synchronizes music with time flow
- **SpacetimeAudioPlayer**: Directional audio for spacetime mechanics
- **Dynamic Playback**: Forward and backward audio playback

### Entity System
All game entities implement relevant interfaces:
- `IRewindable`: For time manipulation
- `IStateMachineEntity`: For complex behaviors
- `IPersistentEntity`: For save/load functionality
- `IInteractableEntity`: For player interactions

## Game Worlds

The game features multiple worlds, each with unique challenges:

1. **World 0**: Tutorial and basic mechanics introduction
2. **World 1**: Core platforming with rewind puzzles
3. **World 2**: Advanced spacetime mechanics
4. **World 3**: Complex multi-layered temporal challenges

## Development

### Adding New Rewindable Objects
1. Implement the `IRewindable` interface
2. Add the object to the appropriate group in scene
3. Register in `RewindDataSource.cs`

### Creating New Levels
1. Create scene in `scenes/level/game/`
2. Add spawn points for player
3. Configure rewind system components
4. Set up save/load persistence

## Contributing

1. Fork the repository
2. Create a feature branch
3. Follow the existing code style and architecture
4. Test thoroughly with the rewind system
5. Submit a pull request

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Credits

- **Engine**: Godot 4.3
- **Programming**: C# with .NET 6.0
- **Architecture**: State machine and entity-component patterns
- **Special Thanks**: To the Godot community for excellent documentation and support

---

*Project768 - Where time is just another dimension to explore.*