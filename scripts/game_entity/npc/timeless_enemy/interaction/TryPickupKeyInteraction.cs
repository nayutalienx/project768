using project768.scripts.common.interaction;
using project768.scripts.game_entity.npc.timeless_enemy.interaction.data;

namespace project768.scripts.game_entity.npc.timeless_enemy.interaction;

public class TryPickupKeyInteraction : Interaction<TimelessEnemy, TimelessEnemyInteractionEvent, TimelessEnemyInteraction>
{
    public TryPickupKeyInteraction(TimelessEnemy entity) : base(entity)
    {
    }

    public override void Interact(TimelessEnemyInteractionEvent eventContext)
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

            eventContext.Key.StateChanger.ChangeState(Key.State.Picked);
        }
    }
}