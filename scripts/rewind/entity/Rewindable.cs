using System;

namespace project768.scripts.rewind.entity;

public interface IRewindable
{
    public int RewindState { get; set; }
    public void RewindStarted();
    public void RewindFinished();
}