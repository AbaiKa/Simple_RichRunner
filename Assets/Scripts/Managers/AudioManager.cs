using System;
using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour, IService
{
    [SerializeField] private AudioSource audioSource;
    public IEnumerator Init(Action<float, string> progress)
    {
        yield return null;
    }

    public void Play(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
}
