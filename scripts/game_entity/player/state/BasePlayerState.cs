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
        if (prevState == Player.State.Rewind && Entity.InteractionContext.HasKey)
        {
            Entity.InteractionContext.Key = GodotObject.InstanceFromId(Entity.InteractionContext.KeyInstanceId) as Key;
        }
    }

    protected void ProcessKey()
    {
        if (Entity.InteractionContext.HasKey)
        {
            Entity.InteractionContext.Key.Transform = Entity.Transform;
        }
    }
}