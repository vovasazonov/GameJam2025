using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Core.Scripts;

namespace Project.Features.Tutorial.Scripts
{
    public class TutorialManager : SingletonBehaviour<TutorialManager>
    {
        [SerializeField] private GameObject _fingerObject; // Assign in inspector
        [SerializeField] private float _moveSpeed = 300f; // Pixels per second

        private Coroutine _tutorialCoroutine;

        private void Start()
        {
            StopTutorial();
        }

        public void StartTutorial(List<Vector2> points)
        {
            if (_fingerObject == null || points == null || points.Count < 2)
            {
                Debug.LogError("TutorialManager: Finger object or points not set correctly.");
                return;
            }

            if (_tutorialCoroutine != null)
            {
                StopCoroutine(_tutorialCoroutine);
            }

            _fingerObject.SetActive(true);
            _tutorialCoroutine = StartCoroutine(MoveFingerAlongPath(points));
        }

        public void StopTutorial()
        {
            if (_tutorialCoroutine != null)
            {
                StopCoroutine(_tutorialCoroutine);
                _tutorialCoroutine = null;
            }

            if (_fingerObject != null)
            {
                _fingerObject.SetActive(false);
            }
        }


        private IEnumerator MoveFingerAlongPath(List<Vector2> points)
        {
            _fingerObject.transform.position = points[0];

            for (int i = 1; i < points.Count; i++)
            {
                Vector2 start = _fingerObject.transform.position;
                Vector2 end = points[i];
                float distance = Vector2.Distance(start, end);
                float duration = distance / _moveSpeed;
                float elapsed = 0f;

                while (elapsed < duration)
                {
                    elapsed += Time.deltaTime;
                    float t = Mathf.Clamp01(elapsed / duration);
                    _fingerObject.transform.position = Vector2.Lerp(start, end, t);
                    yield return null;
                }

                _fingerObject.transform.position = end;
            }

            _fingerObject.SetActive(false); // Hide after reaching end
            _tutorialCoroutine = null;
        }
    }
}