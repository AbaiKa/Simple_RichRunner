using SRRPlayer;
using UnityEngine;

public class ResourceTrigger : ATrigger
{
    [SerializeField] private bool isMinus;
    [SerializeField] private int value;
    protected override void OnTrigger(Player player)
    {
        if (isMinus)
        {
            player.RemoveMoney(value);
        }
        else
        {
            player.AddMoney(value);
        }

        ServicesManager.Instance.Get<AudioManager>().Play(interactSound);
        gameObject.SetActive(false);
    }
}
