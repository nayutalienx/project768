using project768.scripts.state_machine;

namespace project768.scripts.game_entity.npc.timeless_enemy.state;


public class BaseEnemyState : State<TimelessEnemy, TimelessEnemy.State>
{
    public BaseEnemyState(TimelessEnemy entity, TimelessEnemy.State stateEnum) : base(entity, stateEnum)
    {
    }
    
    protected void DropKey()
    {
        if (Entity.InteractionContext.KeyContext.HasKey)
        {
            Entity.InteractionContext.KeyContext.Key.StateChanger.ChangeState(Key.State.Unpicked);
            Entity.InteractionContext.KeyContext.HasKey = false;
        }
    }

    protected void DropTimelessKey()
    {
        if (Entity.InteractionContext.TimelessKeyContext.HasKey)
        {
            Entity.InteractionContext.TimelessKeyContext.Key.StateChanger.ChangeState(TimelessKey.State.Unpicked);
            Entity.InteractionContext.TimelessKeyContext.HasKey = false;
        }
    }
    
    protected void ProcessTimelessKey()
    {
        if (Entity.InteractionContext.TimelessKeyContext.HasKey)
        {
            Entity.InteractionContext.TimelessKeyContext.Key.GlobalPosition = Entity.GlobalPosition;
        }
    }
}