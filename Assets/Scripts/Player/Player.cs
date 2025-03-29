using System;
using UnityEngine;
using UnityEngine.Events;

namespace SRRPlayer
{
    public class Player : MonoBehaviour
    {
        [field: Header("Components")]
        [field: SerializeField] public Camera Camera { get; private set; }  
        [SerializeField] private PlayerAnimatorComponent animatorComponent;
        [SerializeField] private PlayerMovementComponent movementComponent;
        [SerializeField] private PlayerUIComponent uiComponent;
        private PlayerInputComponent inputComponent;
        [Header("Properties")]
        [SerializeField] private StateProps defaultProps;
        [SerializeField] private StateProps[] stateProps;
        [Header("Effects")]
        [SerializeField] private ParticleSystem moneyEffect;
        [SerializeField] private ParticleSystem bloodEffect;

        public UnityEvent<int> onMoneyChange = new UnityEvent<int>();
        private LevelManager levelManager;

        public PlayerState CurrentState { get; private set; }
        public int Money { get; private set; }

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
            Money += amount;
            var props = GetProps();
            if (CurrentState != props.State)
            {
                movementComponent.Rotate();
            }
            CurrentState = props.State;
            animatorComponent.PlayWalk(CurrentState);
            uiComponent.SetInfo(props.State, Money, stateProps[stateProps.Length - 1].Value);
            ActivateModel(props.State);

            onMoneyChange?.Invoke(Money);

            moneyEffect.Play();
        }
        public void RemoveMoney(int amount)
        {
            Money = Mathf.Max(Money - amount, 0);
            if(Money == 0)
            {
                levelManager.FinishGame(false);
            }
            var props = GetProps();
            ActivateModel(props.State);
            CurrentState = props.State;
            uiComponent.SetInfo(props.State, Money, stateProps[stateProps.Length - 1].Value);

            onMoneyChange?.Invoke(Money);

            bloodEffect.Play();
        }
        private void OnLevelStart(LevelItem item)
        {
            canMove = false;
            gameStarted = true;
            movementComponent.Init(item.Path);
            animatorComponent.PlayIdle();
            Money = defaultProps.Value;
            CurrentState = defaultProps.State;
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
            Money = defaultProps.Value;
            CurrentState = defaultProps.State;
            uiComponent.SetActive(true);
            uiComponent.SetInfo(defaultProps.State, Money, stateProps[stateProps.Length - 1].Value);
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
                if (stateProps[i].Value <= Money)
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