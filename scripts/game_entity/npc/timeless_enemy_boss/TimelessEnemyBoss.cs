using System;
using System.Collections.Generic;
using Godot;
using project768.scripts.common;
using project768.scripts.common.interaction;
using project768.scripts.game_entity.npc.timeless_enemy_boss.interaction;
using project768.scripts.game_entity.npc.timeless_enemy_boss.interaction.data;
using project768.scripts.game_entity.npc.timeless_enemy_boss.state;
using project768.scripts.state_machine;

namespace project768.scripts.game_entity.npc.timeless_enemy_boss;

public partial class TimelessEnemyBoss :
    CharacterBody2D,
    IStateMachineEntity<TimelessEnemyBoss, TimelessEnemyBoss.State>,
    IInteractableEntity<TimelessEnemyBoss, TimelessEnemyBossInteractionContext, TimelessEnemyBossInteractionEvent,
        TimelessEnemyBossInteraction>
{
    public enum State
    {
        Idle,
        Triggered,
        TriggeredToTarget,
        Death
    }

    public Dictionary<TimelessEnemyBossInteraction,
        Interaction<TimelessEnemyBoss, TimelessEnemyBossInteractionEvent, TimelessEnemyBossInteraction>> Interactions
    {
        get;
        set;
    }

    public Interactor<TimelessEnemyBoss, TimelessEnemyBossInteractionContext, TimelessEnemyBossInteractionEvent,
        TimelessEnemyBossInteraction> Interactor { get; set; }

    public TimelessEnemyBossInteractionContext InteractionContext { get; set; } = new();
    public State<TimelessEnemyBoss, State> CurrentState { get; set; }
    public Dictionary<State, State<TimelessEnemyBoss, State>> States { get; set; }
    public StateChanger<TimelessEnemyBoss, State> StateChanger { get; set; }


    [Export] public Line2D FieldLine { get; set; }
    [Export] public TimelessKey TimelessKey { get; set; }

    [ExportSubgroup("Enemy physics")] [Export]
    public float Friction = 10.0f;

    [Export] public float MoveSpeed = 150.0f;
    [Export] public Vector2 VelocityLimit = new Vector2(500.0f, 500.0f);

    [ExportSubgroup("Boss settings")] [Export(PropertyHint.Range, "1,5,")]
    public int LifeCount = 5;

    public Area2D HeadArea { get; set; }
    public Area2D AttackArea { get; set; }
    public Label Label { get; set; }
    public RaycastList VisionTarget { get; set; } = new();
    public Sprite2D Sprite { get; set; }
    public CollisionShape2D CollisionShape { get; set; }

    public Tuple<uint, uint> OriginalEntityLayerMask;
    public Tuple<uint, uint> OriginalHeadAreaLayerMask;
    public Tuple<uint, uint> OriginalAttackAreaLayerMask;

    public Vector2 InitialPosition { get; set; }
    public Vector2 TargetToMove { get; set; }
    public float EnemyRadius { get; set; }

    private Dictionary<int, string> LifeTableData = new Dictionary<int, string>()
    {
        {0, ""},
        {1, "*"},
        {2, "**"},
        {3, "***"},
        {4, "****"},
        {5, "*****"},
    };

    public override void _Ready()
    {
        InitialPosition = GlobalPosition;
        Interactions =
            new Dictionary<TimelessEnemyBossInteraction,
                Interaction<TimelessEnemyBoss, TimelessEnemyBossInteractionEvent, TimelessEnemyBossInteraction>>()
            {
                {TimelessEnemyBossInteraction.KillBoss, new KillEnemyInteraction(this)},
            };
        Interactor =
            new Interactor<TimelessEnemyBoss, TimelessEnemyBossInteractionContext, TimelessEnemyBossInteractionEvent,
                TimelessEnemyBossInteraction>(this);

        States = new Dictionary<State, State<TimelessEnemyBoss, State>>()
        {
            {State.Idle, new IdleState(this, State.Idle)},
            {State.Triggered, new TriggeredState(this, State.Triggered)},
            {State.TriggeredToTarget, new TriggeredToTargetState(this, State.TriggeredToTarget)},
            {State.Death, new DeathState(this, State.Death)},
        };
        StateChanger = new StateChanger<TimelessEnemyBoss, State>(this);

        VisionTarget.AddFromNode(GetNode<Node2D>("VisionsTarget"));
        HeadArea = GetNode<Area2D>("EnemyHeadArea");
        AttackArea = GetNode<Area2D>("EnemyAttackArea");
        Label = GetNode<Label>("Label");
        Sprite = GetNode<Sprite2D>("Sprite2D");

        CollisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
        CapsuleShape2D capsuleShape2D = CollisionShape.GetShape() as CapsuleShape2D;
        EnemyRadius = capsuleShape2D.Radius;

        HeadArea.BodyEntered += body => { CurrentState.OnBodyEntered(new CollisionBody("head", body)); };
        AttackArea.BodyEntered += body => { CurrentState.OnBodyEntered(new CollisionBody("attack", body)); };

        OriginalEntityLayerMask = this.GetCollisionLayerMask();
        OriginalAttackAreaLayerMask = AttackArea.GetCollisionLayerMask();
        OriginalHeadAreaLayerMask = HeadArea.GetCollisionLayerMask();

        StateChanger.ChangeState(State.Idle);
    }

    public override void _PhysicsProcess(double delta)
    {
        CurrentState.PhysicProcess(delta);

        Label.Text = $"{LifeTableData[LifeCount]}\n" +
                     $"s: {CurrentState.StateEnum}\n" +
                     $"t: {TargetToMove}";
    }
}