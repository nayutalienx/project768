using System.Linq;

namespace project768.scripts.common;

using System;
using System.Collections.Generic;

public class FixedSizeStack<T>
{
    private readonly List<T> _stack;
    private readonly int _maxSize;

    public FixedSizeStack(int maxSize)
    {
        if (maxSize <= 0)
            throw new ArgumentException("Stack size must be greater than zero.", nameof(maxSize));

        _stack = new List<T>(maxSize);
        _maxSize = maxSize;
    }

    public void Push(T item)
    {
        if (_stack.Count >= _maxSize)
        {
            // Optional: Remove the oldest item if exceeding the maximum size
            _stack.RemoveAt(0);
        }

        _stack.Add(item);
    }

    public T Pop()
    {
        if (_stack.Count == 0)
            throw new InvalidOperationException("Stack is empty.");

        T value = _stack.Last();
        _stack.RemoveAt(_stack.Count - 1);
        return value;
    }

    public int Count => _stack.Count;

    public bool IsEmpty => _stack.Count == 0;
}