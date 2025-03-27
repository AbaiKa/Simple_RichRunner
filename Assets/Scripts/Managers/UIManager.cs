using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, IService
{
    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private Button continueButton;

    private GameManager gameManager;
    private PlayerManager playerManager;
    public IEnumerator Init(Action<float, string> progress)
    {
        gameManager = ServicesManager.Instance.Get<GameManager>();
        playerManager = ServicesManager.Instance.Get<PlayerManager>();

        losePanel.SetActive(false);
        victoryPanel.SetActive(false);
        continueButton.gameObject.SetActive(false);
        continueButton.onClick.AddListener(OnContinue);
        yield return null;
    }
    public void GameOver(bool victory)
    {
        losePanel.SetActive(!victory);
        victoryPanel.SetActive(victory);
        continueButton.gameObject.SetActive(true);
    }
    private void OnContinue()
    {
        playerManager.LevelUp();
        gameManager.Restart();

        losePanel.SetActive(false);
        victoryPanel.SetActive(false);
    }
}
