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

    [Export] public float JUMP_VELOCITY = -400.0f;

    [Export] public float SPEED = 300.0f;

    [Export] public float PUSH_FORCE = 80.0f;

    private PlayerMode playerMode = PlayerMode.GROUND;

    private bool isDead = false;

    private LadderEvent ladderEvent = new LadderEvent();

    private Node2D directionNode;

    private Sprite2D key;

    public override void _Ready()
    {
        directionNode = GetNode<Node2D>("direction");

        var enemies = GetTree().GetNodesInGroup("enemy");

        foreach (var enemy in enemies)
        {
            ((Enemy) enemy).EnemyInteract += OnEnemyReact;
        }

        var ladders = GetTree().GetNodesInGroup("ladder");

        foreach (var ladder in ladders)
        {
            ((Ladder) ladder).LadderInteract += OnLadderReact;
            ;
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (isDead)
        {
            var world = GD.Load<PackedScene>("res://scenes/world.tscn");
            var root = GetTree().GetRoot();
            foreach (var child in root.GetChildren())
            {
                root.RemoveChild(child);
            }
            root.AddChild(world.Instantiate<Node2D>());
        }
    }

    public override void _Input(InputEvent _event)
    {
        // snap off one-way-collission
        if (_event.IsActionPressed("ui_down"))
        {
            Position = Position with {Y = Position.Y + 1};
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
                    -c.GetNormal() * PUSH_FORCE);
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
            toGroundMode();
            Velocity = Vector2.Zero;
            return;
        }

        float direction = Input.GetAxis("ui_up", "ui_down");

        if (direction == 0)
        {
            Velocity = Velocity.MoveToward(
                Velocity with {Y = 0}, SPEED
            );
        }
        else
        {
            Velocity = Velocity with {Y = direction * SPEED};
        }

        Position = Position with {X = ladderEvent.Position.X};

        MoveAndSlide();
    }

    public void MoveOnGround(double delta)
    {
        if ((
                ladderEvent.NearLadder &&
                (Input.IsActionJustPressed("ui_down") || Input.IsActionJustPressed("ui_up"))
            )
           )
        {
            toLadderMode();
            return;
        }

        if (!IsOnFloor())
        {
            Velocity += GetGravity() * (float) delta;
        }

        if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
        {
            Velocity = Velocity with {Y = JUMP_VELOCITY};
        }

        float direction = Input.GetAxis("ui_left", "ui_right");

        if (direction == 0)
        {
            Velocity = Velocity.MoveToward(
                Velocity with {X = 0}, SPEED
            );
        }
        else
        {
            Velocity = Velocity with {X = direction * SPEED};
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

    private void toGroundMode()
    {
        GD.Print("Go to ground mode");
        playerMode = PlayerMode.GROUND;
        key?.Show();
    }

    private void toLadderMode()
    {
        GD.Print("Go to ladder mode");
        playerMode = PlayerMode.LADDER;
        Velocity = Vector2.Zero;
        key?.Hide();
    }

    public void OnEnemyReact(EnemyEvent enemyEvent)
    {
        if (enemyEvent.DiedFromHeadJump)
        {
            Velocity = Velocity with {Y = JUMP_VELOCITY};
        }

        if (enemyEvent.KillPlayer)
        {
            isDead = true;
        }
    }

    public void OnLadderReact(LadderEvent ladderEvent)
    {
        GD.Print($"near ladder {ladderEvent.NearLadder}");
        this.ladderEvent = ladderEvent;
        if (!ladderEvent.NearLadder)
        {
            toGroundMode();
        }
    }

    public bool TryToPick(ItemEnum itemEnum)
    {
        if (itemEnum == ItemEnum.Key && key == null)
        {
            key = GetNode<Sprite2D>("direction/key");
            key.Show();
            return true;
        }

        return false;
    }
}