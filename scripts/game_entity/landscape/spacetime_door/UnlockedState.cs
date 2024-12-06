using Godot;
using project768.scripts.player;
using project768.scripts.rewind;
using project768.scripts.state_machine;

namespace project768.scripts.game_entity.landscape.spacetime_door;

public class UnlockedState : State<SpacetimeLockedDoor, SpacetimeLockedDoor.State>
{
    public UnlockedState(SpacetimeLockedDoor entity, SpacetimeLockedDoor.State stateEnum) : base(entity, stateEnum)
    {
    }
    
    public override void EnterState(SpacetimeLockedDoor.State prevState)
    {
        Entity.CollisionShape2D.SetDeferred("disabled", true);
        Entity.LockArea.SetDeferred("monitoring", false);
        
        Entity.AnimationPlayer.SetCurrentAnimation("DoorOpen");
        Entity.AnimationPlayer.SpeedScale = 0.0f;
        Entity.AnimationPlayer.Seek(0.0f, true);
    }
    
    public override void Process(double delta)
    {
        float animationProgress = SpacetimeRewindPlayer.CalculateTimelineProgress(
            Player.Instance.GlobalPosition.X,
            Entity.PlayerPositionWhenDoorUnlocked.X,
            Entity.PlayerPositionWhenDoorUnlocked.X + Entity.OpenDoorTimelineLength
        );
        float animationPosition = (float) Mathf.Lerp(
            0.0f,
            Entity.AnimationPlayer.GetCurrentAnimationLength(),
            Mathf.Clamp(animationProgress, 0.0f, 0.98f)
        );
        Entity.AnimationPlayer.Seek(animationPosition, true);

        if (Player.Instance.GlobalPosition.X < Entity.PlayerPositionWhenDoorUnlocked.X)
        {
            Entity.StateChanger.ChangeState(SpacetimeLockedDoor.State.Locked);
        }
    }
    
}