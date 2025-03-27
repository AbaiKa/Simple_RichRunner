using System.Collections;
using UnityEngine;

public class RotationAnimation : MonoBehaviour
{
    [SerializeField] private Vector3 targetRotation;
    [SerializeField] private float duration;
    public void Play()
    {
        StartCoroutine(RotateOverTime(targetRotation, duration));
    }
    private IEnumerator RotateOverTime(Vector3 euler, float duration)
    {
        float elapsedTime = 0f;
        Quaternion startRotation = transform.localRotation;
        Quaternion targetRotation = Quaternion.Euler(euler);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            transform.localRotation = Quaternion.Slerp(startRotation, targetRotation, t);
            yield return null;
        }

        transform.localRotation = targetRotation;
    }
}
