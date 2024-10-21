using project768.scripts.state_machine;

namespace project768.scripts.enemy;

public class BaseEnemyState : State<Enemy, Enemy.State>
{
    public BaseEnemyState(Enemy entity, Enemy.State stateEnum) : base(entity, stateEnum)
    {
    }
    
    protected void DropKey()
    {
        if (Entity.InteractionContext.HasKey)
        {
            Entity.InteractionContext.Key.StateChanger.ChangeState(Key.State.Unpicked);
            Entity.InteractionContext.HasKey = false;
        }
    }
}