using UnityEngine;

namespace SRRPlayer
{
    public class PlayerAnimatorComponent : MonoBehaviour
    {
        [SerializeField] private Animator animatorComponent;

        public void PlayIdle()
        {
            SetState(0);
        }
        public void PlayWalk(PlayerState state)
        {
            SetState((int)state);
        }
        public void PlayVictory()
        {
            SetState(5);
        }
        public void PlayLose()
        {
            SetState(6);
        }

        /// <summary>
        /// 0 - idle,
        /// 1, 4 - walk,
        /// 5 - victory,
        /// 6 - lose
        /// </summary>
        private void SetState(int state)
        {
            animatorComponent.SetInteger("state", state);
        }
    }
}
