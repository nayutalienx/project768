using System;

namespace project768.scripts.common.interaction;

public abstract class InteractionEvent<TEnum>
    where TEnum : IConvertible
{
    public TEnum InteractionEnum { get; set; }

    public InteractionEvent(TEnum interactionEnum)
    {
        InteractionEnum = interactionEnum;
    }
}