using SRRPlayer;
using UnityEngine;

public class ResourceTrigger : ATrigger
{
    [SerializeField] private bool isKillParent;
    [SerializeField] private bool isMinus;
    [SerializeField] private int value;
    [SerializeField] private float rotationSpeed = 100f;
    protected override void OnTrigger(Player player)
    {
        if (isMinus)
        {
            player.RemoveMoney(value);
        }
        else
        {
            ServicesManager.Instance.Get<CoinsCollectComponent>().Collect(value, transform.position);
        }

        ServicesManager.Instance.Get<AudioManager>().Play(interactSound);
        gameObject.SetActive(false);
        if(isKillParent) transform.parent.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (rotationSpeed == 0)
            return;

        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }
}
