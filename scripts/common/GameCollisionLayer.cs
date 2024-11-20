using System;

namespace project768.scripts.common;

[Flags]
public enum GameCollisionLayer
{
    Platform = 1 << 0,
    Player = 1 << 1,
    Enemies = 1 << 2,
    Key = 1 << 3,
    Switcher = 1 << 4,
    SceneLoader = 1 << 5,
    CollectableItem = 1 << 6,
    InvisibleWall = 1 << 7,
    Ladder = 1 << 8,
    LadderGrid = 1 << 9,
}