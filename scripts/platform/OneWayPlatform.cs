using Godot;
using System;

public partial class OneWayPlatform : Node
{
    [Export] public string animationName { get; set; }
    private AnimationPlayer animationPlayer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (HasNode("AnimationPlayer"))
        {
            animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
            if (animationPlayer != null && animationName != null)
            {
                animationPlayer.Play(animationName);
            }
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}