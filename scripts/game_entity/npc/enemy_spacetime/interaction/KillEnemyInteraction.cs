using project768.scripts.common.interaction;
using project768.scripts.game_entity.npc.enemy_spacetime.interaction.data;

namespace project768.scripts.game_entity.npc.enemy_spacetime.interaction;

public class
    KillEnemyInteraction : Interaction<EnemySpacetime, EnemySpacetimeInteractionEvent, EnemySpacetimeInteraction>
{
    public KillEnemyInteraction(EnemySpacetime entity) : base(entity)
    {
    }

    public override void Interact(EnemySpacetimeInteractionEvent eventContext)
    {
        if (Entity.CurrentState.StateEnum == EnemySpacetime.State.Move)
        {
            Entity.PlayerPositionWhenEnemyKilled = Entity.Player.GlobalPosition;
            Entity.DeathPath2D.GlobalPosition = Entity.GlobalPosition;
            Entity.StateChanger.ChangeState(EnemySpacetime.State.Death);
        }
    }
}