using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using project768.scripts.common;
using project768.scripts.common.interaction;
using project768.scripts.game_entity.common.system;
using project768.scripts.game_entity.landscape.cannon;
using project768.scripts.player.interaction;
using project768.scripts.rewind.entity;
using project768.scripts.state_machine;

namespace project768.scripts.player;

public partial class Player :
    CharacterBody2D,
    IRewindable,
    IStateMachineEntity<Player, Player.State>,
    IInteractableEntity<Player, PlayerInteractionContext, PlayerInteractionEvent, PlayerInteraction>
{
    public static PreviousSceneData PreviousSceneData { get; set; } = new();

    public List<Node2D> SpawnPositions { get; set; } = new();

    [ExportSubgroup("Player Settings")] [Export]
    public float JumpVelocity = -400.0f;

    [Export] public float MoveSpeed = 300.0f;

    [Export] public float PushForce = 80.0f;
    public float JumpMultiplier { get; set; } = 1.0f;

    public enum State
    {
        Move,
        Ladder,
        Death,
        Rewind
    }

    public Dictionary<PlayerInteraction, Interaction<Player, PlayerInteractionEvent, PlayerInteraction>> Interactions
    {
        get;
        set;
    }

    public Interactor<Player, PlayerInteractionContext, PlayerInteractionEvent, PlayerInteraction> Interactor
    {
        get;
        set;
    }

    public PlayerInteractionContext InteractionContext { get; set; } = new();
    public int RewindState { get; set; }
    public State<Player, State> CurrentState { get; set; }
    public Dictionary<State, State<Player, State>> States { get; set; }
    public StateChanger<Player, State> StateChanger { get; set; }
    public PlayerCache Cache { get; set; }

    public Tuple<uint, uint> OrigCollission;
    public Label Label { get; set; }

    public override void _Ready()
    {
        Label = GetNode<Label>("Label");

        Interactions =
            new Dictionary<PlayerInteraction, Interaction<Player, PlayerInteractionEvent, PlayerInteraction>>()
            {
                {PlayerInteraction.LadderArea, new LadderAreaInteraction(this)},
                {PlayerInteraction.KillPlayer, new KillPlayerInteraction(this)},
                {PlayerInteraction.FallOnEnemyHead, new FallOnEnemyInteraction(this)},
                {PlayerInteraction.TryPickupKey, new TryPickupKeyInteraction(this)},
                {PlayerInteraction.UnlockedDoor, new DoorUnlockedInteraction(this)},
                {PlayerInteraction.SwitcherArea, new SwitcherAreaInteraction(this)},
                {PlayerInteraction.TryPickupTimelessKey, new TryPickupTimelessKeyInteraction(this)},
            };
        Interactor = new(this);

        States = new Dictionary<State, State<Player, State>>()
        {
            {State.Move, new MoveState(this, State.Move)},
            {State.Ladder, new LadderState(this, State.Ladder)},
            {State.Death, new DeathState(this, State.Death)},
            {State.Rewind, new RewindState(this, State.Rewind)},
        };
        StateChanger = new StateChanger<Player, State>(this);
        OrigCollission = this.GetCollisionLayerMask();

        StateChanger.ChangeState(State.Move);

        var area2d = GetNode<Area2D>("Area2D");
        area2d.BodyEntered += body => CurrentState.OnBodyEntered(new CollisionBody("player", body));
        area2d.BodyExited += body => CurrentState.OnBodyExited(new CollisionBody("player", body));
        
        foreach (var child in GetChildren())
        {
            if (child.Name == "spawn")
            {
                SpawnPositions.AddRange(child.GetChildren().ToList().ConvertAll(c => c as Node2D));
                break;
            }
        }

        LoadPreviousSceneData();
        SaveSystem.Instance.LoadGame(GetTree());
    }

    public override void _Input(InputEvent _event)
    {
        CurrentState.HandleInput(_event);
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Cache.LeftClickPressed)
        {
            var pos = GetGlobalMousePosition();
            GlobalPosition = pos;
            Velocity = Vector2.Zero;
        }

        CurrentState.PhysicProcess(delta);

        // Label.Text = $"hor-dir: {Cache.HorizontalDirection}\n" +
        //              $"onfloor: {IsOnFloor()}\n" +
        //              $"velocity: {Velocity}\n"+
        //              $"floor-ang: {GetFloorAngle()}\n"
        //     ;
    }

    public void HandleInput(InputEvent _event)
    {
        var cache = Cache;
        cache.VerticalDirection = Input.GetAxis("ui_up", "ui_down");
        cache.HorizontalDirection = Input.GetAxis("ui_left", "ui_right");

        cache.UpPressed = _event.IsActionPressed("ui_up");
        cache.DownPressed = _event.IsActionPressed("ui_down");
        cache.LeftPressed = _event.IsActionPressed("ui_left");
        cache.RightPressed = _event.IsActionPressed("ui_right");
        cache.JumpPressed = _event.IsActionPressed("ui_accept");
        cache.LeftClickPressed = Input.IsActionPressed("left_click");

        Cache = cache;
    }

    public void CleanCache()
    {
        Cache = new PlayerCache();
    }

    private void ApplyImpulseToRigidBodies()
    {
        // Player impulse to rigid bodies
        for (int i = 0; i < GetSlideCollisionCount(); i++)
        {
            KinematicCollision2D c = GetSlideCollision(i);
            if (c.GetCollider() is RigidBody2D)
            {
                ((RigidBody2D) c.GetCollider()).ApplyCentralImpulse(
                    -c.GetNormal() * PushForce);
            }
        }
    }

    private void LoadPreviousSceneData()
    {
        if (PreviousSceneData.HasData)
        {
            Camera2D camera = GetNode<Camera2D>("Camera2D");
            camera.SetPositionSmoothingEnabled(false);
            GlobalPosition = SpawnPositions[PreviousSceneData.SpawnPositionIndex].GlobalPosition;
            camera.ResetSmoothing();
            camera.SetPositionSmoothingEnabled(true);
            PreviousSceneData.HasData = false;
        }
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
    }
}