using project768.scripts.state_machine;

namespace project768.scripts.game_entity.landscape.switcher;

public class UsedState : State<Switcher, Switcher.State>
{
    public UsedState(Switcher entity, Switcher.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void EnterState(Switcher.State prevState)
    {
        if (prevState == Switcher.State.Initial)
        {
            Entity.AnimationPlayer.InvertAndPlay("move");
            if (Entity.Reaction == Switcher.SwitcherReaction.StartAnimation)
            {
                Entity.InvertAndPlayReact(Entity.ReactAnimationNameOnForward);
            }
        }
    }
    
}