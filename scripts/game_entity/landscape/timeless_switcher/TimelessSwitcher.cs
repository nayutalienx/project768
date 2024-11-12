using System.Collections.Generic;
using Godot;

using project768.scripts.common;
using project768.scripts.common.interaction;
using project768.scripts.game_entity.landscape.timeless_switcher;
using project768.scripts.game_entity.landscape.timeless_switcher.interaction;
using project768.scripts.game_entity.landscape.timeless_switcher.interaction.data;
using project768.scripts.state_machine;


public partial class TimelessSwitcher : StaticBody2D,
    IStateMachineEntity<TimelessSwitcher, TimelessSwitcher.State>,
    IInteractableEntity<TimelessSwitcher, TimelessSwitcherInteractionContext, TimelessSwitcherInteractionEvent, TimelessSwitcherInteraction>
{
    public enum SwitcherReaction
    {
        NoReaction,
        StartAnimation
    }

    public enum SwitcherType
    {
        Single,
        Multiple
    }

    [Export] public SwitcherType Type { get; set; } = SwitcherType.Single;
    [Export] public SwitcherReaction Reaction { get; set; }
    [Export] public AnimationPlayer ReactAnimationPlayer { get; set; }
    [Export] public string ReactAnimationNameOnForward { get; set; }
    [Export] public string ReactAnimationNameOnBackward { get; set; }

    public Dictionary<TimelessSwitcherInteraction, Interaction<TimelessSwitcher, TimelessSwitcherInteractionEvent, TimelessSwitcherInteraction>>
        Interactions { get; set; }

    public Interactor<TimelessSwitcher, TimelessSwitcherInteractionContext, TimelessSwitcherInteractionEvent, TimelessSwitcherInteraction> Interactor
    {
        get;
        set;
    }

    public TimelessSwitcherInteractionContext InteractionContext { get; set; } = new();

    public enum State
    {
        Initial,
        Used
    }

    public int RewindState { get; set; }
    public State<TimelessSwitcher, State> CurrentState { get; set; }
    public Dictionary<State, State<TimelessSwitcher, State>> States { get; set; }
    public StateChanger<TimelessSwitcher, State> StateChanger { get; set; }
    public RewindableAnimationPlayer AnimationPlayer { get; set; }
    public Label Label { get; set; }

    public override void _Ready()
    {
        Interactions =
            new Dictionary<TimelessSwitcherInteraction, Interaction<TimelessSwitcher, TimelessSwitcherInteractionEvent, TimelessSwitcherInteraction>>()
            {
                {TimelessSwitcherInteraction.Toggle, new ToggleTimelessSwitcherInteraction(this)}
            };
        Interactor =
            new Interactor<TimelessSwitcher, TimelessSwitcherInteractionContext, TimelessSwitcherInteractionEvent, TimelessSwitcherInteraction>(this);


        AnimationPlayer = new RewindableAnimationPlayer(
            GetNode<AnimationPlayer>("AnimationPlayer"),
            new string[]
            {
                "move",
                "move_back",
                "end",
                "RESET",
            });

        States = new Dictionary<State, State<TimelessSwitcher, State>>()
        {
            {State.Initial, new InitialState(this, State.Initial)},
            {State.Used, new UsedState(this, State.Used)},
        };
        StateChanger = new StateChanger<TimelessSwitcher, State>(this);

        Label = GetNode<Label>("Label");


        StateChanger.ChangeState(State.Initial);
    }

    public override void _PhysicsProcess(double delta)
    {
        if (AnimationPlayer.AnimationPlayer.IsPlaying())
        {
            Label.Text = $"s: {CurrentState.StateEnum}\n" +
                         $"a: {AnimationPlayer.CurrentAnimation}\n" +
                         $"apos: {AnimationPlayer.AnimationPlayer.CurrentAnimationPosition}";
        }
        else
        {
            Label.Text = $"s: {CurrentState.StateEnum}\n";
        }

        CurrentState.PhysicProcess(delta);
    }

    public void InvertAndPlayReact(string animation)
    {
        if (ReactAnimationPlayer.IsPlaying())
        {
            double currentPos = ReactAnimationPlayer.CurrentAnimationPosition;
            ReactAnimationPlayer.SetCurrentAnimation(animation);
            double newPos = ReactAnimationPlayer.CurrentAnimationLength - currentPos;
            ReactAnimationPlayer.Seek(newPos, true);
        }
        else
        {
            ReactAnimationPlayer.Play(animation);
        }
    }
}