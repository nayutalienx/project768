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

    // Key
    public bool HasKey { get; set; }
    public ulong KeyInstanceId { get; set; }

    // Switcher
    public bool NearSwitcher { get; set; }
    public ulong SwitcherInstanceId { get; set; }

    public PlayerRewindData(Player player)
    {
        Position = player.Position;
        Velocity = player.Velocity;
        CurrentState = player.CurrentState.StateEnum;
        Visible = player.Visible;
        Ladder = player.InteractionContext.LadderContext.Ladder;
        // Key
        HasKey = player.InteractionContext.KeyContext.HasKey;
        KeyInstanceId = player.InteractionContext.KeyContext.KeyInstanceId;
        // Switcher
        NearSwitcher = player.InteractionContext.SwitcherContext.JoinedSwitcherArea;
        SwitcherInstanceId = player.InteractionContext.SwitcherContext.InstanceId;
    }

    public void ApplyData(Player player)
    {
        player.Velocity = Velocity;
        player.Position = Position;
        player.RewindState = (int) CurrentState;
        player.InteractionContext.LadderContext.Ladder = Ladder;
        player.Visible = Visible;
        // Key
        player.InteractionContext.KeyContext.HasKey = HasKey;
        player.InteractionContext.KeyContext.KeyInstanceId = KeyInstanceId;
        // Switcher
        player.InteractionContext.SwitcherContext.JoinedSwitcherArea = NearSwitcher;
        player.InteractionContext.SwitcherContext.InstanceId = SwitcherInstanceId;
    }
}