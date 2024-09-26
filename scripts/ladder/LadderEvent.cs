
using Godot;
using System;

public partial class LadderEvent : GodotObject
{
    public enum LadderSnap
    {
        TOP_SNAP,
        BOTTOM_SNAP
    }

    public bool NearLadder { get; set; }
    public Vector2 Position { get; set; }
    public LadderSnap Snap { get; set; }

}