using Godot;
using project768.scripts.player;

namespace project768.scripts.rewind.entity;

public struct PlayerRewindData
{
    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }
    public Vector2 Ladder { get; set; }
    public bool Visible { get; set; }
    public Player.State CurrentState { get; set; }

    public PlayerRewindData(Player player)
    {
        Position = player.Position;
        Velocity = player.Velocity;
        CurrentState = player.CurrentState.StateEnum;
        Ladder = player.Ladder;
        Visible = player.Visible;
    }

    public void ApplyData(Player player)
    {
        player.Velocity = Velocity;
        player.Position = Position;
        player.RewindState = (int) CurrentState;
        player.Ladder = Ladder;
        player.Visible = Visible;
    }
}