using project768.scripts.state_machine;

namespace project768.scripts.game_entity.landscape.timeless_switcher;

public class InitialState : State<TimelessSwitcher, TimelessSwitcher.State>
{
    public InitialState(TimelessSwitcher entity, TimelessSwitcher.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void EnterState(TimelessSwitcher.State prevState)
    {
        if (prevState == TimelessSwitcher.State.Used)
        {
            Entity.AnimationPlayer.InvertAndPlay("move_back");
            if (Entity.Reaction == TimelessSwitcher.SwitcherReaction.StartAnimation)
            {
                Entity.InvertAndPlayReact(Entity.ReactAnimationNameOnBackward);
            }
        }
    }
}