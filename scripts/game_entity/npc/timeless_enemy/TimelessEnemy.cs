using System;
using System.Collections.Generic;
using Godot;
using project768.scripts.common;
using project768.scripts.common.interaction;
using project768.scripts.game_entity.npc.timeless_enemy.interaction;
using project768.scripts.game_entity.npc.timeless_enemy.interaction.data;
using project768.scripts.game_entity.npc.timeless_enemy.state;
using project768.scripts.state_machine;

public partial class TimelessEnemy :
    CharacterBody2D,
    ISpawnable,
    IStateMachineEntity<TimelessEnemy, TimelessEnemy.State>,
    IInteractableEntity<TimelessEnemy, TimelessEnemyInteractionContext, TimelessEnemyInteractionEvent,
        TimelessEnemyInteraction>
{
    public enum State
    {
        Move,
        Wait,
        Death
    }

    public Dictionary<TimelessEnemyInteraction,
        Interaction<TimelessEnemy, TimelessEnemyInteractionEvent, TimelessEnemyInteraction>> Interactions { get; set; }

    public Interactor<TimelessEnemy, TimelessEnemyInteractionContext, TimelessEnemyInteractionEvent,
        TimelessEnemyInteraction> Interactor { get; set; }

    public TimelessEnemyInteractionContext InteractionContext { get; set; } = new();
    public State<TimelessEnemy, State> CurrentState { get; set; }
    public Dictionary<State, State<TimelessEnemy, State>> States { get; set; }
    public StateChanger<TimelessEnemy, State> StateChanger { get; set; }


    [ExportSubgroup("Enemy physics")]
    [Export]
    public Vector2 SpawnVelocity { get; set; } = Vector2.Zero;

    [Export] public float Friction = 10.0f;
    [Export] public float MoveSpeed = 150.0f;
    [Export] public float PushForce = 80.0f;
    [Export] public Vector2 VelocityLimit = new Vector2(500.0f, 500.0f);


    [ExportSubgroup("Enemy settings")] [Export]
    public bool AliveOnStart = false;

    [Export] public float DeathTimeShouldPassBeforeWait = 0.5f;


    [Export] public int EnemyDirection { get; set; } = 1;
    public Area2D HeadArea { get; set; }
    public Area2D AttackArea { get; set; }
    public Label Label { get; set; }

    public Tuple<uint, uint> OriginalEntityLayerMask;
    public Tuple<uint, uint> OriginalHeadAreaLayerMask;
    public Tuple<uint, uint> OriginalAttackAreaLayerMask;

    public Vector2 InitialPosition { get; set; }
    public TimerManager DeathStopTimer { get; set; }

    public override void _Ready()
    {
        InitialPosition = GlobalPosition;
        Interactions =
            new Dictionary<TimelessEnemyInteraction,
                Interaction<TimelessEnemy, TimelessEnemyInteractionEvent, TimelessEnemyInteraction>>()
            {
                {TimelessEnemyInteraction.TryPickupKey, new TryPickupKeyInteraction(this)},
                {TimelessEnemyInteraction.KillEnemy, new KillEnemyInteraction(this)},
                {TimelessEnemyInteraction.TryPickupTimelessKey, new TryPickupTimelessKeyInteraction(this)},
            };
        Interactor =
            new Interactor<TimelessEnemy, TimelessEnemyInteractionContext, TimelessEnemyInteractionEvent,
                TimelessEnemyInteraction>(this);

        States = new Dictionary<State, State<TimelessEnemy, State>>()
        {
            {State.Move, new MoveState(this, State.Move)},
            {State.Wait, new WaitState(this, State.Wait)},
            {State.Death, new DeathState(this, State.Death)},
        };
        StateChanger = new StateChanger<TimelessEnemy, State>(this);

        HeadArea = GetNode<Area2D>("EnemyHeadArea");
        AttackArea = GetNode<Area2D>("EnemyAttackArea");
        Label = GetNode<Label>("Label");
        DeathStopTimer = new TimerManager(DeathTimeShouldPassBeforeWait);

        HeadArea.BodyEntered += body => { CurrentState.OnBodyEntered(new CollisionBody("head", body)); };
        AttackArea.BodyEntered += body => { CurrentState.OnBodyEntered(new CollisionBody("attack", body)); };

        OriginalEntityLayerMask = this.GetCollisionLayerMask();
        OriginalAttackAreaLayerMask = AttackArea.GetCollisionLayerMask();
        OriginalHeadAreaLayerMask = HeadArea.GetCollisionLayerMask();

        if (AliveOnStart)
        {
            StateChanger.ChangeState(State.Move);
        }
        else
        {
            StateChanger.ChangeState(State.Wait);
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        CurrentState.PhysicProcess(delta);

        Label.Text = $"v: {Velocity}\n" +
                     $"d: {EnemyDirection}";
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

    public bool CanSpawn()
    {
        return CurrentState.StateEnum == State.Wait;
    }

    public bool TrySpawn(Vector2 spawnPoint, Vector2 direction)
    {
        if (CanSpawn())
        {
            GlobalPosition = spawnPoint;
            EnemyDirection = (int) Math.Clamp(direction.X, -1.0f, 1.0f);
            Velocity = SpawnVelocity;
            StateChanger.ChangeState(State.Move);
            return true;
        }

        return false;
    }
}