using Godot;
using System;
using project768.scripts.game_entity.npc.enemy.interaction.data;
using project768.scripts.game_entity.npc.jumping_enemy.interaction.data;
using project768.scripts.game_entity.npc.timeless_enemy.interaction.data;
using project768.scripts.player;
using project768.scripts.player.interaction;
using project768.scripts.rewind;

public partial class SpacetimeClaw : Area2D
{
    public AnimationPlayer AnimationPlayer { get; set; }

    [Export] public float ClawIntervalInPixel { get; set; } = 1000.0f;

    public override void _Ready()
    {
        AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        AnimationPlayer.Play("motion");
        AnimationPlayer.SpeedScale = 0.0f;
        AnimationPlayer.Seek(0.0f, true);

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
                timelessEnemy.Interactor.Interact(
                    new TimelessEnemyInteractionEvent(TimelessEnemyInteraction.KillEnemy));
            }

            if (body is JumpingEnemy jumpingEnemy)
            {
                jumpingEnemy.Interactor.Interact(new JumpingEnemyInteractionEvent(JumpingEnemyInteraction.KillEnemy));
            }
        };
    }

    public override void _PhysicsProcess(double delta)
    {
        float intervalMod = SpacetimeRewindPlayer.Instance.GetCurrentTimelinePosition() % ClawIntervalInPixel;
        float animationRatio = Mathf.Clamp(intervalMod / ClawIntervalInPixel, 0.0f, 0.98f);
        float animationPosition = (float) Mathf.Lerp(0.0f, AnimationPlayer.GetCurrentAnimationLength(), animationRatio);
        AnimationPlayer.Seek(animationPosition , true);
    }
}