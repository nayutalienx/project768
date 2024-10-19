using System;

namespace project768.scripts.common.interaction;

public class Interactor<T, TContext, TEnum>
    where TEnum : IConvertible
    where TContext : InteractionContext
{
    private IInteractableEntity<T, TContext, TEnum> Entity;

    public Interactor(IInteractableEntity<T, TContext, TEnum> interactableEntity)
    {
        Entity = interactableEntity;
    }

    public void Interact(TEnum interactType)
    {
        Entity.Interactions[interactType.ToInt32(null)].Interact();
    }
}