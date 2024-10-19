using System;
using Godot;
using project768.scripts.common;

namespace project768.scripts.state_machine;

public abstract class State<T, TEnum>
    where T : Node2D
    where TEnum : IConvertible
{
    public T Entity { get; set; }
    public TEnum StateEnum { get; set; }

    public State(T entity, TEnum stateEnum)
    {
        Entity = entity;
        StateEnum = stateEnum;
    }

    public virtual void EnterState(TEnum prevState)
    {
    }

    public virtual void HandleInput(InputEvent _event)
    {
    }

    public virtual void Process(double delta)
    {
    }

    public virtual void PhysicProcess(double delta)
    {
    }

    public virtual void OnBodyEntered(CollisionBody body)
    {
    }
    
    public virtual void OnBodyExited(CollisionBody body)
    {
    }
}