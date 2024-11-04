using project768.scripts.state_machine;

namespace project768.scripts.game_entity.landscape.switcher;

public class InitialState : State<Switcher, Switcher.State>
{
    public InitialState(Switcher entity, Switcher.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void EnterState(Switcher.State prevState)
    {
        if (prevState == Switcher.State.Used)
        {
            Entity.AnimationPlayer.InvertAndPlay("move_back");
            if (Entity.Reaction == Switcher.SwitcherReaction.StartAnimation)
            {
                Entity.InvertAndPlayReact(Entity.ReactAnimationNameOnBackward);
            }
        }
    }
    
}