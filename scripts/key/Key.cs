using Godot;
using project768.scripts.item;
using project768.scripts.key;
using project768.scripts.rewind.entity;
using project768.scripts.state_machine;

public partial class Key :
    RigidBody2D,
    Rewindable,
    IStateMachineEntity<Key, Key.State>
{
    public enum State
    {
        Unpicked,
        Picked,
        Rewind
    }

    public int RewindState { get; set; }
    public State<Key, State> CurrentState { get; set; }
    public State<Key, State>[] States { get; set; }
    public StateChanger<Key, State> StateChanger { get; set; }
    public DoorKeyPicker Picker { get; set; }

    public override void _Ready()
    {
        States = new State<Key, State>[]
        {
            new UnpickedState(this, State.Unpicked),
            new PickedState(this, State.Picked),
            new RewindState(this, State.Rewind),
        };
        StateChanger = new StateChanger<Key, State>(this);
        StateChanger.ChangeState(State.Unpicked);

        var pickerArea = GetNode<Area2D>("picker");
        pickerArea.BodyEntered += PickerArea_BodyEntered;
    }

    public override void _PhysicsProcess(double delta)
    {
        CurrentState.PhysicProcess(delta);
    }

    private void PickerArea_BodyEntered(Node2D body)
    {
        if (Picker != null) return;

        if (body is DoorKeyPicker itemPicker && !itemPicker.DoorKeyPickerContext.HasKey)
        {
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
        StateChanger.ChangeState((State) RewindState);
    }
}