using System;
using System.Collections.Generic;
using System.Linq;
using Project.Core.Scripts;
using Project.Features.Input.Scripts;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Project.Features.LoopsExplorer.Scripts
{
    public class LoopsExplorerManager : SingletonBehaviour<LoopsExplorerManager>
    {
        [SerializeField] private float _radiusFinger = 10f;
        [SerializeField] private float _matchThreshold = 0.6f;
        [SerializeField] private LineRenderer _touchLineRenderer;
        [SerializeField] private LineRenderer _loopPrefab;

        private readonly Dictionary<int, List<Vector2>> _allLoopsById = new();
        private readonly HashSet<int> _foundLoopsIds = new();
        private readonly List<Vector2> _fullLine = new();

        private readonly List<Vector2> _touchedPoints = new();
        private bool _isDown;

        private readonly List<LineRenderer> _foundLoops = new();

        public event Action<int> FoundLoop;
        public int TotalLoops => _allLoopsById.Count;
        public int FoundLoops => _foundLoopsIds.Count;
        public IEnumerable<int> AllFoundLoopsIds() => _allLoopsById.Keys;

        // TODO: remove
        // private void Start()
        // {
        //     _foundLoops.Add(Object.Instantiate(_loopPrefab));
        //     var level = LevelDatabase.Instance.GetLevel(3);
        //     InitializeExplorer(level.points, level.cycles);
        //     _foundLoops[0].positionCount = level.points.Count;
        //     for (var i = 0; i < level.points.Count; i++)
        //     {
        //         var point = level.points[i];
        //         _foundLoops[0].SetPosition(i, point);
        //     }
        // }

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

                var inputPosition = InputManager.Instance.PointPosition;
                var cameraZ = -1 * Camera.main.transform.position.z;
                var currentPosition = Camera.main.ScreenToWorldPoint(new Vector3(inputPosition.x, inputPosition.y, cameraZ));
                currentPosition.z = 0;
                
                var closestPoint = GetClosestPointOnLine(new Vector2(currentPosition.x, currentPosition.y));

                if (closestPoint.HasValue)
                {
                    var point = closestPoint.Value;
                    if (_touchedPoints.Count == 0 || _touchedPoints[^1] != point)
                    {
                        _touchedPoints.Add(point);

                        _touchLineRenderer.positionCount++;
                        _touchLineRenderer.SetPosition(_touchedPoints.Count - 1, point);
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
            _touchLineRenderer.positionCount = 0;
            
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
                    Debug.Log($"Loop id:{kvp.Key} found! Total loops: {TotalLoops}, Found loops: {FoundLoops}");
                    FoundLoop?.Invoke(kvp.Key);
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