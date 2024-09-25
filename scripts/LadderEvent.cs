
using Godot;
using System;

public partial class LadderEvent : GodotObject
{

    public bool NearLadder { get; set; }
    public Vector2 Position { get; set; }
    public StaticBody2D LadderTop { get; set; }

}