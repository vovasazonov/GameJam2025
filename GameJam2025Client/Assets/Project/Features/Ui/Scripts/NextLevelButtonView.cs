using System;
using Project.Features.LoopsExplorer.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Project.Features.Ui.Scripts
{
    public class NextLevelButtonView : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private GameObject _content;
        
        private void Start()
        {
            LoopsExplorerManager.Instance.FoundLoop += OnFoundLoop;
            LoopsExplorerManager.Instance.NewDataInitialized += OnNewDataInitialized;
        }

        private void OnNewDataInitialized()
        {
            _content.gameObject.SetActive(false);
        }

        private void OnFoundLoop(int id)
        {
            var nextLevel = LevelManager.Instance.CurrentLevel+ 1;
            var isExistNextLevel = LevelManager.Instance.AllLevels().Contains(nextLevel);
            var areAllLoopsFound = LoopsExplorerManager.Instance.FoundLoops == LoopsExplorerManager.Instance.TotalLoops;
            if (isExistNextLevel && areAllLoopsFound)
            {
                _content.gameObject.SetActive(true);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (LoopsExplorerManager.Instance.FoundLoops == LoopsExplorerManager.Instance.TotalLoops)
            {
                var nextLevel = LevelManager.Instance.CurrentLevel+ 1;
                var isExistNextLevel = LevelManager.Instance.AllLevels().Contains(nextLevel);

                if (isExistNextLevel)
                {
                    LevelManager.Instance.LoadLevel(nextLevel);
                }
            }
        }
    }
}