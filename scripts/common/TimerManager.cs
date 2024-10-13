using System;

namespace project768.scripts.common;

public class TimerManager
{
    private double _timer = 0.0f;
    private double _targetTime;

    public TimerManager(double targetTime)
    {
        _targetTime = targetTime;
    }

    public void Reset()
    {
        _timer = 0.0f;
    }

    public bool Update(double delta)
    {
        if (_timer >= _targetTime)
        {
            return true;
        }

        _timer += delta;
        return false;
    }
}