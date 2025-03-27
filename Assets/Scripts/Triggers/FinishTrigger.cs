using SRRPlayer;

public class FinishTrigger : ATrigger
{
    protected override void OnTrigger(Player player)
    {
        ServicesManager.Instance.Get<LevelManager>().FinishGame(true);
    }
}
