using Godot;
using project768.scripts.game_entity.common;
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
    
    protected void ProcessKey()
    {
        if (Entity.InteractionContext.KeyContext.HasKey)
        {
            Entity.InteractionContext.KeyContext.Key.GlobalTransform = Entity.GlobalTransform;
            Entity.InteractionContext.KeyContext.Key.GlobalPosition = Entity.GlobalPosition;
        }
    }
    
    protected void ProcessTimelessKey()
    {
        if (Entity.InteractionContext.TimelessKeyContext.HasKey)
        {
            Entity.InteractionContext.TimelessKeyContext.Key.GlobalTransform = Entity.GlobalTransform;
            Entity.InteractionContext.TimelessKeyContext.Key.GlobalPosition = Entity.GlobalPosition;
        }
    }
}