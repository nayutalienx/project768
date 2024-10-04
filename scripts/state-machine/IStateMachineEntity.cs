using System;
using Godot;

namespace project768.scripts.state_machine;

public interface IStateMachineEntity<T, TEnum>
    where T : Node2D
    where TEnum : IConvertible
{
    public State<T, TEnum> CurrentState { get; set; }
    public State<T, TEnum>[] States { get; set; }
    public StateChanger<T, TEnum> StateChanger { get; set; }
}