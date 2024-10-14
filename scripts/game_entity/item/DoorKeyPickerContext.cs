namespace project768.scripts.item;

public class DoorKeyPickerContext
{
    public DoorKeyEvent DoorKeyEvent = DoorKeyEvent.None;
    public bool HasKey { get; set; }
    public ulong KeyInstanceId { get; set; }

    public void PutEvent(DoorKeyEvent doorKeyEvent)
    {
        DoorKeyEvent = doorKeyEvent;
    }

    public DoorKeyEvent ConsumeEvent()
    {
        var ev = DoorKeyEvent;
        DoorKeyEvent = DoorKeyEvent.None;
        return ev;
    }
}