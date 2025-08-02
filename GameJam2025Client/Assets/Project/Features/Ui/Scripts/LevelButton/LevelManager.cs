using System.Collections.Generic;
using Project.Core.Scripts;
using Project.Features.Tutorial.Scripts;
using Project.Features.Ui.Scripts.LevelButton;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : SingletonBehaviour<LevelManager>
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject levelMenu;
    [SerializeField] private List<LevelButton> _levelButtons;
    
    public void LoadLevel(int id)
    {
        var points = LevelDatabase.Instance.GetLevel(id).points;
        
        if (id == 1 || id == 3)
        {
            TutorialManager.Instance.StartTutorial(points);
        }

        mainMenu.SetActive(false);
        this.GameObject().SetActive(false);
        levelMenu.SetActive(true);
        LineDrawerFromDotsManager.Instance.DrawLine(points);
    }
}