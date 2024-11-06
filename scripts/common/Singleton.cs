using System;

namespace project768.scripts.common;

public class Singleton<T> where T : class, new()
{
    private static readonly Lazy<T> _instance = new Lazy<T>(() => new T());

    // Конструктор по умолчанию должен быть приватным, чтобы предотвратить создание экземпляра извне.
    protected Singleton() { }

    public static T Instance => _instance.Value;
}