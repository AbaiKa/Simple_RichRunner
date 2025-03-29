using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, IService
{
    [SerializeField] private Canvas gameCanvas;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private Button continueButton;
    [SerializeField] private CoinsCollectComponent coinsCollect;

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
        ServicesManager.Instance.Register(coinsCollect);
        StartCoroutine(coinsCollect.Init(progress));
        yield return null;
    }
    private bool isWin = false;
    public void GameOver(bool victory)
    {
        isWin = victory;
        losePanel.SetActive(!victory);
        victoryPanel.SetActive(victory);
        continueButton.gameObject.SetActive(true);
    }
    private void OnContinue()
    {
        if (isWin)
            playerManager.LevelUp();
        gameManager.Restart();

        losePanel.SetActive(false);
        victoryPanel.SetActive(false);
        continueButton.gameObject.SetActive(false);
    }
}
