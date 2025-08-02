using System.Collections;
using System.Collections.Generic;
using Project.Features.Trains.Scripts;
using UnityEngine;

public class TrainView : MonoBehaviour
{
    [SerializeField] private float _carriageSpacing = 2.5f;
    [SerializeField] private float _speed = 5f;

    private CarriageView[] _carriages;
    private List<Vector2> _loopPath;
    private float _totalDistance;

    public void StartFollowLoop(List<Vector2> loop)
    {
        _loopPath = new List<Vector2>(loop);
        _carriages = GetComponentsInChildren<CarriageView>();

        // Precompute total path distance for wrapping
        _totalDistance = 0f;
        for (int i = 0; i < _loopPath.Count - 1; i++)
        {
            _totalDistance += Vector2.Distance(_loopPath[i], _loopPath[i + 1]);
        }
        _totalDistance += Vector2.Distance(_loopPath[_loopPath.Count - 1], _loopPath[0]); // close loop

        StartCoroutine(FollowPathCoroutine());
    }

    private IEnumerator FollowPathCoroutine()
    {
        float distanceTravelled = 0f;

        while (true)
        {
            for (int i = 0; i < _carriages.Length; i++)
            {
                float targetDistance = distanceTravelled - i * _carriageSpacing;
                Vector2 pos = GetPositionAlongPath(targetDistance);
                _carriages[i].transform.position = pos;

                Vector2 nextPos = GetPositionAlongPath(targetDistance + 0.1f); // small lookahead for rotation
                Vector2 direction = (nextPos - pos).normalized;
                if (direction.sqrMagnitude > 0.001f)
                {
                    _carriages[i].transform.rotation = Quaternion.LookRotation(direction, Vector3.back);
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                }
            }

            distanceTravelled += _speed * Time.deltaTime;
            if (distanceTravelled > _totalDistance)
                distanceTravelled -= _totalDistance;

            yield return null;
        }
    }

    private Vector2 GetPositionAlongPath(float distance)
    {
        if (_loopPath.Count < 2)
            return Vector2.zero;

        distance = (distance % _totalDistance + _totalDistance) % _totalDistance; // wrap around path

        for (int i = 0; i < _loopPath.Count; i++)
        {
            Vector2 a = _loopPath[i];
            Vector2 b = _loopPath[(i + 1) % _loopPath.Count];
            float segmentLength = Vector2.Distance(a, b);

            if (distance <= segmentLength)
                return Vector2.Lerp(a, b, distance / segmentLength);

            distance -= segmentLength;
        }

        return _loopPath[0]; // fallback
    }
}