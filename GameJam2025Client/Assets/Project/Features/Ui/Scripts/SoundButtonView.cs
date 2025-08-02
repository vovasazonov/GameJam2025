using System.Collections;
using UnityEngine;

namespace Project.Features.Ui.Scripts
{
    public class SoundButtonView : MonoBehaviour
    {
        public RectTransform rectTransform;
        public float speed = 50f;

        void Start()
        {
            if (rectTransform == null)
                rectTransform = GetComponent<RectTransform>();
        }

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