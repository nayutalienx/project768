using Godot;
using project768.scripts.game_entity.npc.enemy.interaction.data;

namespace project768.scripts.rewind.entity;

public struct EnemyRewindData
{
    public Vector2 GlobalPosition { get; set; }
    public Vector2 Velocity { get; set; }
    public bool Visible { get; set; }
    public Enemy.State CurrentState { get; set; }
    public int Direction { get; set; }

    // Key
    public bool HasKey { get; set; }
    public ulong KeyInstanceId { get; set; }

    public EnemyRewindData(Enemy enemy)
    {
        GlobalPosition = enemy.GlobalPosition;
        Velocity = enemy.Velocity;
        CurrentState = enemy.CurrentState.StateEnum;
        Visible = enemy.Visible;
        Direction = enemy.EnemyDirection;
        // key
        HasKey = enemy.InteractionContext.HasKey;
        KeyInstanceId = enemy.InteractionContext.KeyInstanceId;
    }

    public void ApplyData(Enemy enemy)
    {
        enemy.Velocity = Velocity;
        enemy.GlobalPosition = GlobalPosition;
        enemy.RewindState = (int) CurrentState;
        enemy.Visible = Visible;
        enemy.EnemyDirection = Direction;
        // key
        enemy.InteractionContext.HasKey = HasKey;
        enemy.InteractionContext.KeyInstanceId = KeyInstanceId;
    }
}