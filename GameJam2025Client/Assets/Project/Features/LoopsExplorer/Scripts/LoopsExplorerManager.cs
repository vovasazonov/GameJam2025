using System.Collections.Generic;
using System.Linq;
using Project.Core.Scripts;
using Project.Features.Input.Scripts;
using UnityEngine;

namespace Project.Features.LoopsExplorer.Scripts
{
    public class LoopsExplorerManager : SingletonBehaviour<LoopsExplorerManager>
    {
        [SerializeField] private float _radiusFinger = 10f;
        [SerializeField] private float _matchThreshold = 0.6f;

        private readonly Dictionary<int, List<Vector2>> _allLoopsById = new();
        private readonly HashSet<int> _foundLoopsIds = new();
        private readonly List<Vector2> _fullLine = new();

        private readonly List<Vector2> _touchedPoints = new();
        private bool _isDown;

        public int TotalLoops => _allLoopsById.Count;
        public int FoundLoops => _foundLoopsIds.Count;
        public IEnumerable<int> AllFoundLoopsIds() => _allLoopsById.Keys;

        public void InitializeExplorer(List<Vector2> fullLine, List<List<Vector2>> loops)
        {
            _fullLine.Clear();
            _allLoopsById.Clear();
            _foundLoopsIds.Clear();

            _fullLine.AddRange(fullLine);

            for (var i = 0; i < loops.Count; i++)
            {
                _allLoopsById.Add(i, loops[i]);
            }
        }

        public List<Vector2> GetLoopById(int id)
        {
            return _allLoopsById[id];
        }

        private void Update()
        {
            if (InputManager.Instance.IsPointDown)
            {
                if (!_isDown)
                {
                    _isDown = true;
                    _touchedPoints.Clear();
                }

                var position = InputManager.Instance.PointPosition;
                var closestPoint = GetClosestPointOnLine(position);

                if (closestPoint.HasValue)
                {
                    var point = closestPoint.Value;
                    if (_touchedPoints.Count == 0 || _touchedPoints.Last() != point)
                    {
                        _touchedPoints.Add(point);
                    }
                }
            }
            else if (_isDown)
            {
                _isDown = false;
                OnTouchEnd();
            }
        }

        private void OnTouchEnd()
        {
            if (_touchedPoints.Count < 3)
                return;

            foreach (var kvp in _allLoopsById)
            {
                if (_foundLoopsIds.Contains(kvp.Key))
                    continue;

                var loop = kvp.Value;
                float overlap = loop.Intersect(_touchedPoints).Count();
                float similarity = overlap / Mathf.Max(loop.Count, _touchedPoints.Count);

                if (similarity >= _matchThreshold)
                {
                    _foundLoopsIds.Add(kvp.Key);
                    Debug.Log($"Loop {kvp.Key} found!");
                    break;
                }
            }
        }

        private Vector2? GetClosestPointOnLine(Vector2 inputPosition)
        {
            foreach (var point in _fullLine)
            {
                if (Vector2.Distance(inputPosition, point) <= _radiusFinger)
                {
                    return point;
                }
            }
            return null;
        }
    }
}