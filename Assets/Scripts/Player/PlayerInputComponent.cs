using UnityEngine;
using UnityEngine.Events;

namespace SRRPlayer
{
    public class PlayerInputComponent
    {
        public UnityEvent onClick = new UnityEvent();
        public UnityEvent<int> onMove = new UnityEvent<int>();

        private Vector2 touchPosition;
        public void Update()
        {
            if (Input.touchCount > 0 || Input.GetMouseButtonDown(0))
            {
                onClick?.Invoke();
            }

            onMove?.Invoke(GetDirection());
        }

        private int GetDirection()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    touchPosition = touch.position;
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    float deltaX = touch.position.x - touchPosition.x;
                    if (Mathf.Abs(deltaX) > 50f)
                    {
                        return deltaX > 0 ? 1 : -1;
                    }
                }
            }

#if UNITY_EDITOR
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                return -1;
            }
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                return 1;
            }
#endif

            return 0;
        }
    }
}