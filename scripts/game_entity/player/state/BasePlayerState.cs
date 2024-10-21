using Godot;
using project768.scripts.player.interaction;
using project768.scripts.state_machine;

namespace project768.scripts.player;

public class BasePlayerState : State<Player, Player.State>
{
    public BasePlayerState(Player entity, Player.State stateEnum) : base(entity, stateEnum)
    {
    }

    protected void RecoverKeyOnEnterState(Player.State prevState)
    {
        if (prevState == Player.State.Rewind && Entity.InteractionContext.KeyContext.HasKey)
        {
            Entity.InteractionContext.KeyContext.Key =
                GodotObject.InstanceFromId(Entity.InteractionContext.KeyContext.KeyInstanceId) as Key;
        }
    }

    protected void RecoverSwitcherOnEnterState(Player.State prevState)
    {
        if (prevState == Player.State.Rewind && Entity.InteractionContext.SwitcherContext.JoinedSwitcherArea)
        {
            Entity.InteractionContext.SwitcherContext.Switcher =
                GodotObject.InstanceFromId(Entity.InteractionContext.SwitcherContext.InstanceId) as Switcher;
        }
    }

    protected void ProcessKey()
    {
        if (Entity.InteractionContext.KeyContext.HasKey)
        {
            Entity.InteractionContext.KeyContext.Key.Transform = Entity.Transform;
            Entity.InteractionContext.KeyContext.Key.GlobalPosition = Entity.GlobalPosition;
        }
    }
}