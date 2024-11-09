using project768.scripts.common.interaction;
using project768.scripts.game_entity.npc.enemy.interaction.data;

namespace project768.scripts.game_entity.npc.enemy.interaction;

public class TryPickupTimelessKeyInteraction : Interaction<Enemy, EnemyInteractionEvent, EnemyInteraction>
{
    public TryPickupTimelessKeyInteraction(Enemy entity) : base(entity)
    {
    }

    public override void Interact(EnemyInteractionEvent eventContext)
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