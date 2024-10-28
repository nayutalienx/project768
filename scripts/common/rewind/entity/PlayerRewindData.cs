using Godot;
using project768.scripts.player;

namespace project768.scripts.rewind.entity;

public struct PlayerRewindData
{
    public Vector2 GlobalPosition { get; set; }
    public Vector2 Velocity { get; set; }
    public Vector2 LadderGlobalPosition { get; set; }
    public bool Visible { get; set; }
    public Player.State CurrentState { get; set; }
    public float JumpMultiplier { get; set; }

    // Key
    public bool HasKey { get; set; }
    public ulong KeyInstanceId { get; set; }

    // Switcher
    public bool NearSwitcher { get; set; }
    public ulong SwitcherInstanceId { get; set; }
    public ulong SceneLoaderInstanceId { get; set; }

    public PlayerRewindData(Player player)
    {
        GlobalPosition = player.GlobalPosition;
        Velocity = player.Velocity;
        CurrentState = player.CurrentState.StateEnum;
        Visible = player.Visible;
        LadderGlobalPosition = player.InteractionContext.LadderContext.LadderGlobalPosition;
        JumpMultiplier = player.JumpMultiplier;
        // Key
        HasKey = player.InteractionContext.KeyContext.HasKey;
        KeyInstanceId = player.InteractionContext.KeyContext.KeyInstanceId;
        // Switcher
        NearSwitcher = player.InteractionContext.SwitcherContext.JoinedSwitcherArea;
        SwitcherInstanceId = player.InteractionContext.SwitcherContext.InstanceId;
        // SceneLoader
        SceneLoaderInstanceId = player.InteractionContext.SceneLoaderContext.InstanceId;
    }

    public void ApplyData(Player player)
    {
        player.Velocity = Velocity;
        player.GlobalPosition = GlobalPosition;
        player.RewindState = (int) CurrentState;
        player.InteractionContext.LadderContext.LadderGlobalPosition = LadderGlobalPosition;
        player.Visible = Visible;
        player.JumpMultiplier = JumpMultiplier;
        // Key
        player.InteractionContext.KeyContext.HasKey = HasKey;
        player.InteractionContext.KeyContext.KeyInstanceId = KeyInstanceId;
        // Switcher
        player.InteractionContext.SwitcherContext.JoinedSwitcherArea = NearSwitcher;
        player.InteractionContext.SwitcherContext.InstanceId = SwitcherInstanceId;
        // SceneLoader
        player.InteractionContext.SceneLoaderContext.InstanceId = SceneLoaderInstanceId;
    }
}