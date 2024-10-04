using System;

namespace project768.scripts.rewind.entity;

public interface Rewindable
{
    public int RewindState { get; set; }
    public void RewindStarted();
    public void RewindFinished();
}