using UnityEngine;
using UnityEngine.UI;

namespace Project.Features.Ui.Scripts.LevelButton
{
    public class LevelButton: MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private int id;

        private void Start()
        {
            _button.onClick.AddListener(LoadLevel); ;
        }

        private void LoadLevel()
        {
            LevelManager.Instance.LoadLevel(id);
        }
    }
}