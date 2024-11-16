namespace project768.scripts.game_entity.npc.timeless_enemy.state;

public class WaitState : BaseEnemyState
{
    public WaitState(TimelessEnemy entity, TimelessEnemy.State stateEnum) : base(entity, stateEnum)
    {
    }
    
    public override void EnterState(TimelessEnemy.State prevState)
    {
        Entity.Visible = false;
        Entity.GlobalPosition = Entity.InitialPosition;
    }
}