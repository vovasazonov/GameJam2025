using System.Collections;
using Project.Core.Scripts;
using UnityEngine;

namespace Project.Features.Cameras
{
    public class CameraManager : SingletonBehaviour<CameraManager>
    {
        [SerializeField] private Vector3 _targetPosition;
        [SerializeField] private Vector3 _targetRotation;
        [SerializeField] private float _transitionDuration = 1.5f;

        private Vector3 _defaultPosition;
        private Vector3 _defaultRotationEuler;
        private Coroutine _transitionRoutine;

        private void Start()
        {
            _defaultPosition = transform.position;
            _defaultRotationEuler = transform.eulerAngles;
        }

        public void Show()
        {
            if (_transitionRoutine != null)
                StopCoroutine(_transitionRoutine);
            _transitionRoutine = StartCoroutine(SmoothTransition(_targetPosition, _targetRotation));
        }

        public void Reset()
        {
            if (_transitionRoutine != null)
                StopCoroutine(_transitionRoutine);

            transform.position = _defaultPosition;
            transform.eulerAngles = _defaultRotationEuler;
        }

        private IEnumerator SmoothTransition(Vector3 targetPos, Vector3 targetRot)
        {
            Vector3 startPos = transform.position;
            Vector3 startRot = transform.eulerAngles;

            float elapsed = 0f;

            while (elapsed < _transitionDuration)
            {
                float t = elapsed / _transitionDuration;
                transform.position = Vector3.Lerp(startPos, targetPos, t);
                transform.rotation = Quaternion.Lerp(Quaternion.Euler(startRot), Quaternion.Euler(targetRot), t);

                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.position = targetPos;
            transform.rotation = Quaternion.Euler(targetRot);
        }
    }
}