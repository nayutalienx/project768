using project768.scripts.common.interaction;

namespace project768.scripts.game_entity.landscape.switcher.interaction;

public class ToggleSwitcherInteraction : Interaction<Switcher, SwitcherInteractionEvent, SwitcherInteraction>
{
    public ToggleSwitcherInteraction(Switcher entity) : base(entity)
    {
    }

    public override void Interact(SwitcherInteractionEvent eventContext)
    {
        if (Entity.AnimationPlayer.AnimationPlayer.IsPlaying())
        {
            return;
        }

        if (Entity.Type == Switcher.SwitcherType.Single)
        {
            Entity.StateChanger.ChangeState(Switcher.State.Used);
        }
        else
        {
            if (Entity.CurrentState.StateEnum == Switcher.State.Initial)
            {
                Entity.StateChanger.ChangeState(Switcher.State.Used);
            }
            else
            {
                Entity.StateChanger.ChangeState(Switcher.State.Initial);
            }
        }
    }
}