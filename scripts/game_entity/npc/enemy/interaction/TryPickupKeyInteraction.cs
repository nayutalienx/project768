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
        if (!Entity.InteractionContext.HasKey &&
            eventContext.Key.CurrentState.StateEnum == Key.State.Unpicked)
        {
            Entity.InteractionContext.HasKey = true;
            Entity.InteractionContext.Key = eventContext.Key;
            Entity.InteractionContext.KeyInstanceId = eventContext.Key.GetInstanceId();

            eventContext.Key.StateChanger.ChangeState(Key.State.Picked);
        }
    }
}