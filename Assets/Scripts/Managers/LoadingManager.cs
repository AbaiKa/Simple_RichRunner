using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour, IService
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Image fillImage;
    [SerializeField] private TextMeshProUGUI progressText;

    public IEnumerator Init(System.Action<float, string> progress)
    {
        StartFakeLoading();
        yield return null;
    }

    public void StartFakeLoading()
    {
        StartCoroutine(LoadingRoutine(Random.Range(3, 5)));
    }

    private IEnumerator LoadingRoutine(float duration)
    {
        panel.SetActive(true);

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / duration);
            progressText.text = $"{Mathf.Round(progress * 100)}%";
            fillImage.fillAmount = progress;
            yield return null;
        }

        panel.SetActive(false);
    }
}
