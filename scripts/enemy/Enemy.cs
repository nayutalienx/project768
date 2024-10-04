using Godot;
using project768.scripts.item;
using project768.scripts.player;
using project768.scripts.rewind.entity;
using project768.scripts.state_machine;
using DeathState = project768.scripts.enemy.DeathState;
using MoveState = project768.scripts.enemy.MoveState;
using RewindState = project768.scripts.enemy.RewindState;

public partial class Enemy :
    CharacterBody2D,
    DoorKeyPicker,
    Rewindable,
    IStateMachineEntity<Enemy, Enemy.State>
{
    public enum State
    {
        Move,
        Death,
        Rewind
    }

    public int RewindState { get; set; }
    public State<Enemy, State> CurrentState { get; set; }
    public State<Enemy, State>[] States { get; set; }
    public StateChanger<Enemy, State> StateChanger { get; set; }
    public DoorKeyPickerContext DoorKeyPickerContext { get; set; } = new();

    [Export] public float MoveSpeed = 150.0f;

    [Export] public float PushForce = 80.0f;
    public RayCast2D FallRaycastLeft { get; set; }
    public RayCast2D FallRaycastRight { get; set; }
    public int EnemyDirection { get; set; } = 1;

    public Area2D HeadArea { get; set; }
    public Area2D AttackArea { get; set; }

    public override void _Ready()
    {
        States = new State<Enemy, State>[]
        {
            new MoveState(this, State.Move),
            new DeathState(this, State.Death),
            new RewindState(this, State.Rewind),
        };
        StateChanger = new StateChanger<Enemy, State>(this);
        StateChanger.ChangeState(State.Move);


        FallRaycastLeft = GetNode<RayCast2D>("FallRaycast_1");
        FallRaycastRight = GetNode<RayCast2D>("FallRaycast_2");
        HeadArea = GetNode<Area2D>("EnemyHeadArea");
        AttackArea = GetNode<Area2D>("EnemyAttackArea");

        HeadArea.BodyEntered += EnemyHeadBodyEntered;
        AttackArea.BodyEntered += EnemyAttackBodyEntered;
    }

    public void EnemyAttackBodyEntered(Node2D body)
    {
        if (body is Player player)
        {
            player.EnteredEnemyAttackArea();
        }
    }

    public void EnemyHeadBodyEntered(Node2D body)
    {
        if (body is Player player)
        {
            player.EnteredEnemyHeadArea();
            StateChanger.ChangeState(State.Death);
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        CurrentState.PhysicProcess(delta);
    }

    private void ApplyImpulseToRigidBodies()
    {
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

    public void RewindStarted()
    {
        StateChanger.ChangeState(State.Rewind);
    }

    public void RewindFinished()
    {
        StateChanger.ChangeState((State) RewindState);
    }

    public void EnteredSpikeArea()
    {
        StateChanger.ChangeState(State.Death);
    }
}