using project768.scripts.common;

namespace project768.scripts.game_entity.npc.timeless_enemy.state;

public class DeathState : BaseEnemyState
{
    public DeathState(TimelessEnemy entity, TimelessEnemy.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void EnterState(TimelessEnemy.State prevState)
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