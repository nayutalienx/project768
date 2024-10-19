using System;

namespace project768.scripts.common.interaction;

public abstract class Interaction<T, TEventContext, TEnum>
    where TEnum : IConvertible
    where TEventContext : InteractionEvent<TEnum>
{
    public T Entity { get; set; }

    public Interaction(T entity)
    {
        Entity = entity;
    }

    public virtual void Interact(TEventContext eventContext)
    {
    }
}