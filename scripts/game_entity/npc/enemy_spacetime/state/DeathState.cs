using project768.scripts.common;
using project768.scripts.rewind;

namespace project768.scripts.game_entity.npc.enemy_spacetime.state;

public class DeathState : BaseEnemySpacetimeState
{
    public DeathState(EnemySpacetime entity, EnemySpacetime.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void EnterState(EnemySpacetime.State prevState)
    {
        Entity.DisableCollision();
        Entity.HeadArea.DisableCollision();
        Entity.AttackArea.DisableCollision();
        DropKey();
        DropTimelessKey();
    }

    public override void PhysicProcess(double delta)
    {
        if (Entity.Player.GlobalPosition.X < Entity.PlayerPositionWhenEnemyKilled.X)
        {
            Entity.StateChanger.ChangeState(EnemySpacetime.State.Move);
            return;
        }

        ProcessTimelinePosition();
    }

    private void ProcessTimelinePosition()
    {
        float deathTimelineProgress = SpacetimeRewindPlayer.CalculateTimelineProgress(
            Entity.Player.GlobalPosition.X,
            Entity.PlayerPositionWhenEnemyKilled.X,
            Entity.PlayerPositionWhenEnemyKilled.X + Entity.DeathTimelineLength
        );
        Entity.DeathPathFollow.ProgressRatio = deathTimelineProgress;

        Entity.GlobalPosition = Entity.DeathPathFollow.GlobalPosition;
    }
}