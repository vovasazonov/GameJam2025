using System.Collections.Generic;
using Project.Core.Scripts;
using Project.Features.LoopsExplorer.Scripts;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Project.Features.Trains.Scripts
{
    public class TrainsManager : SingletonBehaviour<TrainsManager>
    {
        [SerializeField] private List<TrainView> _trainsPrefabs;
        
        private readonly List<TrainView> _trains = new List<TrainView>();
        
        private void Start()
        {
            LoopsExplorerManager.Instance.FoundLoop += OnFoundLoop;
            LoopsExplorerManager.Instance.NewDataInitialized += OnNewDataInitialized;
        }

        private void OnFoundLoop(int loopId)
        {
            var indexPrefab = (LoopsExplorerManager.Instance.FoundLoops - 1) % _trainsPrefabs.Count;
            var prefab = _trainsPrefabs[indexPrefab];
            var view = Instantiate(prefab, transform);
            _trains.Add(view);
            
            var loop = LoopsExplorerManager.Instance.GetLoopById(loopId);
            view.StartFollowLoop(loop);
        }

        private void OnNewDataInitialized()
        {
            foreach (var train in _trains)
            {
                Object.Destroy(train.gameObject);
            }
            _trains.Clear();
        }
    }
}