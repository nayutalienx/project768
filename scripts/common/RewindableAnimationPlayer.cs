using System;
using Godot;

namespace project768.scripts.common;

public class RewindableAnimationPlayer
{
    public AnimationPlayer AnimationPlayer { get; set; }
    public string[] Animations { get; set; }
    public int CurrentAnimationIndex { get; set; }
    public string CurrentAnimation => Animations[CurrentAnimationIndex];

    private int currentSpeed;

    public RewindableAnimationPlayer(
        AnimationPlayer animationPlayer,
        string[] animations
    )
    {
        AnimationPlayer = animationPlayer;
        Animations = animations;

        AnimationPlayer.AnimationStarted += name => { CurrentAnimationIndex = GetAnimationIndex(name); };
    }

    public void SyncRewind(int currentAnimationIndex)
    {
        if (CurrentAnimationIndex != currentAnimationIndex)
        {
            CurrentAnimationIndex = currentAnimationIndex;
            AnimationPlayer.SetCurrentAnimation(Animations[currentAnimationIndex]);
        }
    }

    public void Play(string animation)
    {
        CurrentAnimationIndex = GetAnimationIndex(animation);
        AnimationPlayer.Play(animation);
    }

    public void InvertAndPlay(string animation)
    {
        if (AnimationPlayer.IsPlaying())
        {
            double currentPos = AnimationPlayer.CurrentAnimationPosition;
            CurrentAnimationIndex = GetAnimationIndex(animation);
            AnimationPlayer.SetCurrentAnimation(Animations[CurrentAnimationIndex]);
            double newPos = AnimationPlayer.CurrentAnimationLength - currentPos;
            AnimationPlayer.Seek(newPos, true);
        }
        else
        {
            Play(animation);
        }
    }

    public void UpdateRewindSpeed(int speed)
    {
        AnimationPlayer.SpeedScale = Mathf.Abs(speed);
        currentSpeed = speed;
    }

    private int GetAnimationIndex(string animation)
    {
        for (var i = 0; i < Animations.Length; i++)
        {
            if (animation.Equals(Animations[i]))
            {
                return i;
            }
        }

        throw new Exception("GetAnimationIndex: animation not found: " + animation);
    }
}