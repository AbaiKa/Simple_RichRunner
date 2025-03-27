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
        private PlayerInputComponent inputComponent;
        [Header("Properties")]
        [SerializeField] private PlayerState startState;

        private LevelManager levelManager;
        private bool gameStarted = false;
        private bool canMove = false;
        public void Init()
        {
            inputComponent = new PlayerInputComponent();
            inputComponent.onClick.AddListener(OnClick);
            inputComponent.onMove.AddListener(OnMove);

            levelManager = ServicesManager.Instance.Get<LevelManager>();
            levelManager.onStart.AddListener(OnLevelStart);
            levelManager.onEnd.AddListener(OnLevelEnd);
        }
        private void OnLevelStart(LevelItem item)
        {
            canMove = false;
            gameStarted = true;
            movementComponent.Init(item.Path);
            animatorComponent.PlayIdle();
        }
        private void OnLevelEnd(LevelItem item)
        {
            gameStarted = false;
            canMove = false;
        }

        private void Update()
        {
            if (!gameStarted)
            {
                return;
            }

            inputComponent.Update();
        }
        private void OnClick()
        {
            canMove = true;
            animatorComponent.PlayWalk(startState);
        }
        private void OnMove(int direction)
        {
            if (!canMove)
            {
                return;
            }
            movementComponent.Move(direction);
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