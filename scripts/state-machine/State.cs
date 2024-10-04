using System;
using Godot;

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

    public virtual void ExitState(TEnum prevState)
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

    protected Tuple<uint, uint> DisableCollision(CollisionObject2D entity)
    {
        var res = new Tuple<uint, uint>(entity.CollisionLayer, entity.CollisionMask);
        entity.CollisionLayer = 0;
        entity.CollisionMask = 0;
        return res;
    }

    protected void EnableCollision(CollisionObject2D entity, Tuple<uint, uint> layerMask)
    {
        entity.CollisionLayer = layerMask.Item1;
        entity.CollisionMask = layerMask.Item2;
    }
}