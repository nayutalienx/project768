using Godot;
using project768.scripts.common;
using project768.scripts.game_entity.common;
using project768.scripts.game_entity.common.system;
using project768.scripts.game_entity.landscape.switcher.interaction;
using project768.scripts.player.interaction;
using project768.scripts.state_machine;

namespace project768.scripts.player;

public class MoveState : BasePlayerState
{
    public MoveState(Player entity, Player.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void HandleInput(InputEvent _event)
    {
        Entity.HandleInput(_event);
    }

    public override void EnterState(Player.State prevState)
    {
        RecoverKeyOnEnterState(prevState);
        RecoverSwitcherOnEnterState(prevState);
        RecoverSceneLoader(prevState);
        Entity.EnableCollision(Entity.OrigCollission);
        if (prevState == Player.State.Rewind)
        {
            Entity.CleanCache();
        }
    }

    public override void PhysicProcess(double delta)
    {
        if ((Entity.Cache.DownPressed || Entity.Cache.UpPressed) &&
            Entity.InteractionContext.LadderContext.LadderGlobalPosition != Vector2.Zero)
        {
            Entity.StateChanger.ChangeState(Player.State.Ladder);
            return;
        }

        if (Entity.Cache.UpPressed && Entity.InteractionContext.SceneLoaderContext.SceneLoader != null)
        {
            SaveSystem.Instance.SaveGame(Entity.GetTree());
            Player.PreviousSceneData.HasData = true;
            Player.PreviousSceneData.SpawnPositionIndex =
                Entity.InteractionContext.SceneLoaderContext.SceneLoader.SpawnPositionIndex;
            Entity.InteractionContext.SceneLoaderContext.SceneLoader.LoadDeferred();
        }

        ProcessKey();
        ProcessSwitcher();

        if (!Entity.IsOnFloor())
        {
            Entity.Velocity += Entity.GetGravity() * (float) delta;
        }

        if (Entity.IsOnFloor())
        {
            Entity.JumpMultiplier = 1.0f;
            if (Entity.Cache.JumpPressed)
            {
                Entity.Velocity = Entity.Velocity with {Y = Entity.JumpVelocity};
            }
        }

        float direction = Entity.Cache.HorizontalDirection;

        Entity.Velocity = Entity.Velocity with {X = direction * Entity.MoveSpeed};
        if (direction == 0)
        {
            Entity.Velocity = Entity.Velocity.MoveToward(
                Entity.Velocity with {X = 0}, Entity.MoveSpeed
            );
        }

        if (Entity.Cache.DownPressed)
        {
            Entity.GlobalPosition = Entity.GlobalPosition with {Y = Entity.GlobalPosition.Y + 1};
        }

        Entity.MoveAndSlide();
    }

    public override void OnBodyEntered(CollisionBody body)
    {
        if (body.Body is Key key)
        {
            Entity.Interactor.Interact(new PlayerInteractionEvent(PlayerInteraction.TryPickupKey)
            {
                KeyEvent = new PlayerKeyEvent()
                {
                    Key = key
                }
            });
        }

        if (body.Body is Switcher switcher)
        {
            Entity.Interactor.Interact(new PlayerInteractionEvent(PlayerInteraction.SwitcherArea)
            {
                SwitcherEvent = new PlayerSwitcherEvent
                {
                    JoinedSwitcherArea = true,
                    Switcher = switcher
                }
            });
        }

        if (body.Body is SceneLoader sceneLoader)
        {
            Entity.InteractionContext.SceneLoaderContext.SceneLoader = sceneLoader;
            Entity.InteractionContext.SceneLoaderContext.InstanceId = sceneLoader.GetInstanceId();
            Entity.Label.Text = "enter(up)";
        }
    }

    public override void OnBodyExited(CollisionBody body)
    {
        if (body.Body is Switcher switcher)
        {
            Entity.Interactor.Interact(new PlayerInteractionEvent(PlayerInteraction.SwitcherArea)
            {
                SwitcherEvent = new PlayerSwitcherEvent
                {
                    JoinedSwitcherArea = false,
                    Switcher = switcher
                }
            });
        }

        if (body.Body is SceneLoader)
        {
            Entity.InteractionContext.SceneLoaderContext.SceneLoader = null;
            Entity.InteractionContext.SceneLoaderContext.InstanceId = 0;
            Entity.Label.Text = "";
        }
    }

    private void ProcessSwitcher()
    {
        if (Entity.InteractionContext.SwitcherContext.JoinedSwitcherArea)
        {
            if (Entity.Cache.UpPressed)
            {
                Entity.InteractionContext.SwitcherContext.Switcher.Interactor.Interact(new SwitcherInteractionEvent(SwitcherInteraction.Toggle));
            }
        }
    }
}