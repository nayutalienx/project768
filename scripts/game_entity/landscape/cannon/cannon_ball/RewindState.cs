using project768.scripts.common;
using project768.scripts.game_entity.npc.timeless_enemy.interaction.data;
using project768.scripts.state_machine;

namespace project768.scripts.game_entity.landscape.cannon;

public class RewindState : State<CannonBall, CannonBall.State>
{
    public RewindState(CannonBall entity, CannonBall.State stateEnum) : base(entity, stateEnum)
    {
    }
    
    public override void OnBodyEntered(CollisionBody body)
    {
        if (body.Body is TimelessEnemy timelessEnemy)
        {
            timelessEnemy.Interactor.Interact(new TimelessEnemyInteractionEvent(TimelessEnemyInteraction.KillEnemy));
        }
    }
}