using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour, IService
{
    [SerializeField] private Transform container;
    [SerializeField] private LevelItem[] levelItems;
    [SerializeField] private AudioClip victorySound;
    [SerializeField] private AudioClip loseSound;

    private LevelItem currentLevel;

    public UnityEvent<LevelItem> onStart = new UnityEvent<LevelItem>();
    public UnityEvent<bool> onEnd = new UnityEvent<bool>();
    public IEnumerator Init(Action<float, string> progress)
    {
        StartGame(0);
        yield return null;
    }

    public void StartGame(int id)
    {
        if (levelItems.Length > id)
        {
            if (currentLevel != null)
            {
                Destroy(currentLevel);
            }

            currentLevel = Instantiate(levelItems[id], container);
            currentLevel.Init();
            onStart?.Invoke(currentLevel);
        }
    }
    public void FinishGame(bool victory)
    {
        ServicesManager.Instance.Get<AudioManager>().Play(victory ? victorySound : loseSound);
        onEnd?.Invoke(victory);
    }
}
