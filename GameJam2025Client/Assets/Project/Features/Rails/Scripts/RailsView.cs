using System.Collections.Generic;
using UnityEngine;

namespace Project.Features.Rails.Scripts
{
    public class RailsView : MonoBehaviour
    {
        [SerializeField] private float _lineOffset;
        [SerializeField] private float _distanceBetweenRail;
        [SerializeField] private GameObject _railPrefab;
        [SerializeField] private LineRenderer _leftLineRenderer;
        [SerializeField] private LineRenderer _rightLineRenderer;

        private readonly List<GameObject> _rails = new List<GameObject>();
        private Vector3 _lastRailPosition;
        private readonly List<Vector3> _allPositions = new List<Vector3>();

        public void AddPosition(Vector3 position)
        {
            _allPositions.Add(position);
            
            Vector3 leftPosition = position;
            Vector3 rightPosition = position;
            
            if (_allPositions.Count > 1)
            {
                // Get the last point to calculate direction
                Vector3 lastPosition = _allPositions[^2];
                Vector3 direction = (position - lastPosition).normalized;

                // Perpendicular vector on X-Y plane (Z-up)
                Vector3 perpendicular = new Vector3(-direction.y, direction.x, 0f); // Left-hand normal

                var offset = perpendicular * _lineOffset;

                leftPosition = position - offset;
                rightPosition = position + offset;
            }
            
            _leftLineRenderer.positionCount++;
            _rightLineRenderer.positionCount++;
            _leftLineRenderer.SetPosition(_leftLineRenderer.positionCount - 1, leftPosition);
            _rightLineRenderer.SetPosition(_rightLineRenderer.positionCount - 1, rightPosition);
            
            if (_rails.Count == 0 || Vector3.Distance(_lastRailPosition, position) > _distanceBetweenRail)
            {
                var rail = Instantiate(_railPrefab, transform);
                Vector2 direction = _leftLineRenderer.positionCount > 1
                    ? (_leftLineRenderer.GetPosition(_leftLineRenderer.positionCount - 1) - _leftLineRenderer.GetPosition(_leftLineRenderer.positionCount - 2)).normalized
                    : Vector2.zero;
                Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.back);
                rail.transform.rotation = targetRotation;
                rail.transform.position = position;
                _rails.Add(rail);
                _lastRailPosition = position;
            }
        }
        
        public void AddPositions(List<Vector3> positions)
        {
            positions.ForEach(AddPosition);
        }

        public void SetLoop(bool isLoop)
        {
            _leftLineRenderer.loop = isLoop;
            _rightLineRenderer.loop = isLoop;
        }
    }
}