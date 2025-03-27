using PathCreation;
using System;
using UnityEngine;

namespace SRRPlayer
{
    public class Player : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private PlayerAnimatorComponent animatorComponent;
        [SerializeField] private PlayerMovementComponent movementComponent;

        [SerializeField] private PathCreator pathCreator;

        private void Start()
        {
            movementComponent.Init(pathCreator);
            animatorComponent.PlayWalk(PlayerState.Poor);
        }

        private void Update()
        {
            movementComponent.Move();
        }
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