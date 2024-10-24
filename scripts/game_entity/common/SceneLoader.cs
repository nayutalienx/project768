using Godot;

namespace project768.scripts.game_entity.common;

public partial class SceneLoader : Node2D
{
    [Export] public string Path { get; set; }


    public void Load()
    {
        GetTree().ChangeSceneToFile(Path);
    }

    public void LoadDeferred()
    {
        CallDeferred(nameof(Load));
    }
}