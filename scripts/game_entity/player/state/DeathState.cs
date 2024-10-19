using project768.scripts.state_machine;

namespace project768.scripts.player;

public class DeathState : State<Player, Player.State>
{
    public DeathState(Player entity, Player.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void EnterState(Player.State prevState)
    {
        Entity.Visible = false;
    }
}