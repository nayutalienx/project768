using System;
using Godot;

namespace project768.scripts.common.interaction;

public class Interactor<T, TContext, TEventContext, TEnum>
    where TContext : InteractionContext
    where TEnum : IConvertible
    where TEventContext : InteractionEvent<TEnum>
{
    private IInteractableEntity<T, TContext, TEventContext, TEnum> Entity;

    public Interactor(IInteractableEntity<T, TContext, TEventContext, TEnum> interactableEntity)
    {
        Entity = interactableEntity;
    }

    public void Interact(TEventContext eventContext)
    {
        var interaction = Entity.Interactions[eventContext.InteractionEnum.ToInt32(null)];
        GD.Print($"Invoke Interact: {interaction.GetType().Name}");
        interaction.Interact(eventContext);
    }
}