using System;

namespace project768.scripts.common;

public class TimerManager
{
    public double CurrentTime { get; set; }
    private double _targetTime;

    public TimerManager(double targetTime)
    {
        _targetTime = targetTime;
    }

    public void Reset()
    {
        CurrentTime = 0.0f;
    }

    public bool IsExpired()
    {
        return CurrentTime >= _targetTime;
    }

    public bool Update(double delta)
    {
        if (IsExpired())
        {
            return true;
        }

        CurrentTime += delta;
        return false;
    }
}