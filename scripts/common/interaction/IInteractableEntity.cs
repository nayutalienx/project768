using System;

namespace project768.scripts.common.interaction;

public interface IInteractableEntity<T, TContext, TEnum>
    where TEnum : IConvertible
    where TContext : InteractionContext
{
    public Interaction<T>[] Interactions { get; set; }
    public Interactor<T, TContext, TEnum> Interactor { get; set; }
    public TContext InteractionContext { get; set; }
}