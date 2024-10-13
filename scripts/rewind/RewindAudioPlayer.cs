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

        backwardPlayer.Play();
        backwardPlayer.SetStreamPaused(true);
    }

    public void RewindStarted()
    {
        double backwardPos = audioLen - forwardPlayer.GetPlaybackPosition();
        GD.Print($"backwardPos {backwardPos} = {audioLen} - {forwardPlayer.GetPlaybackPosition()}");
        forwardPlayer.SetStreamPaused(true);
        backwardPlayer.SetStreamPaused(false);
        backwardPlayer.Seek((float) backwardPos);
    }

    public void RewindFinished()
    {
        double forwardPos = audioLen - backwardPlayer.GetPlaybackPosition();
        GD.Print($"forwardPos {forwardPos} = {audioLen} - {backwardPlayer.GetPlaybackPosition()}");
        backwardPlayer.SetStreamPaused(true);
        forwardPlayer.SetStreamPaused(false);
        forwardPlayer.Seek((float) forwardPos);
    }

    public void OnRewindSpeedChanged(int speed)
    {
        if (speed < 0)
        {
            return;
        }

        if (speed == 0)
        {
            backwardPlayer.SetStreamPaused(true);
        }
        else
        {
            if (backwardPlayer.GetStreamPaused())
            {
                backwardPlayer.SetStreamPaused(false);
            }

            backwardPlayer.PitchScale = speed;
        }
    }
}