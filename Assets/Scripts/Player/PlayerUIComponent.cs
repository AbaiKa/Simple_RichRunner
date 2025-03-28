using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SRRPlayer
{
    public class PlayerUIComponent : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private GameObject panel;
        [SerializeField] private TextMeshProUGUI statusTextComponent;
        [SerializeField] private Image fillImageComponent;
        [Header("Properties")]
        [SerializeField] private Color startColor;
        [SerializeField] private Color endColor;
        public void SetActive(bool value)
        {
            panel.SetActive(value);
        }
        private Coroutine progressCoroutine;

        public void SetInfo(PlayerState state, int current, int max)
        {
            float targetProgress = (float)current / max;
            var targetColor = Color.Lerp(startColor, endColor, targetProgress);

            statusTextComponent.text = state.ToString();

            if (progressCoroutine != null)
                StopCoroutine(progressCoroutine);

            progressCoroutine = StartCoroutine(UpdateProgress(targetProgress, targetColor));
        }

        private IEnumerator UpdateProgress(float targetProgress, Color targetColor)
        {
            float startProgress = fillImageComponent.fillAmount;
            Color startColor = fillImageComponent.color;
            float duration = 0.5f; 
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / duration;

                fillImageComponent.fillAmount = Mathf.Lerp(startProgress, targetProgress, t);
                Color newColor = Color.Lerp(startColor, targetColor, t);

                statusTextComponent.color = newColor;
                fillImageComponent.color = newColor;

                yield return null;
            }

            fillImageComponent.fillAmount = targetProgress;
            statusTextComponent.color = targetColor;
            fillImageComponent.color = targetColor;
        }

    }
}
