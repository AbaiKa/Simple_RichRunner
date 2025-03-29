using UnityEngine;

public class CoinItemComponent : MonoBehaviour
{
    public int Value { get; private set; }
    public void Init(int value)
    {
        Value = value;
    }
}
