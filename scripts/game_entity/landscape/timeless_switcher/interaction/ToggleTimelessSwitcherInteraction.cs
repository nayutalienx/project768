using project768.scripts.common.interaction;
using project768.scripts.game_entity.landscape.timeless_switcher.interaction.data;

namespace project768.scripts.game_entity.landscape.timeless_switcher.interaction;

public class ToggleTimelessSwitcherInteraction : Interaction<TimelessSwitcher, TimelessSwitcherInteractionEvent, TimelessSwitcherInteraction>
{
    public ToggleTimelessSwitcherInteraction(TimelessSwitcher entity) : base(entity)
    {
    }

    public override void Interact(TimelessSwitcherInteractionEvent eventContext)
    {
        if (Entity.AnimationPlayer.AnimationPlayer.IsPlaying())
        {
            return;
        }

        if (Entity.Type == TimelessSwitcher.SwitcherType.Single)
        {
            Entity.StateChanger.ChangeState(TimelessSwitcher.State.Used);
        }
        else
        {
            if (Entity.CurrentState.StateEnum == TimelessSwitcher.State.Initial)
            {
                Entity.StateChanger.ChangeState(TimelessSwitcher.State.Used);
            }
            else
            {
                Entity.StateChanger.ChangeState(TimelessSwitcher.State.Initial);
            }
        }
    }
}