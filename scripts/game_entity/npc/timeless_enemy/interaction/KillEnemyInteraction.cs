using Godot;
using project768.scripts.common.interaction;
using project768.scripts.game_entity.npc.timeless_enemy.interaction.data;

namespace project768.scripts.game_entity.npc.timeless_enemy.interaction;

public class KillEnemyInteraction : Interaction<TimelessEnemy, TimelessEnemyInteractionEvent, TimelessEnemyInteraction>
{
    public KillEnemyInteraction(TimelessEnemy entity) : base(entity)
    {
    }

    public override void Interact(TimelessEnemyInteractionEvent eventContext)
    {
        if (Entity.CurrentState.StateEnum == TimelessEnemy.State.Move)
        {
            GD.Print($"{Entity.Name} killed");
            Entity.StateChanger.ChangeState(TimelessEnemy.State.Death);
        }
    }
}