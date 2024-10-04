using System;
using Godot;
using project768.scripts.state_machine;

namespace project768.scripts.player;

public class RewindState : State<Player, Player.State>
{
    private Tuple<uint, uint> origCollission;
    public RewindState(Player entity, Player.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void EnterState(Player.State prevState)
    {
        origCollission = DisableCollision(Entity);
        base.EnterState(prevState);
    }

    public override void ExitState(Player.State nextState)
    {
        EnableCollision(Entity, origCollission);
        Entity.CleanCache();
    }
}