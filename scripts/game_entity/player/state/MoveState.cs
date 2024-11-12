using Godot;
using project768.scripts.common;
using project768.scripts.game_entity.common;
using project768.scripts.game_entity.common.system;
using project768.scripts.game_entity.landscape.switcher.interaction;
using project768.scripts.game_entity.landscape.timeless_switcher.interaction.data;
using project768.scripts.player.interaction;

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
        Entity.EnableCollision(Entity.OrigCollission);
        if (prevState == Player.State.Rewind)
        {
            Entity.CleanCache();
        }
    }

    public override void PhysicProcess(double delta)
    {
        if (StateChangedToLadder())
        {
            return;
        }

        ProcessSceneLoad();
        ProcessKey();
        ProcessTimelessKey();
        ProcessSwitchers();

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

    private bool StateChangedToLadder()
    {
        if ((Entity.Cache.DownPressed || Entity.Cache.UpPressed))
        {
            Area2D areaLadder = Entity.InteractionArea.IsOverlappingAreaWithLayer(GameCollisionLayer.Ladder);
            if (areaLadder != null)
            {
                Entity.InteractionContext.LadderContext.LadderGlobalPosition = areaLadder.GlobalPosition;
                Entity.StateChanger.ChangeState(Player.State.Ladder);
                return true;
            }
        }

        return false;
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

        if (body.Body is TimelessKey timelessKey)
        {
            Entity.Interactor.Interact(new PlayerInteractionEvent(PlayerInteraction.TryPickupTimelessKey)
            {
                TimelessKeyEvent = new PlayerTimelessKeyEvent()
                {
                    Key = timelessKey
                }
            });
        }


        if (body.Body is SceneLoader sceneLoader)
        {
            Entity.Label.Text = "enter(up)";
        }
    }

    public override void OnBodyExited(CollisionBody body)
    {
        if (body.Body is SceneLoader)
        {
            Entity.Label.Text = "";
        }
    }

    private void ProcessSwitchers()
    {
        if (Entity.Cache.UpPressed)
        {
            Node2D node = Entity.InteractionArea.IsOverlappingBodyWithLayer(GameCollisionLayer.Switcher);
            if (node is Switcher switcher)
            {
                switcher.Interactor.Interact(
                    new SwitcherInteractionEvent(SwitcherInteraction.Toggle));
            }

            if (node is TimelessSwitcher timelessSwitcher)
            {
                timelessSwitcher.Interactor.Interact(
                    new TimelessSwitcherInteractionEvent(TimelessSwitcherInteraction.Toggle));
            }
        }
    }

    private void ProcessSceneLoad()
    {
        if (Entity.Cache.UpPressed)
        {
            Node2D node = Entity.InteractionArea.IsOverlappingBodyWithLayer(GameCollisionLayer.SceneLoader);
            if (node is SceneLoader sceneLoader)
            {
                SaveSystem.Instance.SaveGame(Entity.GetTree());
                Player.PreviousSceneData.HasData = true;
                Player.PreviousSceneData.SpawnPositionIndex =
                    sceneLoader.SpawnPositionIndex;
                sceneLoader.LoadDeferred();
            }
        }
    }
}