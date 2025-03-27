using SRRPlayer;
using UnityEngine;

public abstract class ATrigger : MonoBehaviour
{
    protected abstract void OnTrigger(Player player);

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Triggered: {other.name}");
        if(other.TryGetComponent(out Player player))
        {
            OnTrigger(player);
        }
    }
}
