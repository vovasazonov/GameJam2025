using System;
using System.Collections.Generic;
using Project.Core.Scripts;
using Project.Features.LineCalculation.Scripts;
using UnityEngine;

namespace Project.Features.LevelSaver
{
    public class SaveLevelManager : SingletonBehaviour<SaveLevelManager>
    {
        private void Start()
        {
            PaintManager.Instance.PointsUpdated += OnPointsUpdated;
        }

        private void OnDestroy()
        {
            PaintManager.Instance.PointsUpdated -= OnPointsUpdated;
        }


        public void OnPointsUpdated()
        {
            List<Vector2> points = PaintManager.Instance.CurrentPoints;
            var cycles = LineCalculationManager.Instance.FindLoops(points);
            LevelDatabase.Instance.AddLevel(points, cycles);
        }
    }
}