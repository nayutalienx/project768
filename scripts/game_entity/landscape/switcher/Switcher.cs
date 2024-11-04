using Godot;
using System;
using System.Collections.Generic;
using project768.scripts.common;
using project768.scripts.common.interaction;
using project768.scripts.game_entity.landscape.switcher;
using project768.scripts.game_entity.landscape.switcher.interaction;
using project768.scripts.rewind.entity;
using project768.scripts.state_machine;

public partial class Switcher : Node2D,
    IStateMachineEntity<Switcher, Switcher.State>,
    IRewindable,
    IInteractableEntity<Switcher, SwitcherInteractionContext, SwitcherInteractionEvent, SwitcherInteraction>
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

    public Dictionary<SwitcherInteraction, Interaction<Switcher, SwitcherInteractionEvent, SwitcherInteraction>>
        Interactions { get; set; }

    public Interactor<Switcher, SwitcherInteractionContext, SwitcherInteractionEvent, SwitcherInteraction> Interactor
    {
        get;
        set;
    }

    public SwitcherInteractionContext InteractionContext { get; set; } = new();

    public enum State
    {
        Initial,
        Used,
        Rewind
    }

    public int RewindState { get; set; }
    public State<Switcher, State> CurrentState { get; set; }
    public Dictionary<State, State<Switcher, State>> States { get; set; }
    public StateChanger<Switcher, State> StateChanger { get; set; }
    public RewindableAnimationPlayer AnimationPlayer { get; set; }
    public Label Label { get; set; }

    public override void _Ready()
    {
        Interactions =
            new Dictionary<SwitcherInteraction, Interaction<Switcher, SwitcherInteractionEvent, SwitcherInteraction>>()
            {
                {SwitcherInteraction.Toggle, new ToggleSwitcherInteraction(this)}
            };
        Interactor =
            new Interactor<Switcher, SwitcherInteractionContext, SwitcherInteractionEvent, SwitcherInteraction>(this);


        AnimationPlayer = new RewindableAnimationPlayer(
            GetNode<AnimationPlayer>("AnimationPlayer"),
            new string[]
            {
                "move",
                "move_back",
                "end",
                "RESET",
            });

        States = new Dictionary<State, State<Switcher, State>>()
        {
            {State.Initial, new InitialState(this, State.Initial)},
            {State.Used, new UsedState(this, State.Used)},
            {State.Rewind, new RewindState(this, State.Rewind)},
        };
        StateChanger = new StateChanger<Switcher, State>(this);

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
        AnimationPlayer.UpdateRewindSpeed(speed);
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