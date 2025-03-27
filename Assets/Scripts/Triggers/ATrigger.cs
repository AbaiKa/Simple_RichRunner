using SRRPlayer;
using UnityEngine;

public abstract class ATrigger : MonoBehaviour
{
    [SerializeField] protected AudioClip interactSound;
    protected abstract void OnTrigger(Player player);

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.parent.TryGetComponent(out Player player))
        {
            OnTrigger(player);
        }
    }
}
