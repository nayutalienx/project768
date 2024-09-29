using Godot;
using project768.scripts.item;

public partial class Enemy : CharacterBody2D, ItemPicker
{
    [Export] public float MoveSpeed = 150.0f;

    [Export] public float PushForce = 80.0f;

    private RayCast2D fallRaycastLeft;
    private RayCast2D fallRaycastRight;

    public bool IsDead { get; set; }
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
        if (body is Player player)
        {
            player.IsDead = true;
        }
    }

    public void EnemyHeadBodyEntered(Node2D body)
    {
        if (body is Player player)
        {
            player.Velocity = player.Velocity with {Y = player.JumpVelocity};
            IsDead = true;
        }
    }

    public override void _Process(double delta)
    {
        if (IsDead)
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
        if (IsDead)
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

        Velocity = Velocity with {X = enemyDirection * MoveSpeed};

        MoveAndSlide();

        // enemy impulse to rigid bodies
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