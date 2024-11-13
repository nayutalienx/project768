using System;
using System.Collections.Generic;
using Godot;
using project768.scripts.common;
using project768.scripts.common.interaction;
using project768.scripts.game_entity.npc.jumping_enemy.interaction;
using project768.scripts.game_entity.npc.jumping_enemy.interaction.data;
using project768.scripts.game_entity.npc.jumping_enemy.state;
using project768.scripts.state_machine;

public partial class JumpingEnemy :
    CharacterBody2D,
    IStateMachineEntity<JumpingEnemy, JumpingEnemy.State>,
    IInteractableEntity<JumpingEnemy, JumpingEnemyInteractionContext, JumpingEnemyInteractionEvent,
        JumpingEnemyInteraction>
{
    public enum State
    {
        Idle,
        Triggered,
        Rewind
    }

    public State<JumpingEnemy, State> CurrentState { get; set; }
    public Dictionary<State, State<JumpingEnemy, State>> States { get; set; }
    public StateChanger<JumpingEnemy, State> StateChanger { get; set; }

    public Dictionary<JumpingEnemyInteraction,
        Interaction<JumpingEnemy, JumpingEnemyInteractionEvent, JumpingEnemyInteraction>> Interactions { get; set; }

    public Interactor<JumpingEnemy, JumpingEnemyInteractionContext, JumpingEnemyInteractionEvent,
        JumpingEnemyInteraction> Interactor { get; set; }

    public JumpingEnemyInteractionContext InteractionContext { get; set; }

    [ExportSubgroup("Enemy settings")]
    [Export] public float IdleFloorInterval = 1.0f;
    [Export] public float Friction = 10.0f;
    [Export] public int JumpsToRevertDirection = 3;
    
    public Area2D HeadArea { get; set; }
    public Area2D AttackArea { get; set; }
    public Label Label { get; set; }
    public Vector2 Direction { get; set; }
    
    public TimerManager IdleFloorTimerManager { get; set; }

    public Tuple<uint, uint> OriginalEntityLayerMask;
    public Tuple<uint, uint> OriginalHeadAreaLayerMask;
    public Tuple<uint, uint> OriginalAttackAreaLayerMask;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        States = new Dictionary<State, State<JumpingEnemy, State>>()
        {
            {State.Idle, new IdleState(this, State.Idle)},
            {State.Triggered, new TriggeredState(this, State.Triggered)},
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
        if (HasNode("direction"))
        {
            Direction = GetNode<RayCast2D>("direction").TargetPosition;
        }
        else
        {
            Direction = Vector2.Up;
        }

        HeadArea.BodyEntered += body => { CurrentState.OnBodyEntered(new CollisionBody("head", body)); };
        AttackArea.BodyEntered += body => { CurrentState.OnBodyEntered(new CollisionBody("attack", body)); };

        OriginalEntityLayerMask = this.GetCollisionLayerMask();
        OriginalAttackAreaLayerMask = AttackArea.GetCollisionLayerMask();
        OriginalHeadAreaLayerMask = HeadArea.GetCollisionLayerMask();

        IdleFloorTimerManager = new TimerManager(IdleFloorInterval);

        StateChanger.ChangeState(State.Idle);
    }

    public override void _PhysicsProcess(double delta)
    {
        CurrentState.PhysicProcess(delta);

        Label.Text = $"v: {Velocity}\n" +
                     $"d: {Direction}";
    }
}