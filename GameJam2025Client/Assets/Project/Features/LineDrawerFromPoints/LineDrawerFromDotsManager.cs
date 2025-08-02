using UnityEngine;
using System.Collections.Generic;
using Project.Core.Scripts;
using Project.Features.LineCalculation.Scripts;

[RequireComponent(typeof(LineRenderer))]
public class LineDrawerFromDotsManager : SingletonBehaviour<LineDrawerFromDotsManager>
{
    [Header("Line Settings")]
    [SerializeField] private float lineWidth = 0.1f;
    [SerializeField] private Material lineMaterial;
    [SerializeField] private Color lineColor = Color.white;
    [SerializeField] private int sortingOrder = 0;

    private LineRenderer lineRenderer;

    private void Awake()
    {
        InitializeLineRenderer();
    }

    private void InitializeLineRenderer()
    {
        lineRenderer = GetComponent<LineRenderer>();
        
        // Настройка основных параметров линии
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.material = lineMaterial;
        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;
        lineRenderer.sortingOrder = sortingOrder;
        
        // Оптимизация для динамического изменения
        lineRenderer.useWorldSpace = false;
        lineRenderer.textureMode = LineTextureMode.Tile;
        lineRenderer.numCapVertices = 5;
        lineRenderer.numCornerVertices = 5;
    }

    /// <summary>
    /// Отрисовывает линию по заданным точкам
    /// </summary>
    /// <param name="points">Список точек в 2D пространстве</param>
    public void DrawLine(List<Vector2> points)
    {
        if (points == null || points.Count < 2)
        {
            ClearLine();
            return;
        }

        // Преобразуем 2D точки в 3D (с Z=0 для 2D пространства)
        Vector3[] positions = new Vector3[points.Count];
        for (int i = 0; i < points.Count; i++)
        {
            positions[i] = new Vector3(points[i].x, points[i].y, 0);
        }

        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(positions);
    }

    /// <summary>
    /// Очищает текущую линию
    /// </summary>
    public void ClearLine()
    {
        lineRenderer.positionCount = 0;
    }

    /// <summary>
    /// Обновляет внешний вид линии
    /// </summary>
    public void UpdateLineAppearance(float width, Color color, Material material = null)
    {
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        
        if (material != null)
        {
            lineRenderer.material = material;
        }
    }
}