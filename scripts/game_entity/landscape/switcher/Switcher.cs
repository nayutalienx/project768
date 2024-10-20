using Godot;
using System;
using System.Collections.Generic;
using project768.scripts.common;
using project768.scripts.game_entity.landscape.switcher;
using project768.scripts.rewind.entity;
using project768.scripts.state_machine;

public partial class Switcher : Node2D,
    IStateMachineEntity<Switcher, Switcher.State>,
    IRewindable
{
    public enum SwitcherReaction
    {
        NoReaction,
        StartAnimation
    }

    [Export] public SwitcherReaction Reaction { get; set; }
    [Export] public AnimationPlayer ReactAnimationPlayer { get; set; }
    [Export] public string ReactAnimationName { get; set; }

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

    public override void _Ready()
    {
        AnimationPlayer = new RewindableAnimationPlayer(
            GetNode<AnimationPlayer>("AnimationPlayer"),
            new string[]
            {
                "move",
                "end"
            });

        States = new Dictionary<State, State<Switcher, State>>()
        {
            {State.Initial, new InitialState(this, State.Initial)},
            {State.Used, new UsedState(this, State.Used)},
            {State.Rewind, new RewindState(this, State.Rewind)},
        };
        StateChanger = new StateChanger<Switcher, State>(this);


        StateChanger.ChangeState(State.Initial);
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
}