namespace project768.scripts.common;

using System;
using System.Collections.Generic;

public class FixedSizeStack<T>
{
    private readonly LinkedList<T> _stack;
    private readonly int _maxSize;

    public FixedSizeStack(int maxSize)
    {
        if (maxSize <= 0)
            throw new ArgumentException("Stack size must be greater than zero.", nameof(maxSize));

        _stack = new LinkedList<T>();
        _maxSize = maxSize;
    }

    public void Push(T item)
    {
        if (_stack.Count >= _maxSize)
        {
            // Optional: Remove the oldest item if exceeding the maximum size
            _stack.RemoveLast();
        }
        _stack.AddFirst(item);
    }

    public T Pop()
    {
        if (_stack.Count == 0)
            throw new InvalidOperationException("Stack is empty.");

        T value = _stack.First.Value;
        _stack.RemoveFirst();
        return value;
    }

    public T Peek()
    {
        if (_stack.Count == 0)
            throw new InvalidOperationException("Stack is empty.");

        return _stack.First.Value;
    }

    public int Count => _stack.Count;

    public bool IsEmpty => _stack.Count == 0;
}