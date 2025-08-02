using System;
using Project.Features.LevelSaver;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelLoaderButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private TMP_InputField _inputedInfo;

    private void Start()
    {
        _button.onClick.AddListener(DrawLevel);
    }

    private void DrawLevel()
    {
        var text = _inputedInfo.text;
        Debug.Log(text);
        var level = LevelDatabase.Instance.GetLevel(Convert.ToInt32(text)).points;
        LineDrawerFromDotsManager.Instance.DrawLine(level);
    }
}
