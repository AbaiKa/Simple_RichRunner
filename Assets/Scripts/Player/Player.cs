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

        public PlayerState CurrentState { get; private set; }

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
            if (CurrentState != props.State)
            {
                movementComponent.Rotate();
            }
            CurrentState = props.State;
            animatorComponent.PlayWalk(CurrentState);
            uiComponent.SetInfo(props.State, money, stateProps[stateProps.Length - 1].Value);
            ActivateModel(props.State);
        }
        public void RemoveMoney(int amount)
        {
            money = Mathf.Max(money - amount, 0);
            if(money == 0)
            {
                levelManager.FinishGame(false);
            }
            var props = GetProps();
            ActivateModel(props.State);
            CurrentState = props.State;
            uiComponent.SetInfo(props.State, money, stateProps[stateProps.Length - 1].Value);
        }
        private void OnLevelStart(LevelItem item)
        {
            canMove = false;
            gameStarted = true;
            movementComponent.Init(item.Path);
            animatorComponent.PlayIdle();
            ActivateModel(defaultProps.State);
        }
        private void OnLevelEnd(bool victory)
        {
            gameStarted = false;
            canMove = false;
            uiComponent.SetActive(false);
            if (victory)
                animatorComponent.PlayVictory();
            else
                animatorComponent.PlayLose();
        }
        private void OnClick()
        {
            if (canMove)
                return;
            canMove = true;
            money = defaultProps.Value;
            CurrentState = defaultProps.State;
            uiComponent.SetActive(true);
            uiComponent.SetInfo(defaultProps.State, money, stateProps[stateProps.Length - 1].Value);
            animatorComponent.PlayWalk(CurrentState);
        }
        private void OnMove(int direction)
        {
            if (!canMove)
            {
                return;
            }
            movementComponent.Move(direction);
        }
        private void ActivateModel(PlayerState state)
        {
            for(int i = 0; i < stateProps.Length; i++)
            {
                stateProps[i].Model.SetActive(state == stateProps[i].State);
            }
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
            [field: SerializeField] public GameObject Model { get; private set; }
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