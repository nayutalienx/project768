using System;

namespace project768.scripts.common.interaction;

public abstract class Interaction<T, TInteractionEvent, TEnum>
    where TEnum : IConvertible
    where TInteractionEvent : InteractionEvent<TEnum>
{
    public T Entity { get; set; }

    public Interaction(T entity)
    {
        Entity = entity;
    }

    public virtual void Interact(TInteractionEvent eventContext)
    {
    }
}