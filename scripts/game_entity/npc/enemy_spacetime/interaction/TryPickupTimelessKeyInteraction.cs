using project768.scripts.common.interaction;
using project768.scripts.game_entity.npc.enemy_spacetime.interaction.data;

namespace project768.scripts.game_entity.npc.enemy_spacetime.interaction;

public class
    TryPickupTimelessKeyInteraction : Interaction<EnemySpacetime, EnemySpacetimeInteractionEvent,
    EnemySpacetimeInteraction>
{
    public TryPickupTimelessKeyInteraction(EnemySpacetime entity) : base(entity)
    {
    }

    public override void Interact(EnemySpacetimeInteractionEvent eventContext)
    {
        if (Entity.InteractionContext.KeyContext.HasKey || Entity.InteractionContext.TimelessKeyContext.HasKey)
        {
            return;
        }

        if (
            eventContext.TimelessKey.CurrentState.StateEnum == TimelessKey.State.Unpicked)
        {
            Entity.InteractionContext.TimelessKeyContext.HasKey = true;
            Entity.InteractionContext.TimelessKeyContext.Key = eventContext.TimelessKey;

            eventContext.TimelessKey.StateChanger.ChangeState(TimelessKey.State.Picked);
        }
    }
}