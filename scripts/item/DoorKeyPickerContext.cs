namespace project768.scripts.item;

public class DoorKeyPickerContext
{
    private DoorKeyEvent doorKeyEvent = DoorKeyEvent.None;
    public bool HasKey { get; set; }

    public void PutEvent(DoorKeyEvent doorKeyEvent)
    {
        this.doorKeyEvent = doorKeyEvent;
    }

    public DoorKeyEvent ConsumeEvent()
    {
        var ev = doorKeyEvent;
        doorKeyEvent = DoorKeyEvent.None;
        return ev;
    }
}