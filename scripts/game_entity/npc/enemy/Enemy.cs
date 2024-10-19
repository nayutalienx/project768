using System;
using System.Reflection.Metadata;
using Godot;
using project768.scripts.common;
using project768.scripts.common.interaction;
using project768.scripts.game_entity.npc.enemy.interaction;
using project768.scripts.game_entity.npc.enemy.interaction.data;
using project768.scripts.player;
using project768.scripts.player.interaction;
using project768.scripts.rewind.entity;
using project768.scripts.state_machine;
using DeathState = project768.scripts.enemy.DeathState;
using MoveState = project768.scripts.enemy.MoveState;
using RewindState = project768.scripts.enemy.RewindState;
using TryPickupKeyInteraction = project768.scripts.game_entity.npc.enemy.interaction.TryPickupKeyInteraction;

public partial class Enemy :
    CharacterBody2D,
    IRewindable,
    IStateMachineEntity<Enemy, Enemy.State>,
    IInteractableEntity<Enemy, EnemyInteractionContext, EnemyInteractionEvent, EnemyInteraction>
{
    public enum State
    {
        Move,
        Death,
        Rewind
    }

    public Interaction<Enemy, EnemyInteractionEvent, EnemyInteraction>[] Interactions { get; set; }
    public Interactor<Enemy, EnemyInteractionContext, EnemyInteractionEvent, EnemyInteraction> Interactor { get; set; }
    public EnemyInteractionContext InteractionContext { get; set; } = new();
    public int RewindState { get; set; }
    public State<Enemy, State> CurrentState { get; set; }
    public State<Enemy, State>[] States { get; set; }
    public StateChanger<Enemy, State> StateChanger { get; set; }

    [Export] public float MoveSpeed = 150.0f;

    [Export] public float PushForce = 80.0f;
    public RayCast2D FallRaycastLeft { get; set; }
    public RayCast2D FallRaycastRight { get; set; }
    public int EnemyDirection { get; set; } = 1;

    public Area2D HeadArea { get; set; }
    public Area2D AttackArea { get; set; }

    public Tuple<uint, uint> OriginalEntityLayerMask;
    public Tuple<uint, uint> OriginalHeadAreaLayerMask;
    public Tuple<uint, uint> OriginalAttackAreaLayerMask;

    public override void _Ready()
    {
        Interactions = new Interaction<Enemy, EnemyInteractionEvent, EnemyInteraction>[]
        {
            new TryPickupKeyInteraction(this),
            new KillEnemyInteraction(this)
        };
        Interactor = new Interactor<Enemy, EnemyInteractionContext, EnemyInteractionEvent, EnemyInteraction>(this);

        States = new State<Enemy, State>[]
        {
            new MoveState(this, State.Move),
            new DeathState(this, State.Death),
            new RewindState(this, State.Rewind),
        };
        StateChanger = new StateChanger<Enemy, State>(this);

        FallRaycastLeft = GetNode<RayCast2D>("FallRaycast_1");
        FallRaycastRight = GetNode<RayCast2D>("FallRaycast_2");
        HeadArea = GetNode<Area2D>("EnemyHeadArea");
        AttackArea = GetNode<Area2D>("EnemyAttackArea");

        HeadArea.BodyEntered += body => { CurrentState.OnBodyEntered(new CollisionBody("head", body)); };
        AttackArea.BodyEntered += body => { CurrentState.OnBodyEntered(new CollisionBody("attack", body)); };

        OriginalEntityLayerMask = this.GetCollisionLayerMask();
        OriginalAttackAreaLayerMask = AttackArea.GetCollisionLayerMask();
        OriginalHeadAreaLayerMask = HeadArea.GetCollisionLayerMask();

        StateChanger.ChangeState(State.Move);
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

    public void OnRewindSpeedChanged(int speed)
    {
    }
}