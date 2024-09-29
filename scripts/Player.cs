using System.Collections.Generic;
using Godot;
using project768.scripts.item;

public partial class Player : CharacterBody2D, ItemPicker
{
    enum PlayerMode
    {
        GROUND,
        LADDER
    }

    [Export] public float JumpVelocity = -400.0f;

    [Export] public float MoveSpeed = 300.0f;

    [Export] public float PushForce = 80.0f;

    private PlayerMode playerMode = PlayerMode.GROUND;

    public bool IsDead { get; set; }

    public Vector2 Ladder { get; set; }

    private Node2D directionNode;

    public Sprite2D DoorKey { get; set; }

    public override void _Ready()
    {
        directionNode = GetNode<Node2D>("direction");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (IsDead)
        {
            reloadFullScene();
        }
    }

    private void reloadFullScene()
    {
        var world = GD.Load<PackedScene>("res://scenes/world.tscn");
        var root = GetTree().GetRoot();
        foreach (var child in root.GetChildren())
        {
            root.RemoveChild(child);
        }

        root.AddChild(world.Instantiate<Node2D>());
    }

    public override void _Input(InputEvent _event)
    {
        // snap off one-way-collission
        if (_event.IsActionPressed("ui_down"))
        {
            Position = Position with {Y = Position.Y + 1};
        }

        if (_event.IsActionPressed("reload"))
        {
            reloadFullScene();
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        switch (playerMode)
        {
            case PlayerMode.GROUND:
                MoveOnGround(delta);
                break;
            case PlayerMode.LADDER:
                MoveOnLadder(delta);
                break;
        }

        // Player impulse to rigid bodies
        for (int i = 0; i < GetSlideCollisionCount(); i++)
        {
            KinematicCollision2D c = GetSlideCollision(i);
            if (c.GetCollider() is RigidBody2D)
            {
                ((RigidBody2D) c.GetCollider()).ApplyCentralImpulse(
                    -c.GetNormal() * PushForce);
            }
        }
    }

    private void MoveOnLadder(double delta)
    {
        if (
            Input.IsActionJustPressed("ui_left") ||
            Input.IsActionJustPressed("ui_right") ||
            Input.IsActionJustPressed("ui_accept")
        )
        {
            ToGroundMode();
            Velocity = Vector2.Zero;
            return;
        }

        float direction = Input.GetAxis("ui_up", "ui_down");

        if (direction == 0)
        {
            Velocity = Velocity.MoveToward(
                Velocity with {Y = 0}, MoveSpeed
            );
        }
        else
        {
            Velocity = Velocity with {Y = direction * MoveSpeed};
        }

        Position = Position with {X = Ladder.X};

        MoveAndSlide();
    }

    public void MoveOnGround(double delta)
    {
        if ((
                (Input.IsActionJustPressed("ui_down") ||
                 Input.IsActionJustPressed("ui_up"))
                && Ladder != Vector2.Zero
            )
           )
        {
            ToLadderMode();
            return;
        }

        if (!IsOnFloor())
        {
            Velocity += GetGravity() * (float) delta;
        }

        if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
        {
            Velocity = Velocity with {Y = JumpVelocity};
        }

        float direction = Input.GetAxis("ui_left", "ui_right");

        if (direction == 0)
        {
            Velocity = Velocity.MoveToward(
                Velocity with {X = 0}, MoveSpeed
            );
        }
        else
        {
            Velocity = Velocity with {X = direction * MoveSpeed};
            if (direction > 0)
            {
                directionNode.Scale = directionNode.Scale with {X = 1};
            }
            else
            {
                directionNode.Scale = directionNode.Scale with {X = -1};
            }
        }

        MoveAndSlide();
    }

    public void ToGroundMode()
    {
        GD.Print("Go to ground mode");
        playerMode = PlayerMode.GROUND;
        DoorKey?.Show();
    }

    public void ToLadderMode()
    {
        GD.Print("Go to ladder mode");
        playerMode = PlayerMode.LADDER;
        Velocity = Vector2.Zero;
        DoorKey?.Hide();
    }

    public bool TryToPick(ItemEnum itemEnum)
    {
        if (itemEnum == ItemEnum.Key && DoorKey == null)
        {
            DoorKey = GetNode<Sprite2D>("direction/key");
            DoorKey.Show();
            return true;
        }

        return false;
    }
}