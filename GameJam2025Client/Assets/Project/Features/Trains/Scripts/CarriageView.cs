using UnityEngine;

namespace Project.Features.Trains.Scripts
{
    [RequireComponent(typeof(Animation))]
    public class CarriageView : MonoBehaviour
    {
        private Animation _rideAnimation;

        private void Awake()
        {
            _rideAnimation = GetComponent<Animation>();
        }

        public void PlayRideAnimation()
        {
            _rideAnimation.Play();
        }
    }
}