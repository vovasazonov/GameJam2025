using System.Collections;
using Project.Features.Audio.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Project.Features.Ui.Scripts.SubSections
{
    public class AudioToggleView : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image _onImage;
        [SerializeField] private Image _offImage;

        private void Start()
        {
            // UpdateToggleView();
            if (rectTransform == null)
                rectTransform = GetComponent<RectTransform>();
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
            
            StartMove(); // just matvey request
        }
        
        public RectTransform rectTransform;
        public float speed = 50f;

        public void StartMove()
        {
            StartCoroutine(MoveRightCoroutine());
        }

        private IEnumerator MoveRightCoroutine()
        {
            while (true)
            {
                rectTransform.anchoredPosition += Vector2.right * speed * Time.deltaTime;
                yield return null;
            }
        }
    }
}