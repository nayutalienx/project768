using Godot;
using project768.scripts.game_entity.npc.enemy.interaction.data;

namespace project768.scripts.rewind.entity;

public struct JumpingEnemyRewindData
{
    public Vector2 GlobalPosition { get; set; }
    public Vector2 Velocity { get; set; }
    public bool Visible { get; set; }
    public JumpingEnemy.State CurrentState { get; set; }
    public Vector2 Direction { get; set; }

    public JumpingEnemyRewindData(JumpingEnemy enemy)
    {
        GlobalPosition = enemy.GlobalPosition;
        Velocity = enemy.Velocity;
        CurrentState = enemy.CurrentState.StateEnum;
        Visible = enemy.Visible;
        Direction = enemy.Direction;
    }

    public void ApplyData(JumpingEnemy enemy)
    {
        enemy.Velocity = Velocity;
        enemy.GlobalPosition = GlobalPosition;
        enemy.RewindState = (int) CurrentState;
        enemy.Visible = Visible;
        enemy.Direction = Direction;
    }
}