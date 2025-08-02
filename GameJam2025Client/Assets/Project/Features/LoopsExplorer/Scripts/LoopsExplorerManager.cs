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

        private readonly HashSet<int> _foundLoopsIds = new HashSet<int>();
        private List<Vector2> _fullLine = new List<Vector2>();
        private List<List<Vector2>> _toFindLoops = new List<List<Vector2>>();

        private bool _isDown;

        public void InitializeExplorer(List<Vector2> fullLine, List<List<Vector2>> loops)
        {
            _fullLine = fullLine;
            _toFindLoops = loops;
            _foundLoopsIds.Clear();
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