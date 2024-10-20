using System;
using System.Collections.Generic;


namespace project768.scripts.common.interaction;

public interface IInteractableEntity<T, TContext, TEventContext, TEnum>
    where TEnum : IConvertible
    where TContext : InteractionContext
    where TEventContext : InteractionEvent<TEnum>
{
    public Dictionary<TEnum, Interaction<T, TEventContext, TEnum>> Interactions { get; set; }
    public Interactor<T, TContext, TEventContext, TEnum> Interactor { get; set; }
    public TContext InteractionContext { get; set; }
}