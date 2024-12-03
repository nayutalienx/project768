using project768.scripts.common;
using project768.scripts.player;
using project768.scripts.rewind;

namespace project768.scripts.game_entity.npc.enemy_spacetime.state;

public class DeathState : BaseEnemySpacetimeState
{
    public DeathState(EnemySpacetime entity, EnemySpacetime.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void EnterState(EnemySpacetime.State prevState)
    {
        Entity.Visible = true;
        Entity.DisableCollision();
        Entity.HeadArea.DisableCollision();
        Entity.AttackArea.DisableCollision();
        DropKey();
        DropTimelessKey();
    }

    public override void PhysicProcess(double delta)
    {
        if (Player.Instance.GlobalPosition.X < Entity.PlayerPositionWhenEnemyKilled.X)
        {
            Entity.StateChanger.ChangeState(EnemySpacetime.State.Move);
            return;
        }

        ProcessTimelinePosition();
    }

    private void ProcessTimelinePosition()
    {
        float deathTimelineProgress = SpacetimeRewindPlayer.CalculateTimelineProgress(
            Player.Instance.GlobalPosition.X,
            Entity.PlayerPositionWhenEnemyKilled.X,
            Entity.PlayerPositionWhenEnemyKilled.X + Entity.DeathTimelineLength
        );

        if (deathTimelineProgress > 0.90)
        {
            Entity.LockWaitingToDeath = false;
            Entity.StateChanger.ChangeState(EnemySpacetime.State.Wait);
            return;
        }

        Entity.DeathPathFollow.ProgressRatio = deathTimelineProgress;

        Entity.GlobalPosition = Entity.DeathPathFollow.GlobalPosition;
    }
}