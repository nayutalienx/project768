using Godot;
using System;

public partial class Player : CharacterBody2D
{
    enum PlayerMode
    {
        GROUND,
        LADDER
    }

    [Export] public float JUMP_VELOCITY = -400.0f;

    [Export] public float SPEED = 300.0f;

    private PlayerMode playerMode = PlayerMode.GROUND;

    private bool isDead = false;

    private LadderEvent ladderEvent = new LadderEvent();

    public override void _Ready()
    {
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
            GetTree().ReloadCurrentScene();
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_down"))
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
        }

        MoveAndSlide();
    }

    private void toGroundMode()
    {
        GD.Print("Go to ground mode");
        playerMode = PlayerMode.GROUND;
    }

    private void toLadderMode()
    {
        GD.Print("Go to ladder mode");
        playerMode = PlayerMode.LADDER;
        Velocity = Vector2.Zero;
        Position = Position with {X = ladderEvent.Position.X};
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
}