using SRRPlayer;
using System;
using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IService
{
    [SerializeField] private Transform container;
    [SerializeField] private Player prefab;
    public Player Player { get; private set; }
    public int CurrentLevel {  get; private set; }
    public IEnumerator Init(Action<float, string> progress)
    {
        CurrentLevel = 0;
        Player = Instantiate(prefab, container);
        Player.Init();
        yield return null;
    }

    public void LevelUp()
    {
        CurrentLevel++;
    }
}
