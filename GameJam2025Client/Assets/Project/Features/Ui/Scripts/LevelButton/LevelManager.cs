using System.Collections.Generic;
using Project.Core.Scripts;
using Project.Features.Ui.Scripts.LevelButton;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager: SingletonBehaviour<LevelManager>
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject levelMenu;
    [SerializeField] private List<LevelButton> _levelButtons;

    public void LoadLevel(int id)
    {
        mainMenu.SetActive(false);
        levelMenu.SetActive(false);
        var points = LevelDatabase.Instance.GetLevel(id).points;
        LineDrawerFromDotsManager.Instance.DrawLine(points);
    }
}
