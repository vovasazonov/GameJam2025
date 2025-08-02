using System.Collections.Generic;
using System.Linq;
using Project.Core.Scripts;
using Project.Features.Input.Scripts;
using Project.Features.LineCalculation.Scripts;
using UnityEngine;

namespace Project.Features.LoopsExplorer.Scripts
{
    public class LoopsExplorerManager : SingletonBehaviour<LoopsExplorerManager>
    {
        [SerializeField] private float _radiusFinger = 10f;
        [SerializeField] private float _matchThreshold = 0.8f;

        private readonly Dictionary<int, List<Vector2>> _allLoopsById = new Dictionary<int, List<Vector2>>();
        private readonly HashSet<int> _foundLoopsIds = new HashSet<int>();
        private readonly List<Vector2> _fullLine = new List<Vector2>();

        private bool _isDown;

        public int TotalLoops => _allLoopsById.Count;
        public int FoundLoops => _foundLoopsIds.Count;
        
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
        
        private void Update()
        {
            if (InputManager.Instance.IsPointDown)
            {
                if (!_isDown)
                {
                    _isDown = true;
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
        }
    }
}