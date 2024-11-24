using System.Linq;
using Godot;

namespace project768.scripts.game_entity.common;

public partial class SceneLoader : StaticBody2D
{
    [Export(PropertyHint.File, "*.tscn")] public string Path { get; set; }

    [Export] public int SpawnPositionIndex { get; set; }

    public Label Label;

    public override void _Ready()
    {
        Label = GetNode<Label>("Label");
        if (!string.IsNullOrEmpty(Path))
        {
            Label.Text = Path.Split("/").Last();
        }
    }

    public void Load()
    {
        GetTree().ChangeSceneToFile(Path);
    }

    public void LoadDeferred()
    {
        CallDeferred(nameof(Load));
    }
}