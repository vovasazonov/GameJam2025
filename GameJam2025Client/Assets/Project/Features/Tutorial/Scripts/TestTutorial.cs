using UnityEngine;

namespace Project.Features.Tutorial.Scripts
{
    public class TestTutorial : MonoBehaviour
    {
        private void OnEnable()
        {
            var points = PaintManager.Instance.CurrentPoints;
            if (points == null || points.Count < 2)
            {
                return;
            }
            TutorialManager.Instance.StartTutorial(points);
        }

        private void OnDisable()
        {
            TutorialManager.Instance.StopTutorial();
        }
    }
}