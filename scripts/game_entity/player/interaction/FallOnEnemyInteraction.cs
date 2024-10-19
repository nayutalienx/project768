using project768.scripts.common.interaction;

namespace project768.scripts.player.interaction;

public class FallOnEnemyInteraction : Interaction<Player, PlayerInteractionEvent, PlayerInteraction>
{
    public FallOnEnemyInteraction(Player entity) : base(entity)
    {
    }

    public override void Interact(PlayerInteractionEvent eventContext)
    {
        Entity.Velocity = Entity.Velocity with {Y = Entity.JumpVelocity};
    }
}