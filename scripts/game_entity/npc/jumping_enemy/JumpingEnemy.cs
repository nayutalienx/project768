using System;
using System.Collections.Generic;
using Godot;
using project768.scripts.common;
using project768.scripts.common.interaction;
using project768.scripts.game_entity.npc.jumping_enemy.interaction;
using project768.scripts.game_entity.npc.jumping_enemy.interaction.data;
using project768.scripts.game_entity.npc.jumping_enemy.state;
using project768.scripts.rewind.entity;
using project768.scripts.state_machine;

public partial class JumpingEnemy :
    CharacterBody2D,
    IRewindable,
    ISpawnable,
    IStateMachineEntity<JumpingEnemy, JumpingEnemy.State>,
    IInteractableEntity<JumpingEnemy, JumpingEnemyInteractionContext, JumpingEnemyInteractionEvent,
        JumpingEnemyInteraction>
{
    public enum State
    {
        Wait,
        Idle,
        Triggered,
        Death,
        Rewind
    }

    public int RewindState { get; set; }
    public State<JumpingEnemy, State> CurrentState { get; set; }
    public Dictionary<State, State<JumpingEnemy, State>> States { get; set; }
    public StateChanger<JumpingEnemy, State> StateChanger { get; set; }

    public Dictionary<JumpingEnemyInteraction,
        Interaction<JumpingEnemy, JumpingEnemyInteractionEvent, JumpingEnemyInteraction>> Interactions { get; set; }

    public Interactor<JumpingEnemy, JumpingEnemyInteractionContext, JumpingEnemyInteractionEvent,
        JumpingEnemyInteraction> Interactor { get; set; }

    public JumpingEnemyInteractionContext InteractionContext { get; set; }

    [ExportSubgroup("Enemy settings")] [Export]
    public float IdleFloorInterval = 1.0f;

    [Export] public float JumpAttackCooldown = 1.5f;
    [Export] public float Friction = 10.0f;
    [Export] public int JumpsToRevertDirection = 3;
    [Export] public float TriggeredDirectionScale = 1.5f;
    [Export] public float JumpAttackDirectionScale = 3.5f;
    [Export] public float DeathTimeShouldPassBeforeWait = 0.5f;
    [Export] public Vector2 SpawnVelocity { get; set; } = Vector2.Zero;
    [Export] public bool AliveOnStart = false;

    public Area2D HeadArea { get; set; }
    public Area2D AttackArea { get; set; }
    public Label Label { get; set; }
    public Vector2 Direction { get; set; }
    public Vector2 InitialPosition { get; set; }
    public RaycastList VisionTarget { get; set; } = new();
    public NavigationAgent2D NavigationAgent2D { get; set; }

    public TimerManager DeathStopTimer { get; set; }

    public float JumpAttackDistance { get; set; }

    public Node2D TriggerPoint { get; set; }

    public TimerManager IdleFloorTimerManager { get; set; }
    public TimerManager JumpAttackTimerManager { get; set; }

    public Tuple<uint, uint> OriginalEntityLayerMask;
    public Tuple<uint, uint> OriginalHeadAreaLayerMask;
    public Tuple<uint, uint> OriginalAttackAreaLayerMask;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        InitialPosition = GlobalPosition;

        States = new Dictionary<State, State<JumpingEnemy, State>>()
        {
            {State.Wait, new WaitState(this, State.Wait)},
            {State.Idle, new IdleState(this, State.Idle)},
            {State.Triggered, new TriggeredState(this, State.Triggered)},
            {State.Death, new DeathState(this, State.Death)},
            {State.Rewind, new RewindState(this, State.Rewind)}
        };
        StateChanger = new StateChanger<JumpingEnemy, State>(this);

        Interactions = new Dictionary<JumpingEnemyInteraction,
            Interaction<JumpingEnemy, JumpingEnemyInteractionEvent, JumpingEnemyInteraction>>()
        {
            {JumpingEnemyInteraction.KillEnemy, new KillEnemyInteraction(this)}
        };
        Interactor =
            new Interactor<JumpingEnemy, JumpingEnemyInteractionContext, JumpingEnemyInteractionEvent,
                JumpingEnemyInteraction>(this);

        HeadArea = GetNode<Area2D>("EnemyHeadArea");
        AttackArea = GetNode<Area2D>("EnemyAttackArea");
        Label = GetNode<Label>("Label");

        VisionTarget.AddFromNode(GetNode<Node2D>("VisionsTarget"));
        NavigationAgent2D = GetNode<NavigationAgent2D>("NavigationAgent2D");

        DeathStopTimer = new TimerManager(DeathTimeShouldPassBeforeWait);

        if (HasNode("direction"))
        {
            Direction = GetNode<RayCast2D>("direction").TargetPosition;
        }
        else
        {
            Direction = Vector2.Up;
        }

        if (HasNode("jump_attack"))
        {
            JumpAttackDistance = GetNode<RayCast2D>("jump_attack").TargetPosition.Length();
        }

        HeadArea.BodyEntered += body => { CurrentState.OnBodyEntered(new CollisionBody("head", body)); };
        AttackArea.BodyEntered += body => { CurrentState.OnBodyEntered(new CollisionBody("attack", body)); };

        OriginalEntityLayerMask = this.GetCollisionLayerMask();
        OriginalAttackAreaLayerMask = AttackArea.GetCollisionLayerMask();
        OriginalHeadAreaLayerMask = HeadArea.GetCollisionLayerMask();

        IdleFloorTimerManager = new TimerManager(IdleFloorInterval);
        JumpAttackTimerManager = new TimerManager(JumpAttackCooldown);

        if (AliveOnStart)
        {
            StateChanger.ChangeState(State.Idle);
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
                     $"d: {Direction}\n" +
                     $"jc: {JumpAttackTimerManager.CurrentTime}\n" +
                     $"s: {CurrentState.StateEnum}";
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
        return CurrentState.StateEnum == State.Wait;
    }

    public bool TrySpawn(Vector2 spawnPoint, Vector2 direction)
    {
        if (CanSpawn())
        {
            GlobalPosition = spawnPoint;
            Velocity = SpawnVelocity;
            StateChanger.ChangeState(State.Idle);
            return true;
        }

        return false;
    }
}