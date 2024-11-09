using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using Godot;
using project768.scripts.common;
using project768.scripts.common.interaction;
using project768.scripts.enemy;
using project768.scripts.game_entity.npc.enemy.interaction;
using project768.scripts.game_entity.npc.enemy.interaction.data;
using project768.scripts.game_entity.npc.spawner;
using project768.scripts.player;
using project768.scripts.player.interaction;
using project768.scripts.rewind.entity;
using project768.scripts.state_machine;
using DeathState = project768.scripts.enemy.DeathState;
using MoveState = project768.scripts.enemy.MoveState;
using RewindState = project768.scripts.enemy.RewindState;
using TryPickupKeyInteraction = project768.scripts.game_entity.npc.enemy.interaction.TryPickupKeyInteraction;
using TryPickupTimelessKeyInteraction = project768.scripts.game_entity.npc.enemy.interaction.TryPickupTimelessKeyInteraction;

public partial class Enemy :
    CharacterBody2D,
    IRewindable,
    ISpawnable,
    IStateMachineEntity<Enemy, Enemy.State>,
    IInteractableEntity<Enemy, EnemyInteractionContext, EnemyInteractionEvent, EnemyInteraction>
{
    public enum State
    {
        Move,
        Death,
        Rewind
    }

    public Dictionary<EnemyInteraction, Interaction<Enemy, EnemyInteractionEvent, EnemyInteraction>> Interactions
    {
        get;
        set;
    }

    public Interactor<Enemy, EnemyInteractionContext, EnemyInteractionEvent, EnemyInteraction> Interactor { get; set; }
    public EnemyInteractionContext InteractionContext { get; set; } = new();
    public int RewindState { get; set; }
    public State<Enemy, State> CurrentState { get; set; }
    public Dictionary<State, State<Enemy, State>> States { get; set; }
    public StateChanger<Enemy, State> StateChanger { get; set; }


    [ExportSubgroup("Enemy physics")]
    [Export]
    public Vector2 SpawnVelocity { get; set; } = Vector2.Zero;

    [Export] public float Friction = 10.0f;
    [Export] public float MoveSpeed = 150.0f;
    [Export] public float PushForce = 80.0f;
    [Export] public Vector2 VelocityLimit = new Vector2(500.0f, 500.0f);


    [ExportSubgroup("Enemy settings")] [Export]
    public bool AliveOnStart = false;
    
    public int EnemyDirection { get; set; } = 1;
    public Area2D HeadArea { get; set; }
    public Area2D AttackArea { get; set; }
    public Label Label { get; set; }

    public Tuple<uint, uint> OriginalEntityLayerMask;
    public Tuple<uint, uint> OriginalHeadAreaLayerMask;
    public Tuple<uint, uint> OriginalAttackAreaLayerMask;

    public Vector2 InitialPosition { get; set; }

    public override void _Ready()
    {
        InitialPosition = GlobalPosition;
        Interactions = new Dictionary<EnemyInteraction, Interaction<Enemy, EnemyInteractionEvent, EnemyInteraction>>()
        {
            {EnemyInteraction.TryPickupKey, new TryPickupKeyInteraction(this)},
            {EnemyInteraction.KillEnemy, new KillEnemyInteraction(this)},
            {EnemyInteraction.TryPickupTimelessKey, new TryPickupTimelessKeyInteraction(this)},
        };
        Interactor = new Interactor<Enemy, EnemyInteractionContext, EnemyInteractionEvent, EnemyInteraction>(this);

        States = new Dictionary<State, State<Enemy, State>>()
        {
            {State.Move, new MoveState(this, State.Move)},
            {State.Death, new DeathState(this, State.Death)},
            {State.Rewind, new RewindState(this, State.Rewind)},
        };
        StateChanger = new StateChanger<Enemy, State>(this);
        
        HeadArea = GetNode<Area2D>("EnemyHeadArea");
        AttackArea = GetNode<Area2D>("EnemyAttackArea");
        Label = GetNode<Label>("Label");

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
            StateChanger.ChangeState(State.Death);
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

    public void RewindStarted()
    {
        StateChanger.ChangeState(State.Rewind);
    }

    public void RewindFinished()
    {
        StateChanger.ChangeState((State) RewindState);
    }

    public void OnRewindSpeedChanged(int speed)
    {
    }

    public bool CanSpawn()
    {
        return CurrentState.StateEnum == State.Death;
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