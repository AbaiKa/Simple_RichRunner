using SRRPlayer;
using UnityEngine;

public class DoorTrigger : ATrigger
{
    [SerializeField] private PlayerState stateToReach;
    [SerializeField] private RotationAnimation firstDoor;
    [SerializeField] private RotationAnimation secondDoor;
    protected override void OnTrigger(Player player)
    {
        if(player.CurrentState >= stateToReach)
        {
            firstDoor.Play();
            secondDoor.Play();
            ServicesManager.Instance.Get<AudioManager>().Play(interactSound);
        }
        else
        {
            ServicesManager.Instance.Get<LevelManager>().FinishGame(true);
        }
    }
}
