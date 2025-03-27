using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private LevelManager levelManager;

    private ServicesManager servicesManager;
    private void Awake()
    {
        servicesManager = new ServicesManager();
        servicesManager.Init();

        servicesManager.Register(playerManager);
        servicesManager.Register(levelManager);

        StartCoroutine(InitRoutine());
    }
    private IEnumerator InitRoutine()
    {
        var services = servicesManager.GetServices();
        float loaded = 0;
        int total = services.Length;
        for (int i = 0; i < services.Length; i++)
        {
            yield return StartCoroutine(InitializeService(services[i], progress =>
            {
                float value = (loaded / total) + (progress / total);
            }));
            loaded++;
        }
    }
    private IEnumerator InitializeService(IService service, Action<float> progressCallback)
    {
        yield return StartCoroutine(service.Init((progress, message) =>
        {
            progressCallback(progress);
        }));
    }
}
