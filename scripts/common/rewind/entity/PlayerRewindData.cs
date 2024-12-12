﻿using Godot;
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

    public PlayerRewindData(Player player)
    {
        Position = player.Position;
        Velocity = player.Velocity;
        CurrentState = player.CurrentState.StateEnum;
        Visible = player.Visible;
        Ladder = player.InteractionContext.Ladder;
        // Key
        HasKey = player.InteractionContext.HasKey;
        KeyInstanceId = player.InteractionContext.KeyInstanceId;
    }

    public void ApplyData(Player player)
    {
        player.Velocity = Velocity;
        player.Position = Position;
        player.RewindState = (int) CurrentState;
        player.InteractionContext.Ladder = Ladder;
        player.Visible = Visible;
        // Key
        player.InteractionContext.HasKey = HasKey;
        player.InteractionContext.KeyInstanceId = KeyInstanceId;
    }
}