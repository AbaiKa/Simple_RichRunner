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
        public void SetInfo(PlayerState state, int current, int max)
        {
            float progress = (float)current / max;

            var targetColor = Color.Lerp(startColor, endColor, progress);

            statusTextComponent.text = state.ToString();
            fillImageComponent.fillAmount = progress;
            statusTextComponent.color = targetColor;
            fillImageComponent.color = targetColor;
        }
    }
}
