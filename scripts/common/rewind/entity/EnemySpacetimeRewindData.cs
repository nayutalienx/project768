using Godot;
using project768.scripts.game_entity.npc.enemy.interaction.data;

namespace project768.scripts.rewind.entity;

public struct EnemySpacetimeRewindData
{
    public Vector2 GlobalPosition { get; set; }
    public EnemySpacetime.State CurrentState { get; set; }
    public bool Visible { get; set; }
    public bool LockSpawn { get; set; }
    public bool LockWaitingToDeath { get; set; }
    
    public Vector2 PlayerPositionWhenEnemyKilled;
    public Vector2 PlayerPositionWhenEnemyWaitingAfterKilled;

    // Key
    public bool HasKey { get; set; }
    public ulong KeyInstanceId { get; set; }
    
    public EnemySpacetimeRewindData(EnemySpacetime enemy)
    {
        GlobalPosition = enemy.GlobalPosition;
        Visible = enemy.Visible;
        CurrentState = enemy.CurrentState.StateEnum;
        LockSpawn = enemy.LockSpawn;
        LockWaitingToDeath = enemy.LockWaitingToDeath;
        
        PlayerPositionWhenEnemyKilled = enemy.PlayerPositionWhenEnemyKilled;
        PlayerPositionWhenEnemyWaitingAfterKilled = enemy.PlayerPositionWhenEnemyWaitingAfterKilled;
        
        // key
        HasKey = enemy.InteractionContext.KeyContext.HasKey;
        KeyInstanceId = enemy.InteractionContext.KeyContext.KeyInstanceId;
    }

    public void ApplyData(EnemySpacetime enemy)
    {
        
        enemy.GlobalPosition = GlobalPosition;
        enemy.RewindState = (int) CurrentState;
        enemy.Visible = Visible;
        enemy.LockSpawn = LockSpawn;
        enemy.LockWaitingToDeath = LockWaitingToDeath;
        enemy.PlayerPositionWhenEnemyKilled = PlayerPositionWhenEnemyKilled;
        enemy.PlayerPositionWhenEnemyWaitingAfterKilled = PlayerPositionWhenEnemyWaitingAfterKilled;
        // key
        enemy.InteractionContext.KeyContext.HasKey = HasKey;
        enemy.InteractionContext.KeyContext.KeyInstanceId = KeyInstanceId;
    }
}