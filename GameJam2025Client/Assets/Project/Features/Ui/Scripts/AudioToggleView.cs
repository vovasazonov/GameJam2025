using Project.Features.Audio.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Project.Features.Ui.Scripts
{
    public class AudioToggleView : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image _onImage;
        [SerializeField] private Image _offImage;

        private void Start()
        {
            UpdateToggleView();
        }

        private void UpdateToggleView()
        {
            _onImage.enabled = AudioManager.Instance.IsOn;
            _offImage.enabled = !AudioManager.Instance.IsOn;
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            AudioManager.Instance.IsOn = !AudioManager.Instance.IsOn;
            UpdateToggleView();
        }
    }
}