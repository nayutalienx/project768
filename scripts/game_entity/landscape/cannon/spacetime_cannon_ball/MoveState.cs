using project768.scripts.common;
using project768.scripts.game_entity.npc.enemy.interaction.data;
using project768.scripts.game_entity.npc.jumping_enemy.interaction.data;
using project768.scripts.game_entity.npc.timeless_enemy.interaction.data;
using project768.scripts.player;
using project768.scripts.player.interaction;
using project768.scripts.state_machine;

namespace project768.scripts.game_entity.landscape.cannon.spacetime_cannon_ball;

public class MoveState : State<SpacetimeCannonBall, SpacetimeCannonBall.State>
{
    public MoveState(SpacetimeCannonBall entity, SpacetimeCannonBall.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void EnterState(SpacetimeCannonBall.State prevState)
    {
        Entity.ShowBall();
    }

    public override void PhysicProcess(double delta)
    {
        Entity.GlobalPosition = Entity.SpacetimePathFollow.GlobalPosition;
        if (Player.Instance.PosDelta == 0)
        {
            Entity.Particles.SpeedScale = 0;
        }
        else
        {
            Entity.Particles.SpeedScale = 1;
        }
    }

    public override void OnBodyEntered(CollisionBody body)
    {
        
        if (body.Body is Player player)
        {
            player.Interactor.Interact(new PlayerInteractionEvent(PlayerInteraction.KillPlayer));
            Entity.PlayerKilled = true;
            Entity.PlayerKilledPosition = player.GlobalPosition;
            Entity.StateChanger.ChangeState(SpacetimeCannonBall.State.Wait);
        }
        
    }
}