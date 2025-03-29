using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinsCollectComponent : MonoBehaviour, IService
{
    [SerializeField] private Transform container;
    [SerializeField] private CoinItemComponent coinPrefab;
    [SerializeField] private TextMeshProUGUI coinsTextComponent;

    private PlayerManager playerManager;
    public IEnumerator Init(System.Action<float, string> progress)
    {
        playerManager = ServicesManager.Instance.Get<PlayerManager>();
        playerManager.onInit.AddListener((p) => { p.onMoneyChange.AddListener((m) => { coinsTextComponent.text = m.ToString(); }); });
        coinsTextComponent.text = "0";
        yield return null;
    }
    public void Collect(int amount, Vector3 position)
    {
        Vector3 startPosition = Camera.main.WorldToScreenPoint(position);

        var coins = GetCoinsAmount(amount);
        for (int i = 0; i < coins.Count; i++)
        {
            Vector3 randomOffset = Random.insideUnitCircle * 10;
            var coin = Instantiate(coinPrefab, startPosition + randomOffset, Quaternion.identity, transform);
            coin.transform.SetAsFirstSibling();
            coin.Init(coins[i]);
            coin.transform.DOMove(container.position, Random.Range(0.3f, 0.7f))
                .SetEase(Ease.InOutBounce).OnComplete(() => 
                {
                    playerManager.Player.AddMoney(coin.Value);
                    Destroy(coin.gameObject); 
                }).Play();
        }
    }

    private List<int> GetCoinsAmount(int amount)
    {
        List<int> coinParts = new List<int>();
        int parts = Random.Range(2, 5);

        while (parts > 1)
        {
            int part = Random.Range(1, amount - (parts - 1));
            coinParts.Add(part);
            amount -= part;
            parts--;
        }

        coinParts.Add(amount);

        return coinParts;
    }

}
