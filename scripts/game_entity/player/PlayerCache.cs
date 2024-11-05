namespace project768.scripts.player;

public struct PlayerCache
{
    public bool DownPressed { get; set; }
    public bool UpPressed { get; set; }
    public bool LeftPressed { get; set; }
    public bool RightPressed { get; set; }
    public bool JumpPressed { get; set; }
    public bool LeftClickPressed { get; set; }
    public float VerticalDirection { get; set; }
    public float HorizontalDirection { get; set; }
}