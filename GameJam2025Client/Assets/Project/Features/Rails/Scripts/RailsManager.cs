using Project.Core.Scripts;
using UnityEngine;

namespace Project.Features.Rails.Scripts
{
    public class RailsManager : SingletonBehaviour<RailsManager>
    {
        [SerializeField] private RailsView _railsPrefab;

        public RailsView CreateRails()
        {
            var rails = Instantiate(_railsPrefab, transform);
            return rails;
        }
    }
}