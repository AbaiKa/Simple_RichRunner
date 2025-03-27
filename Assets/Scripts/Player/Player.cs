using System;
using UnityEngine;

namespace SRRPlayer
{
    public class Player : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private PlayerAnimatorComponent animatorComponent;
    }

    [Serializable]
    public enum PlayerState
    {
        Homeless = 1,
        Poor = 1,
        Wealthy = 2,
        Rich = 3,
        Millionaire = 4,
    }
}