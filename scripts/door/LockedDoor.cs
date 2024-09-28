using Godot;
using System;

public partial class LockedDoor : AnimatableBody2D
{
    private AnimationPlayer animationPlayer;
    private CollisionShape2D collisionShape2D;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        collisionShape2D = GetNode<CollisionShape2D>("CollisionShape2D");
        GetNode<Area2D>("lock_area").BodyEntered += OnBodyEntered;

        //animationPlayer.AnimationFinished += name => collisionShape2D.Disabled = true;
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is Player player && player.DoorKey != null)
        {
            animationPlayer.Play("DoorOpen");
            player.DoorKey.Hide();
            player.DoorKey = null;
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}