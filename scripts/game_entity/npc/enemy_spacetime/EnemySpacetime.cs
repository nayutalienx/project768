using System;
using System.Collections.Generic;
using Godot;
using project768.scripts.common;
using project768.scripts.common.interaction;
using project768.scripts.game_entity.npc.enemy_spacetime.interaction;
using project768.scripts.game_entity.npc.enemy_spacetime.interaction.data;
using project768.scripts.game_entity.npc.enemy_spacetime.state;
using project768.scripts.player;
using project768.scripts.rewind.entity;
using project768.scripts.state_machine;
using DeathState = project768.scripts.game_entity.npc.enemy_spacetime.state.DeathState;
using MoveState = project768.scripts.game_entity.npc.enemy_spacetime.state.MoveState;
using RewindState = project768.scripts.game_entity.npc.enemy_spacetime.state.RewindState;

public partial class EnemySpacetime :
    CharacterBody2D,
    IRewindable,
    IStateMachineEntity<EnemySpacetime, EnemySpacetime.State>,
    IInteractableEntity<EnemySpacetime, EnemySpacetimeInteractionContext, EnemySpacetimeInteractionEvent,
        EnemySpacetimeInteraction>
{
    public enum State
    {
        Move,
        Death,
        Wait,
        Rewind
    }

    public Dictionary<EnemySpacetimeInteraction,
        Interaction<EnemySpacetime, EnemySpacetimeInteractionEvent, EnemySpacetimeInteraction>> Interactions
    {
        get;
        set;
    }

    public Interactor<EnemySpacetime, EnemySpacetimeInteractionContext, EnemySpacetimeInteractionEvent,
        EnemySpacetimeInteraction> Interactor { get; set; }

    public EnemySpacetimeInteractionContext InteractionContext { get; set; } = new();
    public int RewindState { get; set; }
    public State<EnemySpacetime, State> CurrentState { get; set; }
    public Dictionary<State, State<EnemySpacetime, State>> States { get; set; }
    public StateChanger<EnemySpacetime, State> StateChanger { get; set; }

    [ExportSubgroup("Enemy settings")] [Export]
    public bool AliveOnStart = false;

    public Area2D HeadArea { get; set; }
    public Area2D AttackArea { get; set; }
    public Label Label { get; set; }

    public Tuple<uint, uint> OriginalEntityLayerMask;
    public Tuple<uint, uint> OriginalHeadAreaLayerMask;
    public Tuple<uint, uint> OriginalAttackAreaLayerMask;

    [Export] public float DeathTimelineLength = 300.0f;
    [Export] public SpacetimePathFollow SpacetimePathFollow { get; set; }
    
    public Path2D DeathPath2D { get; set; }
    public PathFollow2D DeathPathFollow { get; set; }

    public Vector2 PlayerPositionWhenEnemyKilled;

    public Player Player;

    public override void _Ready()
    {
        Interactions =
            new Dictionary<EnemySpacetimeInteraction,
                Interaction<EnemySpacetime, EnemySpacetimeInteractionEvent, EnemySpacetimeInteraction>>()
            {
                {EnemySpacetimeInteraction.TryPickupKey, new TryPickupKeyInteraction(this)},
                {EnemySpacetimeInteraction.KillEnemy, new KillEnemyInteraction(this)},
                {EnemySpacetimeInteraction.TryPickupTimelessKey, new TryPickupTimelessKeyInteraction(this)},
            };
        Interactor =
            new Interactor<EnemySpacetime, EnemySpacetimeInteractionContext, EnemySpacetimeInteractionEvent,
                EnemySpacetimeInteraction>(this);

        States = new Dictionary<State, State<EnemySpacetime, State>>()
        {
            {State.Move, new MoveState(this, State.Move)},
            {State.Death, new DeathState(this, State.Death)},
            {State.Wait, new WaitState(this, State.Wait)},
            {State.Rewind, new RewindState(this, State.Rewind)},
        };
        StateChanger = new StateChanger<EnemySpacetime, State>(this);

        DeathPath2D = GetNode<Path2D>("DeathPath2D");
        DeathPathFollow = GetNode<PathFollow2D>("DeathPath2D/PathFollow2D");
        
        var enemyParent = GetParent();
        RemoveChild(DeathPath2D);
        Callable.From(() =>
        {
            enemyParent.AddChild(DeathPath2D);
        }).CallDeferred();
        
        HeadArea = GetNode<Area2D>("EnemyHeadArea");
        AttackArea = GetNode<Area2D>("EnemyAttackArea");
        Label = GetNode<Label>("Label");
        Player = GetTree().GetFirstNodeInGroup("player") as Player;

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

        Label.Text = $"s: {CurrentState.StateEnum}\n" +
                     $"pr: {SpacetimePathFollow.GlobalPosition}";
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