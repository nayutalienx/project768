using System;
using Godot;

namespace project768.scripts.state_machine;

public class StateChanger<T, TEnum>
    where T : Node2D
    where TEnum : IConvertible
{
    public IStateMachineEntity<T, TEnum> StateMachineEntity { get; set; }

    public StateChanger(IStateMachineEntity<T, TEnum> entity)
    {
        StateMachineEntity = entity;
    }

    public void ChangeState(TEnum newState)
    {
        if (StateMachineEntity.CurrentState != null)
        {
            StateMachineEntity.CurrentState.ExitState(newState);
        }

        var prevState = StateMachineEntity.CurrentState;
        StateMachineEntity.CurrentState = StateMachineEntity.States[newState.ToInt32(null)];

        if (prevState != null)
        {
            StateMachineEntity.CurrentState.EnterState(prevState.StateEnum);
        }
        else
        {
            StateMachineEntity.CurrentState.EnterState(default);
        }

        // GD.Print(
        //     $"{StateMachineEntity.CurrentState.Entity.Name} : " +
        //     $"{prevState?.GetType().Name} -> " +
        //     $"{StateMachineEntity.CurrentState.GetType().Name}");
    }
}