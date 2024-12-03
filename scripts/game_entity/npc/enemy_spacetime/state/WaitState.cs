using project768.scripts.common;
using project768.scripts.player;
using project768.scripts.rewind;

namespace project768.scripts.game_entity.npc.enemy_spacetime.state;

public class WaitState : BaseEnemySpacetimeState
{
    public WaitState(EnemySpacetime entity, EnemySpacetime.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void EnterState(EnemySpacetime.State prevState)
    {
        Entity.DisableCollision();
        Entity.HeadArea.DisableCollision();
        Entity.AttackArea.DisableCollision();
        Entity.Visible = false;

        if (prevState == EnemySpacetime.State.Death)
        {
            Entity.PlayerPositionWhenEnemyWaitingAfterKilled = Player.Instance.GlobalPosition;
        }
    }

    public override void PhysicProcess(double delta)
    {
        if (!Entity.LockWaitingToDeath &&
            Player.Instance.GlobalPosition.X < Entity.PlayerPositionWhenEnemyWaitingAfterKilled.X)
        {
            Entity.StateChanger.ChangeState(EnemySpacetime.State.Death);
            return;
        }

        if (!Entity.AliveOnStart && !Entity.LockSpawn &&
            SpacetimeRewindPlayer.Instance.GetCurrentTimelinePosition() > Entity.SpawnTimeInPixel)
        {
            Entity.LockSpawn = true;
            Entity.StateChanger.ChangeState(EnemySpacetime.State.Move);
            return;
        }
    }
}