using Godot;
using project768.scripts.common;

namespace project768.scripts.game_entity.npc.timeless_enemy_boss.state;

public class DeathState : BaseTimelessEnemyBossState
{
    public DeathState(TimelessEnemyBoss entity, TimelessEnemyBoss.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void EnterState(TimelessEnemyBoss.State prevState)
    {
        Entity.Sprite.SetMaterial(null);
        if (Entity.TimelessKey != null)
        {
            Entity.TimelessKey.StateChanger.ChangeState(TimelessKey.State.Unpicked);
        }
    }

    public override void OnBodyEntered(CollisionBody body)
    {
    }
}