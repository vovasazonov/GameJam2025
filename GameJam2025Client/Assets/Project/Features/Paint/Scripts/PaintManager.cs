using System;
using System.Collections.Generic;
using System.Linq;
using Project.Core.Scripts;
using Project.Features.Input.Scripts;
using Project.Features.LineCalculation.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

[RequireComponent(typeof(LineRenderer))]
public class PaintManager : SingletonBehaviour<PaintManager>
{
    [SerializeField] private float _minDistanceBetweenPoints = 0.1f;
    [SerializeField] private GameObject _intersectionPrefab;
    [SerializeField] private bool _shouldDrawResult = true;
    [SerializeField] private LineRenderer _lineRendererLoopPrefab;

    private LineRenderer _lineRenderer;
    private Vector2 _previousPosition;
    private bool _hasPreviousInput;
    private InputAction _pointAction;
    private readonly List<GameObject> _tempObjects = new List<GameObject>();

    public event Action PointsUpdated;
    public List<Vector2> CurrentPoints { get; } = new List<Vector2>();

    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (InputManager.Instance.IsPointDown)
        {
            Draw();
        }
        else
        {
            if (_hasPreviousInput)
            {
                _hasPreviousInput = false;
                OnStopPaint();
            }
        }
    }
    
    private void UpdateDrawnLoops()
    {
        CurrentPoints.Clear();
        for (var i = 0; i < _lineRenderer.positionCount; i++)
        {
            var point = _lineRenderer.GetPosition(i);
            CurrentPoints.Add(new Vector2(point.x, point.y));
        }

        var points = CurrentPoints;
        LineCalculationManager.Instance.AddIntersectionsToLine(ref points);
        
        PointsUpdated?.Invoke();
    }

    private void DrawIntersections()
    {
        var intersections = CurrentPoints.GroupBy(x => x)
            .Where(g => g.Count() > 1)
            .Select(y => y.Key);
        foreach (var intersection in intersections)
        {
            var obj = Object.Instantiate(_intersectionPrefab);
            obj.transform.position = intersection;
            _tempObjects.Add(obj);
        }
    }
    
    private void DrawPossibleLoops()
    {
        var routes = LineCalculationManager.Instance.FindRoutes(CurrentPoints);
        foreach (var route in routes)
        {
            var obj = Object.Instantiate(_lineRendererLoopPrefab);
            obj.transform.position = Vector3.zero;
            obj.positionCount = route.Count;
            for (int i = 0; i < route.Count; i++)
            {
                var cameraZ = PositionZ();
                var point = new Vector3(route[i].x, route[i].y, cameraZ);
                obj.SetPosition(i, point);
                obj.startColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
                obj.endColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
            }
        }
    }

    private void Draw()
    {
        var input = InputManager.Instance.PointPosition;

        var cameraZ = PositionZ();
        var currentPosition = Camera.main.ScreenToWorldPoint(new Vector3(input.x, input.y, cameraZ));
        currentPosition.z = 0;

        if (_hasPreviousInput)
        {
            if (Vector3.Distance(currentPosition, _previousPosition) > _minDistanceBetweenPoints)
            {
                _lineRenderer.positionCount++;
                _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, currentPosition);
                _previousPosition = currentPosition;
            }
        }
        else
        {
            _lineRenderer.positionCount = 1;
            _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, currentPosition);
            _previousPosition = currentPosition;

            _hasPreviousInput = true;
            OnStartPaint();
        }
    }

    private static float PositionZ()
    {
        return -1 * Camera.main.transform.position.z;
    }

    private void OnStartPaint()
    {
        _lineRenderer.loop = false;
        
        _tempObjects.ForEach(Object.Destroy);
        _tempObjects.Clear();
    }

    private void OnStopPaint()
    {
        _lineRenderer.loop = true;
        UpdateDrawnLoops();
        if (_shouldDrawResult)
        {
            DrawIntersections();
            DrawPossibleLoops();
        }
    }
}