using Godot;
using project768.scripts.item;
using project768.scripts.key;
using project768.scripts.rewind.entity;
using project768.scripts.state_machine;

public partial class Key :
    RigidBody2D,
    IRewindable,
    IStateMachineEntity<Key, Key.State>
{
    public enum State
    {
        Unpicked,
        Picked,
        Used,
        Rewind
    }

    public int RewindState { get; set; }
    public State<Key, State> CurrentState { get; set; }
    public State<Key, State>[] States { get; set; }
    public StateChanger<Key, State> StateChanger { get; set; }
    public ulong PickerInstanceId { get; set; } = 0;
    public DoorKeyPicker Picker { get; set; }
    public Area2D PickerArea { get; set; }

    public override void _Ready()
    {
        States = new State<Key, State>[]
        {
            new UnpickedState(this, State.Unpicked),
            new PickedState(this, State.Picked),
            new UsedState(this, State.Used),
            new RewindState(this, State.Rewind),
        };
        StateChanger = new StateChanger<Key, State>(this);
        StateChanger.ChangeState(State.Unpicked);

        PickerArea = GetNode<Area2D>("picker");
        PickerArea.BodyEntered += PickerArea_BodyEntered;
    }

    public override void _PhysicsProcess(double delta)
    {
        CurrentState.PhysicProcess(delta);
    }

    private void PickerArea_BodyEntered(Node2D body)
    {
        if (CurrentState.StateEnum != State.Unpicked) return;

        if (body is DoorKeyPicker itemPicker && !itemPicker.DoorKeyPickerContext.HasKey)
        {
            PickerInstanceId = body.GetInstanceId();
            Picker = itemPicker;
            Picker.DoorKeyPickerContext.HasKey = true;
            StateChanger.ChangeState(State.Picked);
        }
    }

    public void RewindStarted()
    {
        StateChanger.ChangeState(State.Rewind);
    }

    public void RewindFinished()
    {
        if (PickerInstanceId != 0)
        {
            Picker = InstanceFromId(PickerInstanceId) as DoorKeyPicker;
        }

        StateChanger.ChangeState((State) RewindState);
    }
}