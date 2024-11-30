using Godot;
using project768.scripts.game_entity.npc.enemy.interaction.data;

namespace project768.scripts.rewind.entity;

public struct EnemySpacetimeRewindData
{
    public Vector2 GlobalPosition { get; set; }
    public EnemySpacetime.State CurrentState { get; set; }

    // Key
    public bool HasKey { get; set; }
    public ulong KeyInstanceId { get; set; }
    
    public EnemySpacetimeRewindData(EnemySpacetime enemy)
    {
        GlobalPosition = enemy.GlobalPosition;
        
        CurrentState = enemy.CurrentState.StateEnum;
        // key
        HasKey = enemy.InteractionContext.KeyContext.HasKey;
        KeyInstanceId = enemy.InteractionContext.KeyContext.KeyInstanceId;
    }

    public void ApplyData(EnemySpacetime enemy)
    {
        
        enemy.GlobalPosition = GlobalPosition;
        enemy.RewindState = (int) CurrentState;
        // key
        enemy.InteractionContext.KeyContext.HasKey = HasKey;
        enemy.InteractionContext.KeyContext.KeyInstanceId = KeyInstanceId;
    }
}