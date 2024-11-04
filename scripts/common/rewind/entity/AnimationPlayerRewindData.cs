using Godot;
using project768.scripts.common;

namespace project768.scripts.rewind.entity;

public struct AnimationPlayerRewindData
{
    public bool IsAnimationPlaying { get; set; }
    public double AnimationPosition { get; set; }
    public int CurrentAnimationIndex { get; set; }

    public AnimationPlayerRewindData(RewindableAnimationPlayer animationPlayer)
    {
        IsAnimationPlaying = animationPlayer.AnimationPlayer.IsPlaying();
        if (IsAnimationPlaying)
        {
            AnimationPosition = animationPlayer.AnimationPlayer.GetCurrentAnimationPosition();
        }
        else
        {
            AnimationPosition = 0.0f;
        }

        CurrentAnimationIndex = animationPlayer.CurrentAnimationIndex;
    }

    public void ApplyData(RewindableAnimationPlayer animationPlayer)
    {
        
        animationPlayer.SyncRewind(CurrentAnimationIndex);
        
        if (IsAnimationPlaying)
        {
            animationPlayer.AnimationPlayer.Seek(AnimationPosition, true);
            animationPlayer.AnimationPlayer.Play();
        }
        else
        {
            if (animationPlayer.AnimationPlayer.IsPlaying())
            {
                animationPlayer.AnimationPlayer.Pause();
            }
        }
    }
}