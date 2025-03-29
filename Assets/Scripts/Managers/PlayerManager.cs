using SRRPlayer;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour, IService
{
    [SerializeField] private Transform container;
    [SerializeField] private Player prefab;

    public UnityEvent<Player> onInit = new UnityEvent<Player>();

    public Player Player { get; private set; }
    public int CurrentLevel {  get; private set; }
    public IEnumerator Init(Action<float, string> progress)
    {
        CurrentLevel = 0;
        Player = Instantiate(prefab, container);
        Player.Init();

        onInit?.Invoke(Player);
        yield return null;
    }

    public void LevelUp()
    {
        CurrentLevel++;
    }
}
