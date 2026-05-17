using System;
using System.Collections.Generic;

public static class ServiceLocator
{
    private static readonly Dictionary<Type, object> services = new();

    public static void Register<T>(T service)
    {
        var type = typeof(T);

        if (services.ContainsKey(type))
        {
            UnityEngine.Debug.LogWarning($"Service {type.Name} already registered");
            return;
        }

        services[type] = service;
    }

    public static T Get<T>()
    {
        var type = typeof(T);

        if (services.TryGetValue(type, out var service))
        {
            return (T)service;
        }

        throw new Exception($"Service {type.Name} not found");
    }

    public static bool TryGet<T>(out T service)
    {
        if (services.TryGetValue(typeof(T), out var obj))
        {
            service = (T)obj;
            return true;
        }

        service = default;
        return false;
    }

    public static void Unregister<T>()
    {
        services.Remove(typeof(T));
    }
}