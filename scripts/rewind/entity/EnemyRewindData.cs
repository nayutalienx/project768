using Godot;

namespace project768.scripts.rewind.entity;

public struct EnemyRewindData
{
    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }
    public bool Visible { get; set; }
    public Enemy.State CurrentState { get; set; }

    public EnemyRewindData(Enemy enemy)
    {
        Position = enemy.Position;
        Velocity = enemy.Velocity;
        CurrentState = enemy.CurrentState.StateEnum;
        Visible = enemy.Visible;
    }

    public void ApplyData(Enemy enemy)
    {
        enemy.Velocity = Velocity;
        enemy.Position = Position;
        enemy.RewindState = (int) CurrentState;
        enemy.Visible = Visible;
    }
}