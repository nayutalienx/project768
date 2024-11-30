using project768.scripts.common.interaction;
using project768.scripts.game_entity.npc.enemy_spacetime.interaction.data;

namespace project768.scripts.game_entity.npc.enemy_spacetime.interaction;

public class TryPickupKeyInteraction : Interaction<EnemySpacetime, EnemySpacetimeInteractionEvent, EnemySpacetimeInteraction>
{
    public TryPickupKeyInteraction(EnemySpacetime entity) : base(entity)
    {
    }

    public override void Interact(EnemySpacetimeInteractionEvent eventContext)
    {
        if (Entity.InteractionContext.KeyContext.HasKey || Entity.InteractionContext.TimelessKeyContext.HasKey)
        {
            return;
        }

        if (
            eventContext.Key.CurrentState.StateEnum == Key.State.Unpicked)
        {
            Entity.InteractionContext.KeyContext.HasKey = true;
            Entity.InteractionContext.KeyContext.Key = eventContext.Key;
            Entity.InteractionContext.KeyContext.KeyInstanceId = eventContext.Key.GetInstanceId();

            eventContext.Key.StateChanger.ChangeState(Key.State.Picked);
        }
    }
}