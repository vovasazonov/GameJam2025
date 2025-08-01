using System.Collections;
using System.Collections.Generic;
using Project.Core.Scripts;
using UnityEngine;

namespace Project.Features.LineCalculation.Scripts
{
    public class DFSVisualizerManager : SingletonBehaviour<DFSVisualizerManager>
    {
        [SerializeField] private LineRenderer _lineRendererPrefab;
        [SerializeField] private float _delay = 0.05f;
        
        private readonly List<LineRenderer> _steps = new List<LineRenderer>();
        
        public void Clear()
        {
            StopAllCoroutines();
            _steps.ForEach(i=> Destroy(i.gameObject));
            _steps.Clear();
        }
        
        public void VisualizeStep(List<Vector2> path)
        {
            var step = Instantiate(_lineRendererPrefab, this.transform, true);
            _steps.Add(step);
            StartCoroutine(AnimatePath(path, step));
        }

        private IEnumerator AnimatePath(List<Vector2> path, LineRenderer step)
        {
            step.positionCount = 0;

            var color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
            step.startColor = color;
            step.endColor = color;
            
            for (int i = 0; i < path.Count; i++)
            {
                step.positionCount = i + 1;
                step.SetPosition(i, path[i]);
                yield return new WaitForSeconds(_delay);
            }
        }
    }
}