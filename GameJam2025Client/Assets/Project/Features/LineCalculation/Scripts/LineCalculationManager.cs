using System.Collections.Generic;
using System.Linq;
using Project.Core.Scripts;
using UnityEngine;

namespace Project.Features.LineCalculation.Scripts
{
    public class LineCalculationManager : SingletonBehaviour<LineCalculationManager>
    {
        public List<List<Vector2>> FindRoutes(List<Vector2> points)
        {
            points = points.Select(i => Round(i)).ToList();
            
            DFSVisualizerManager.Instance?.Clear();

            var intersections = new HashSet<Vector2>(
                points.GroupBy(p => p).Where(g => g.Count() > 1).Select(g => g.Key)
            );

            var graph = new Dictionary<Vector2, List<Vector2>>();
            for (int i = 0; i < points.Count - 1; i++)
            {
                AddEdge(graph, points[i], points[i + 1]);
                AddEdge(graph, points[i + 1], points[i]);
                if (i == points.Count - 2)
                {
                    AddEdge(graph, points[i+1], points[0]);
                    AddEdge(graph, points[0], points[i+1]);
                }
            }

            var routes = new List<List<Vector2>>();
            var uniqueRoutes = new HashSet<string>();

            foreach (var start in intersections)
            {
                DFS(start, start, new List<Vector2>(), new HashSet<Vector2>(), graph, intersections, routes, uniqueRoutes);
            }

            return routes;
        }

        private void DFS(
            Vector2 current,
            Vector2 start,
            List<Vector2> path,
            HashSet<Vector2> visitedNonIntersections,
            Dictionary<Vector2, List<Vector2>> graph,
            HashSet<Vector2> intersections,
            List<List<Vector2>> routes,
            HashSet<string> uniqueRoutes)
        {
            path.Add(current);

            bool isIntersection = intersections.Contains(current);
            if (!isIntersection)
            {
                visitedNonIntersections.Add(current);
            }

            // Valid loop: length > 2, ends back at starting intersection
            if (path.Count > 2 && current == start)
            {
                string key = SerializeLoop(path);
                string reversedKey = SerializeLoop(path.AsEnumerable().Reverse());

                if (!uniqueRoutes.Contains(key) && !uniqueRoutes.Contains(reversedKey))
                {
                    uniqueRoutes.Add(key);
                    routes.Add(new List<Vector2>(path));
                }

                return;
            }

            foreach (var neighbor in graph[current])
            {
                bool neighborIsIntersection = intersections.Contains(neighbor);

                // Don't revisit non-intersections
                if (!neighborIsIntersection && visitedNonIntersections.Contains(neighbor))
                    continue;

                // Prevent infinite self-loop from 2-point cycle
                if (path.Count > 1 && neighbor == start && path[path.Count - 2] == start)
                    continue;

                DFS(
                    neighbor,
                    start,
                    new List<Vector2>(path),
                    new HashSet<Vector2>(visitedNonIntersections),
                    graph,
                    intersections,
                    routes,
                    uniqueRoutes
                );
            }
        }

        private string SerializeLoop(IEnumerable<Vector2> path)
        {
            return string.Join("->", path.Select(p => $"{p.x:F4},{p.y:F4}"));
        }

        private void AddEdge(Dictionary<Vector2, List<Vector2>> graph, Vector2 a, Vector2 b)
        {
            if (!graph.TryGetValue(a, out var neighbors))
            {
                neighbors = new List<Vector2>();
                graph[a] = neighbors;
            }

            if (!neighbors.Contains(b))
                neighbors.Add(b);
        }

        private Vector2 Round(Vector2 v, int decimals = 2)
        {
            return new Vector2(
                (float)Mathf.Round(v.x * Mathf.Pow(10, decimals)) / Mathf.Pow(10, decimals),
                (float)Mathf.Round(v.y * Mathf.Pow(10, decimals)) / Mathf.Pow(10, decimals)
            );
        }
        
        public void AddIntersectionsToLine(ref List<Vector2> points)
        {
            List<(int insertIndex, Vector2 point)> intersectionsToInsert = new List<(int insertIndex, Vector2 point)>();

            for (int i = 0; i < points.Count - 1; i++)
            {
                Vector2 a1 = points[i];
                Vector2 a2 = points[i + 1];

                for (int j = i + 2; j < points.Count - 1; j++)
                {
                    if (j == i || j == i + 1)
                        continue;

                    Vector2 b1 = points[j];
                    Vector2 b2 = points[j + 1];

                    if (LineSegmentsIntersect(a1, a2, b1, b2, out Vector2 intersection))
                    {
                        // Insert after a1-a2 and b1-b2
                        intersectionsToInsert.Add((i + 1, intersection));
                        intersectionsToInsert.Add((j + 1, intersection));
                    }
                }
            }

            intersectionsToInsert.Sort((a, b) => b.insertIndex.CompareTo(a.insertIndex));

            foreach (var (index, point) in intersectionsToInsert)
            {
                points.Insert(index, point);
            }
        }

        public List<Vector2> FindIntersections(List<Vector2> points)
        {
            List<Vector2> intersections = new List<Vector2>();

            for (int i = 0; i < points.Count - 1; i++)
            {
                Vector2 a1 = points[i];
                Vector2 a2 = points[i + 1];

                for (int j = i + 2; j < points.Count - 1; j++)
                {
                    if (j == i || j == i + 1)
                    {
                        continue;
                    }

                    Vector2 b1 = points[j];
                    Vector2 b2 = points[j + 1];

                    if (LineSegmentsIntersect(a1, a2, b1, b2, out Vector2 intersection))
                    {
                        intersections.Add(intersection);
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
}