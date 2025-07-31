using System.Collections.Generic;
using Project.Core.Scripts;
using Project.Features.Input.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(LineRenderer))]
public class PaintManager : SingletonBehaviour<PaintManager>
{
    [SerializeField] private float _minDistanceBetweenPoints = 0.1f;

    private LineRenderer _lineRenderer;
    private Vector2 _previousPosition;
    private bool _hasPreviousInput;
    private InputAction _pointAction;

    public List<Vector2> CurrentPoints { get; private set; } = new();

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

        if (CurrentPoints.Count > 0)
        {
            _lineRenderer.positionCount++;
            _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, CurrentPoints[0]);
        }

        foreach (var point in FindIntersections(CurrentPoints))
        {
            Debug.Log(point);
        }
        // TODO: detect intersection points and put there in CurrentPoint list
    }

    private void Draw()
    {
        var input = InputManager.Instance.PointPosition;
        
        var cameraZ = -1 * Camera.main.transform.position.z;
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

    private void OnStartPaint()
    {
        _lineRenderer.loop = false;
    }

    private void OnStopPaint()
    {
        _lineRenderer.loop = true;
        UpdateDrawnLoops();
    }
    
    private List<Vector2> FindIntersections(List<Vector2> points)
    {
        List<Vector2> intersections = new();

        for (int i = 0; i < points.Count - 1; i++)
        {
            Vector2 a1 = points[i];
            Vector2 a2 = points[i + 1];

            for (int j = i + 2; j < points.Count - 1; j++)
            {
                // Skip adjacent segments
                if (j == i || j == i + 1)
                {
                    continue;
                }
                Vector2 b1 = points[j];
                Vector2 b2 = points[j + 1];

                if (LineSegmentsIntersect(a1, a2, b1, b2, out Vector2 intersection))
                {
                    intersections.Add(intersection);
                    // Optional: Insert into points list (if needed)
                }
            }
        }

        return intersections;
    }

    private bool LineSegmentsIntersect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, out Vector2 intersection)
    {
        intersection = Vector2.zero;

        float denominator = (p1.x - p2.x) * (p3.y - p4.y) - 
                            (p1.y - p2.y) * (p3.x - p4.x);

        if (Mathf.Approximately(denominator, 0))
        {
            return false; // Parallel lines
        }
        float x = ((p1.x * p2.y - p1.y * p2.x) * (p3.x - p4.x) -
                   (p1.x - p2.x) * (p3.x * p4.y - p3.y * p4.x)) / denominator;

        float y = ((p1.x * p2.y - p1.y * p2.x) * (p3.y - p4.y) -
                   (p1.y - p2.y) * (p3.x * p4.y - p3.y * p4.x)) / denominator;

        intersection = new Vector2(x, y);

        // Check if the intersection is within both segments
        if (IsPointOnSegment(p1, p2, intersection) && IsPointOnSegment(p3, p4, intersection))
        {
            return true;
        }
        return false;
    }

    private bool IsPointOnSegment(Vector2 a, Vector2 b, Vector2 p)
    {
        return p.x >= Mathf.Min(a.x, b.x) && p.x <= Mathf.Max(a.x, b.x) &&
               p.y >= Mathf.Min(a.y, b.y) && p.y <= Mathf.Max(a.y, b.y);
    }
}