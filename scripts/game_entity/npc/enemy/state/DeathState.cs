using project768.scripts.common;
using project768.scripts.state_machine;

namespace project768.scripts.enemy;

public class DeathState : BaseEnemyState
{
    public DeathState(Enemy entity, Enemy.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void EnterState(Enemy.State prevState)
    {
        Entity.GlobalPosition = Entity.InitialPosition;
        Entity.DisableCollision();
        Entity.HeadArea.DisableCollision();
        Entity.AttackArea.DisableCollision();
        Entity.Visible = false;
        DropKey();
        DropTimelessKey();
    }
}