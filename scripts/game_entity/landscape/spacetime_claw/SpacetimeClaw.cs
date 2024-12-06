using Godot;
using System;
using project768.scripts.game_entity.npc.enemy.interaction.data;
using project768.scripts.game_entity.npc.jumping_enemy.interaction.data;
using project768.scripts.game_entity.npc.timeless_enemy.interaction.data;
using project768.scripts.player;
using project768.scripts.player.interaction;

public partial class SpacetimeClaw : Area2D
{
	public int RewindState { get; set; }
	public AnimationPlayer AnimationPlayer { get; set; }

	public override void _Ready()
	{
		AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

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
	
}