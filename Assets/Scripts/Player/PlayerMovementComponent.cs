using PathCreation;
using UnityEngine;

namespace SRRPlayer
{
    public class PlayerMovementComponent : MonoBehaviour
    {
        [SerializeField] private Transform model;
        [SerializeField, Range(0, 5)]
        [Tooltip("Отвечает за скорость передвижения персонажа вперед")] private float movementSpeed = 2f;
        [SerializeField, Range(0, 5)]
        [Tooltip("Отвечает за смещение в стороны")] private float sideOffset = 1.5f;
        [SerializeField, Range(0, 5)]
        [Tooltip("Отвечает за скорость смещения в стороны")] private float sideSpeed = 1.5f;

        private PathCreator pathComponent;
        private float elapsedDistance;

        private Vector2 touchPosition;
        public void Init(PathCreator path)
        {
            pathComponent = path;
            elapsedDistance = 0;
        }
        public void Move()
        {
            elapsedDistance += movementSpeed * Time.deltaTime;
            Vector3 pathPosition = pathComponent.path.GetPointAtDistance(elapsedDistance, EndOfPathInstruction.Stop);

            int direction = GetDirection();
            float sidePosition = model.localPosition.x;

            if (direction != 0)
            {
                sidePosition += direction * sideSpeed * Time.deltaTime;
                sidePosition = Mathf.Clamp(sidePosition, -sideOffset, sideOffset);
            }

            transform.position = pathPosition;
            model.localPosition = new Vector3(sidePosition, 0, 0);

            var targetRotation = pathComponent.path.GetRotationAtDistance(elapsedDistance, EndOfPathInstruction.Stop).eulerAngles;
            transform.rotation = Quaternion.Euler(targetRotation.x, targetRotation.y, 0);
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

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(new Vector3(transform.position.x + sideOffset, transform.position.y), 0.1f);
            Gizmos.DrawWireSphere(new Vector3(transform.position.x - sideOffset, transform.position.y), 0.1f);
        }
    }
}