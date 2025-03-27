using PathCreation;
using System.Collections;
using UnityEngine;

namespace SRRPlayer
{
    public class PlayerMovementComponent : MonoBehaviour
    {
        [SerializeField] private Transform model;
        [SerializeField] private Transform camera;
        [SerializeField, Range(0, 5)]
        [Tooltip("Отвечает за скорость передвижения персонажа вперед")] private float movementSpeed = 2f;
        [SerializeField, Range(0, 5)]
        [Tooltip("Отвечает за смещение в стороны")] private float sideOffset = 1.5f;
        [SerializeField, Range(0, 5)]
        [Tooltip("Отвечает за скорость смещения в стороны")] private float sideSpeed = 1.5f;
        [SerializeField, Range(0, 50)]
        [Tooltip("Отвечает за угол поворота персонажа при смещении")] private float sideRotation = 10f;

        private PathCreator pathComponent;
        private float elapsedDistance;
        public void Init(PathCreator path)
        {
            pathComponent = path;
            elapsedDistance = 2.5f;
            transform.position = pathComponent.path.GetPointAtDistance(elapsedDistance, EndOfPathInstruction.Stop);
            var targetRotation = pathComponent.path.GetRotationAtDistance(elapsedDistance, EndOfPathInstruction.Stop).eulerAngles;
            transform.rotation = Quaternion.Euler(targetRotation.x, targetRotation.y, 0);
            model.localPosition = Vector3.zero;
            camera.localPosition = new Vector3(0, camera.localPosition.y, camera.localPosition.z);
        }
        public void Move(int direction)
        {
            elapsedDistance += movementSpeed * Time.deltaTime;
            Vector3 pathPosition = pathComponent.path.GetPointAtDistance(elapsedDistance, EndOfPathInstruction.Stop);

            float sidePosition = model.localPosition.x;

            if (direction != 0)
            {
                sidePosition += direction * sideSpeed * Time.deltaTime;
                sidePosition = Mathf.Clamp(sidePosition, -sideOffset, sideOffset);
            }

            transform.position = pathPosition;

            model.localPosition = new Vector3(sidePosition, 0, 0);
            camera.localPosition = new Vector3(sidePosition, camera.localPosition.y, camera.localPosition.z);
            Quaternion targetModelEuler = Quaternion.Euler(0, direction * sideRotation, 0);
            if (!isRotating)
            {
                model.localRotation = Quaternion.Slerp(model.localRotation, targetModelEuler, sideSpeed * Time.deltaTime);
            }

            var targetRotation = pathComponent.path.GetRotationAtDistance(elapsedDistance, EndOfPathInstruction.Stop).eulerAngles;
            transform.rotation = Quaternion.Euler(targetRotation.x, targetRotation.y, 0);
        }
        private bool isRotating = false;
        public void Rotate()
        {
            isRotating = true;
            StartCoroutine(RotateOverTime(0.2f));
        }
        private IEnumerator RotateOverTime(float duration)
        {
            float elapsedTime = 0f;
            float startY = model.localRotation.eulerAngles.y;
            float targetY = startY + 360f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / duration;
                float newY = Mathf.Lerp(startY, targetY, t);

                model.localRotation = Quaternion.Euler(0, newY, 0);
                yield return null;
            }

            model.localRotation = Quaternion.Euler(0, targetY, 0);
            isRotating = false;
        }


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(new Vector3(transform.position.x + sideOffset, transform.position.y), 0.1f);
            Gizmos.DrawWireSphere(new Vector3(transform.position.x - sideOffset, transform.position.y), 0.1f);
        }
    }
}