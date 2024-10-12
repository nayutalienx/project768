using Godot;
using project768.scripts.rewind.entity;

namespace project768.scripts.rewind;

public partial class RewindAudioPlayer : Node2D, IRewindable
{
    public int RewindState { get; set; }
    private AudioStreamPlayer forwardPlayer;
    private AudioStreamPlayer backwardPlayer;

    private double audioLen;

    public override void _Ready()
    {
        forwardPlayer = GetNode<AudioStreamPlayer>("forward_player");
        backwardPlayer = GetNode<AudioStreamPlayer>("reverse_player");


        audioLen = forwardPlayer.GetStream().GetLength();
        forwardPlayer.Play();
    }

    public void RewindStarted()
    {
        double backwardPos = audioLen - forwardPlayer.GetPlaybackPosition();
        GD.Print($"backwardPos {backwardPos} = {audioLen} - {forwardPlayer.GetPlaybackPosition()}");
        forwardPlayer.Stop();
        backwardPlayer.Play();
        backwardPlayer.Seek((float) backwardPos);
    }

    public void RewindFinished()
    {
        double forwardPos = audioLen - backwardPlayer.GetPlaybackPosition();
        GD.Print($"forwardPos {forwardPos} = {audioLen} - {backwardPlayer.GetPlaybackPosition()}");
        backwardPlayer.Stop();
        forwardPlayer.Play();
        forwardPlayer.Seek((float) forwardPos);
    }
}