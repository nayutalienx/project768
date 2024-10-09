using Godot;

namespace project768.scripts.rewind.entity;

public struct AnimationPlayerRewindData
{
    public bool IsAnimationPlaying { get; set; }
    public double AnimationPosition { get; set; }

    public AnimationPlayerRewindData(AnimationPlayer animationPlayer)
    {
        IsAnimationPlaying = animationPlayer.IsPlaying();
        if (IsAnimationPlaying)
        {
            AnimationPosition = animationPlayer.GetCurrentAnimationPosition();
        }
        else
        {
            AnimationPosition = 0.0f;
        }
    }

    public void ApplyData(AnimationPlayer animationPlayer)
    {
        if (IsAnimationPlaying)
        {
            animationPlayer.Seek(AnimationPosition, true);
            animationPlayer.Play();
        }
        else
        {
            if (animationPlayer.IsPlaying())
            {
                animationPlayer.Stop();
            }
        }
    }
}