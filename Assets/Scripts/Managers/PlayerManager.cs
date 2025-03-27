using SRRPlayer;
using System;
using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IService
{
    [SerializeField] private Transform container;
    [SerializeField] private Player prefab;
    public Player Player { get; private set; }
    public IEnumerator Init(Action<float, string> progress)
    {
        Player = Instantiate(prefab, container);
        Player.Init();
        yield return null;
    }
}
