using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ServicesManager
{
    public static ServicesManager Instance { get; private set; }

    private readonly Dictionary<string, IService> services = new Dictionary<string, IService>();

    public void Init()
    {
        Instance = this;
    }
    public IService[] GetServices()
    {
        return services.Values.ToArray();
    }

    public void Register<T>(T service) where T : IService
    {
        string key = typeof(T).Name;
        if (services.ContainsKey(key))
        {
            Debug.LogError($"Service '{key}' is already registered");
            return;
        }

        services.Add(key, service);
    }

    public void Unregister<T>(T service) where T : IService
    {
        string key = typeof(T).Name;
        if (!services.ContainsKey(key))
        {
            Debug.Log($"Service '{key}' not yet registered");
            return;
        }

        services.Remove(key);
    }
    public T Get<T>() where T : IService
    {
        string key = typeof(T).Name;

        if (!services.ContainsKey(key))
        {
            string msg = $"There is no '{key}' service";
            Debug.LogError(msg);
            throw new InvalidOperationException(msg);
        }

        return (T)services[key];
    }
}

public interface IService
{
    public IEnumerator Init(Action<float, string> progress);
}
