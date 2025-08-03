using System.Collections.Generic;
using Project.Core.Scripts;
using Project.Features.Cameras;
using Project.Features.LoopsExplorer.Scripts;
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
    
    public int CurrentLevel { get; private set; }
    
    public List<int> AllLevels()
    {
        return LevelDatabase.Instance.GetAllLevelIds();
    }
    
    public void LoadLevel(int id)
    {
        CurrentLevel = id;
        var data = LevelDatabase.Instance.GetLevel(id);
        var points = data.points;
        
        if (id == 1 || id == 3)
        {
            TutorialManager.Instance.StartTutorial(points);
        }

        CameraManager.Instance.Reset();
        mainMenu.SetActive(false);
        this.GameObject().SetActive(false);
        levelMenu.SetActive(true);
        LineDrawerFromDotsManager.Instance.DrawLine(points);
        LoopsExplorerManager.Instance.InitializeExplorer(points, data.cycles);
    }
}