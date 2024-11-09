using project768.scripts.common.interaction;
using project768.scripts.game_entity.npc.enemy.interaction.data;

namespace project768.scripts.game_entity.npc.enemy.interaction;

public class TryPickupKeyInteraction : Interaction<Enemy, EnemyInteractionEvent, EnemyInteraction>
{
    public TryPickupKeyInteraction(Enemy entity) : base(entity)
    {
    }

    public override void Interact(EnemyInteractionEvent eventContext)
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