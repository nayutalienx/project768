using Godot;
using System;
using project768.scripts.common;
using project768.scripts.game_entity.landscape.switcher;
using project768.scripts.rewind.entity;
using project768.scripts.state_machine;

public partial class Switcher : Node2D,
    IStateMachineEntity<Switcher, Switcher.State>,
    IRewindable
{
    public enum State
    {
        Initial,
        Used,
        Rewind
    }

    public int RewindState { get; set; }
    public State<Switcher, State> CurrentState { get; set; }
    public State<Switcher, State>[] States { get; set; }
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


        States = new State<Switcher, State>[]
        {
            new InitialState(this, State.Initial),
            new UsedState(this, State.Used),
            new RewindState(this, State.Rewind)
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