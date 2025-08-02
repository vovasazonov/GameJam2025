using System;
using Project.Features.LevelSaver;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelLoaderButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _inputedInfo;

    private void Start()
    {
        _button.onClick.AddListener(DrawLevel);
    }

    private void DrawLevel()
    {
        var level = LevelDatabase.Instance.GetLevel(Convert.ToInt32(_inputedInfo.text)).points;
        LineDrawerFromDotsManager.Instance.DrawLine(level);
    }
}
