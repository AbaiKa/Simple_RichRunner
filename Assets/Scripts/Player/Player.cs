using System;
using UnityEngine;

namespace SRRPlayer
{
    public class Player : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private PlayerAnimatorComponent animatorComponent;
        [SerializeField] private PlayerMovementComponent movementComponent;
        [SerializeField] private PlayerUIComponent uiComponent;
        private PlayerInputComponent inputComponent;
        [Header("Properties")]
        [SerializeField] private StateProps defaultProps;
        [SerializeField] private StateProps[] stateProps;

        private LevelManager levelManager;

        private int money;
        private bool gameStarted = false;
        private bool canMove = false;
        public void Init()
        {
            uiComponent.SetActive(false);

            inputComponent = new PlayerInputComponent();
            inputComponent.onClick.AddListener(OnClick);
            inputComponent.onMove.AddListener(OnMove);

            levelManager = ServicesManager.Instance.Get<LevelManager>();
            levelManager.onStart.AddListener(OnLevelStart);
            levelManager.onEnd.AddListener(OnLevelEnd);
        }
        private void Update()
        {
            if (!gameStarted)
            {
                return;
            }

            inputComponent.Update();
        }

        public void AddMoney(int amount)
        {
            money += amount;
            var props = GetProps();
            uiComponent.SetInfo(props.State, money, stateProps[stateProps.Length - 1].Value);
        }
        public void RemoveMoney(int amount)
        {
            money = Mathf.Max(money - amount, 0);
            if(money == 0)
            {
                levelManager.FinishGame(false);
            }
            var props = GetProps();
            uiComponent.SetInfo(props.State, money, stateProps[stateProps.Length - 1].Value);
        }
        private void OnLevelStart(LevelItem item)
        {
            canMove = false;
            gameStarted = true;
            movementComponent.Init(item.Path);
            animatorComponent.PlayIdle();
        }
        private void OnLevelEnd(bool victory)
        {
            gameStarted = false;
            canMove = false;
        }
        private void OnClick()
        {
            canMove = true;
            money = defaultProps.Value;
            uiComponent.SetActive(true);
            uiComponent.SetInfo(defaultProps.State, money, stateProps[stateProps.Length - 1].Value);
            animatorComponent.PlayWalk(defaultProps.State);
        }
        private void OnMove(int direction)
        {
            if (!canMove)
            {
                return;
            }
            movementComponent.Move(direction);
        }

        private StateProps GetProps()
        {
            for (int i = stateProps.Length - 1; i >= 0; i--)
            {
                if (stateProps[i].Value <= money)
                {
                    return stateProps[i];
                }
            }

            return stateProps[0];
        }

        [Serializable]
        public class StateProps
        {
            [field: SerializeField] public PlayerState State { get; private set; }
            [field: SerializeField] public int Value { get; private set; }
        }
    }

    [Serializable]
    public enum PlayerState
    {
        Homeless,
        Poor,
        Wealthy,
        Rich,
        Millionaire,
    }
}