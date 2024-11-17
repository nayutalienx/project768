using Godot;
using System;
using project768.scripts.common;
using project768.scripts.game_entity.npc.enemy.interaction.data;
using project768.scripts.game_entity.npc.jumping_enemy.interaction.data;
using project768.scripts.game_entity.npc.timeless_enemy.interaction.data;
using project768.scripts.player;
using project768.scripts.player.interaction;
using project768.scripts.rewind.entity;

public partial class Claw : Area2D, IRewindable
{
    public int RewindState { get; set; }
    public RewindableAnimationPlayer AnimationPlayer { get; set; }

    public override void _Ready()
    {
        AnimationPlayer = new RewindableAnimationPlayer(
            GetNode<AnimationPlayer>("AnimationPlayer") as AnimationPlayer,
            new[]
            {
                "motion",
            }
        );

        AnimationPlayer.Play("motion");

        BodyEntered += body =>
        {
            if (body is Player player)
            {
                player.Interactor.Interact(new PlayerInteractionEvent(PlayerInteraction.KillPlayer));
                return;
            }

            if (body is Enemy enemy)
            {
                enemy.Interactor.Interact(new EnemyInteractionEvent(EnemyInteraction.KillEnemy));
            }
            
            if (body is TimelessEnemy timelessEnemy)
            {
                timelessEnemy.Interactor.Interact(new TimelessEnemyInteractionEvent(TimelessEnemyInteraction.KillEnemy));
            }

            if (body is JumpingEnemy jumpingEnemy)
            {
                jumpingEnemy.Interactor.Interact(new JumpingEnemyInteractionEvent(JumpingEnemyInteraction.KillEnemy));
            }
        };
    }
    
    public void RewindStarted()
    {
        
    }

    public void RewindFinished()
    {
        
    }

    public void OnRewindSpeedChanged(int speed)
    {
        AnimationPlayer.UpdateRewindSpeed(speed);
    }
}