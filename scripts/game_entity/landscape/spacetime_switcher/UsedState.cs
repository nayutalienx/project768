using project768.scripts.player;
using project768.scripts.state_machine;

namespace project768.scripts.game_entity.landscape.spacetime_switcher;

public class UsedState : State<SpacetimeSwitcher, SpacetimeSwitcher.State>
{
    public UsedState(SpacetimeSwitcher entity, SpacetimeSwitcher.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void EnterState(SpacetimeSwitcher.State prevState)
    {
        Entity.StickSprite.Rotation = -90.0f;
        if (prevState != SpacetimeSwitcher.State.Rewind)
        {
            Entity.PlayerPositionWhenUsed = Player.Instance.GlobalPosition;
        }
        if (Entity.Reaction == SpacetimeSwitcher.SwitcherReaction.ControlAnimation)
        {
            Entity.ReactAnimationPlayer.ShouldPlay = true;
            if (prevState != SpacetimeSwitcher.State.Rewind)
            {
                Entity.ReactAnimationPlayer.PlayerPositionStartTimeline = Entity.PlayerPositionWhenUsed;
            }
        }
    }
    
    public override void PhysicProcess(double delta)
    {
        if (Player.Instance.GlobalPosition.X < Entity.PlayerPositionWhenUsed.X)
        {
            Entity.StateChanger.ChangeState(SpacetimeSwitcher.State.Initial);
        }
    }
}