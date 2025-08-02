using System.Collections.Generic;
using UnityEngine;

namespace Project.Features.Trains.Scripts
{
    public class TrainView : MonoBehaviour
    {
        private CarriageView[] _carriages;

        public void StartFollowLoop(List<Vector2> loop)
        {
            _carriages = GetComponentsInChildren<CarriageView>();
        }
    }
}