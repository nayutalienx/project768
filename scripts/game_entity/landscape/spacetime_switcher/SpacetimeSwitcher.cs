using Godot;
using System;
using System.Collections.Generic;
using project768.scripts.common;
using project768.scripts.common.interaction;
using project768.scripts.game_entity.landscape.spacetime_switcher;
using project768.scripts.game_entity.landscape.spacetime_switcher.interaction;
using project768.scripts.game_entity.landscape.spacetime_switcher.interaction.data;
using project768.scripts.game_entity.landscape.switcher.interaction;
using project768.scripts.rewind.entity;
using project768.scripts.state_machine;

public partial class SpacetimeSwitcher : StaticBody2D,
    IStateMachineEntity<SpacetimeSwitcher, SpacetimeSwitcher.State>,
    IRewindable,
    IInteractableEntity<SpacetimeSwitcher, SpacetimeSwitcherInteractionContext, SpacetimeSwitcherInteractionEvent, SpacetimeSwitcherInteraction>
{
    public enum SwitcherReaction
    {
        NoReaction,
        ControlAnimation
    }

    public enum SwitcherType
    {
        Single
    }

    [Export] public SwitcherType Type { get; set; } = SwitcherType.Single;
    [Export] public SwitcherReaction Reaction { get; set; }
    [Export] public SpacetimeAnimationPlayer ReactAnimationPlayer { get; set; }

    public Dictionary<SpacetimeSwitcherInteraction, Interaction<SpacetimeSwitcher, SpacetimeSwitcherInteractionEvent, SpacetimeSwitcherInteraction>>
        Interactions { get; set; }

    public Interactor<SpacetimeSwitcher, SpacetimeSwitcherInteractionContext, SpacetimeSwitcherInteractionEvent, SpacetimeSwitcherInteraction> Interactor
    {
        get;
        set;
    }

    public SpacetimeSwitcherInteractionContext InteractionContext { get; set; } = new();

    public enum State
    {
        Initial,
        Used,
        Rewind
    }

    public int RewindState { get; set; }
    public State<SpacetimeSwitcher, State> CurrentState { get; set; }
    public Dictionary<State, State<SpacetimeSwitcher, State>> States { get; set; }
    public StateChanger<SpacetimeSwitcher, State> StateChanger { get; set; }
    public Label Label { get; set; }
    public Sprite2D StickSprite;
    public Vector2 PlayerPositionWhenUsed { get; set; }

    public override void _Ready()
    {
        
        StickSprite = GetNode<Sprite2D>("sprites/stick");
        
        Interactions =
            new Dictionary<SpacetimeSwitcherInteraction, Interaction<SpacetimeSwitcher, SpacetimeSwitcherInteractionEvent, SpacetimeSwitcherInteraction>>()
            {
                {SpacetimeSwitcherInteraction.Toggle, new ToggleSpacetimeSwitcherInteraction(this)}
            };
        Interactor =
            new Interactor<SpacetimeSwitcher, SpacetimeSwitcherInteractionContext, SpacetimeSwitcherInteractionEvent, SpacetimeSwitcherInteraction>(this);
        

        States = new Dictionary<State, State<SpacetimeSwitcher, State>>()
        {
            {State.Initial, new InitialState(this, State.Initial)},
            {State.Used, new UsedState(this, State.Used)},
            {State.Rewind, new RewindState(this, State.Rewind)},
        };
        StateChanger = new StateChanger<SpacetimeSwitcher, State>(this);

        Label = GetNode<Label>("Label");


        StateChanger.ChangeState(State.Initial);
    }

    public override void _PhysicsProcess(double delta)
    {
        Label.Text = $"s: {CurrentState.StateEnum}\n";

        CurrentState.PhysicProcess(delta);
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