using project768.scripts.state_machine;

namespace project768.scripts.game_entity.landscape.timeless_switcher;

public class UsedState : State<TimelessSwitcher, TimelessSwitcher.State>
{
    public UsedState(TimelessSwitcher entity, TimelessSwitcher.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void EnterState(TimelessSwitcher.State prevState)
    {
        if (prevState == TimelessSwitcher.State.Initial)
        {
            Entity.AnimationPlayer.InvertAndPlay("move");
            if (Entity.Reaction == TimelessSwitcher.SwitcherReaction.StartAnimation)
            {
                Entity.InvertAndPlayReact(Entity.ReactAnimationNameOnForward);
            }
        }
    }
    
}