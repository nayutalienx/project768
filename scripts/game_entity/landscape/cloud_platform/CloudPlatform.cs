using System;
using System.Collections.Generic;
using Godot;
using project768.scripts.common;
using project768.scripts.game_entity.npc.spawner;
using project768.scripts.rewind.entity;
using project768.scripts.state_machine;

namespace project768.scripts.game_entity.landscape.cloud_platform;

public partial class CloudPlatform : AnimatableBody2D,
    IRewindable,
    ISpawnable,
    IStateMachineEntity<CloudPlatform, CloudPlatform.State>
{
    public float Speed { get; set; } = 200.0f;

    public enum State
    {
        Wait,
        Move,
        Rewind
    }

    public State<CloudPlatform, State> CurrentState { get; set; }
    public Dictionary<State, State<CloudPlatform, State>> States { get; set; }
    public StateChanger<CloudPlatform, State> StateChanger { get; set; }
    public int RewindState { get; set; }
    public Sprite2D Sprite { get; set; }
    public GpuParticles2D Particles { get; set; }
    public RayCast2D RayCast2D { get; set; }
    public CollisionShape2D CollisionShape2D { get; set; }

    public Vector2 Direction { get; set; } = Vector2.Right;

    public bool CloudHidden
    {
        get => cloudHidden;
        set
        {
            if (value)
            {
                HideCloud();
            }
            else
            {
                ShowCloud();
            }
        }
    }

    private bool cloudHidden = true;
    private Tuple<uint, uint> collissionLayerMask;

    public override void _Ready()
    {
        Sprite = GetNode<Sprite2D>("Sprite2D");
        Particles = GetNode<GpuParticles2D>("GPUParticles2D");

        States = new Dictionary<State, State<CloudPlatform, State>>()
        {
            {State.Wait, new WaitState(this, State.Wait)},
            {State.Move, new MoveState(this, State.Move)},
            {State.Rewind, new RewindState(this, State.Rewind)},
        };
        StateChanger = new StateChanger<CloudPlatform, State>(this);

        RayCast2D = GetNode<RayCast2D>("RayCast2D");
        CollisionShape2D = GetNode<CollisionShape2D>("CollisionShape2D");

        collissionLayerMask = this.GetCollisionLayerMask();


        StateChanger.ChangeState(State.Wait);
    }

    public void HideCloud()
    {
        this.DisableCollision();
        RayCast2D.SetEnabled(false);

        cloudHidden = true;
        Sprite.Hide();
        Particles.SetEmitting(false);
    }

    public void ShowCloud()
    {
        this.EnableCollision(collissionLayerMask);
        RayCast2D.SetEnabled(true);

        cloudHidden = false;
        Sprite.Show();
        Particles.SetEmitting(true);
    }

    public override void _PhysicsProcess(double delta)
    {
        CurrentState.PhysicProcess(delta);
    }

    public void RewindStarted()
    {
        StateChanger.ChangeState(State.Rewind);
    }

    public void RewindFinished()
    {
        StateChanger.ChangeState((State) RewindState);
    }

    public void OnRewindSpeedChanged(int speed)
    {
        if (speed == 0)
        {
            Particles.SetEmitting(false);
        }
        else
        {
            Particles.SetEmitting(true);
            Particles.SpeedScale = Mathf.Abs(speed);
        }
    }

    public bool CanSpawn()
    {
        return CurrentState.StateEnum == State.Wait;
    }

    public bool TrySpawn(Vector2 spawnPosition, Vector2 direction)
    {
        if (CanSpawn())
        {
            Direction = direction;
            GlobalPosition = spawnPosition;
            StateChanger.ChangeState(State.Move);
            return true;
        }

        return false;
    }
}