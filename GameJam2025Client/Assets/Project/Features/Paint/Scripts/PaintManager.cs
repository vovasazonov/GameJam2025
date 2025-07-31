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

    public List<Vector2> CurrentPoint { get; private set; } = new();

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
                UpdateDrawnLoops();
            }

            _hasPreviousInput = false;
        }
    }

    private void UpdateDrawnLoops()
    {
        CurrentPoint.Clear();
        for (var i = 0; i < _lineRenderer.positionCount; i++)
        {
            var point = _lineRenderer.GetPosition(i);
            CurrentPoint.Add(new Vector2(point.x, point.y));
        }
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
        }
    }
}