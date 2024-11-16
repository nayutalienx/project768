namespace project768.scripts.enemy;

public class WaitState : BaseEnemyState
{
    public WaitState(Enemy entity, Enemy.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void EnterState(Enemy.State prevState)
    {
        Entity.Visible = false;
        Entity.GlobalPosition = Entity.InitialPosition;
    }
}