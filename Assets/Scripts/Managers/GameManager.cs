using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour, IService
{
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private LoadingManager loadingManager;

    private ServicesManager servicesManager;
    private void Awake()
    {
        servicesManager = new ServicesManager();
        servicesManager.Init();

        servicesManager.Register(this);
        servicesManager.Register(loadingManager);
        servicesManager.Register(playerManager);
        servicesManager.Register(levelManager);
        servicesManager.Register(audioManager);
        servicesManager.Register(uiManager);

        StartCoroutine(InitRoutine());
    }
    public void Restart()
    {
        levelManager.StartGame(playerManager.CurrentLevel);
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

    public IEnumerator Init(Action<float, string> progress)
    {
        yield return null;
    }
}
