using project768.scripts.state_machine;

namespace project768.scripts.game_entity.landscape.spacetime_door;

public class RewindState : State<SpacetimeLockedDoor, SpacetimeLockedDoor.State>
{
    public RewindState(SpacetimeLockedDoor entity, SpacetimeLockedDoor.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void EnterState(SpacetimeLockedDoor.State prevState)
    {
        Entity.AnimationPlayer.AnimationPlayer.SpeedScale = 0.0f;
    }
}