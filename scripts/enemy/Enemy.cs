using Godot;
using project768.scripts.item;

public partial class Enemy : CharacterBody2D, ItemPicker
{
    [Signal]
    public delegate void EnemyInteractEventHandler(
        EnemyEvent ladderEvent
    );

    public float JUMP_VELOCITY = -400.0f;

    [Export] public float SPEED = 300.0f;

    [Export] public float PUSH_FORCE = 80.0f;

    private RayCast2D fallRaycastLeft;
    private RayCast2D fallRaycastRight;

    private bool isDead;
    private int enemyDirection = 1;
    private bool lockDirection = false;
    private Timer lockDirectionTimer;

    private Node2D directionNode;

    private Sprite2D key;

    public override void _Ready()
    {
        directionNode = GetNode<Node2D>("direction");

        fallRaycastLeft = GetNode<RayCast2D>("FallRaycast_1");
        fallRaycastRight = GetNode<RayCast2D>("FallRaycast_2");

        GetNode<Area2D>("EnemyHeadArea").BodyEntered += EnemyHeadBodyEntered;
        GetNode<Area2D>("EnemyAttackArea").BodyEntered += EnemyAttackBodyEntered;

        lockDirectionTimer = GetNode<Timer>("DirectionTimer");
        lockDirectionTimer.Timeout += OnTimerFinish;
    }

    public void EnemyAttackBodyEntered(Node2D body)
    {
        EmitSignal(
            SignalName.EnemyInteract,
            new EnemyEvent
            {
                KillPlayer = true
            });
    }

    public void EnemyHeadBodyEntered(Node2D body)
    {
        EmitSignal(
            SignalName.EnemyInteract,
            new EnemyEvent
            {
                DiedFromHeadJump = true
            });

        isDead = true;
    }

    public override void _Process(double delta)
    {
        if (isDead)
        {
            if (key != null && key.IsVisible())
            {
                var newKeyScene = GD.Load<PackedScene>("res://scenes/key_rb.tscn");
                var rb = newKeyScene.Instantiate<RigidBody2D>();
                rb.Position = Position;
                GD.Print($"Instantiated key scene");
                GetTree().GetRoot().AddChild(rb);
            }
            QueueFree();
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (isDead)
        {
            return;
        }

        if (!IsOnFloor())
        {
            Velocity += GetGravity() * (float) delta;
        }

        if (
            !fallRaycastLeft.IsColliding() ||
            !fallRaycastRight.IsColliding()
            || IsOnWall()
        )
        {
            if (IsOnFloor() && !lockDirection)
            {
                enemyDirection *= -1;
                directionNode.Scale = directionNode.Scale with {X = enemyDirection};
                lockDirection = true;
                lockDirectionTimer.Start();
            }
        }

        Velocity = Velocity with {X = enemyDirection * SPEED};

        MoveAndSlide();

        // enemy impulse to rigid bodies
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

    private void OnTimerFinish()
    {
        lockDirection = false;
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