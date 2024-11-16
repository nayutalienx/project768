using project768.scripts.common;

namespace project768.scripts.game_entity.npc.timeless_enemy.state;

public class DeathState : BaseEnemyState
{
    public DeathState(TimelessEnemy entity, TimelessEnemy.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void EnterState(TimelessEnemy.State prevState)
    {
        Entity.DeathStopTimer.Reset();
        Entity.DisableCollision();
        Entity.HeadArea.DisableCollision();
        Entity.AttackArea.DisableCollision();
        DropKey();
        DropTimelessKey();
    }

    public override void PhysicProcess(double delta)
    {
        if (Entity.DeathStopTimer.IsExpired())
        {
            Entity.StateChanger.ChangeState(TimelessEnemy.State.Wait);
        }
        else
        {
            Entity.DeathStopTimer.Update(delta);
            Entity.Velocity += Entity.GetGravity() * (float) delta;
            Entity.MoveAndSlide();
        }
    }
}