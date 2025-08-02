using System;
using Project.Features.LoopsExplorer.Scripts;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class LoopCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI loopFoundDisplay;
    [SerializeField] private GameObject levelChoserMenu;
    [SerializeField] private GameObject levelMenu;

    void Start()
    {
        LoopsExplorerManager.Instance.FoundLoop += UpdateFoundLoopsDisplay;
    }

    private void UpdateFoundLoopsDisplay(int Zatichka)
    {
        loopFoundDisplay.text = Convert.ToString(LoopsExplorerManager.Instance.FoundLoops) + " found from" + " " +
                                LoopsExplorerManager.Instance.TotalLoops;

        if (LoopsExplorerManager.Instance.FoundLoops == LoopsExplorerManager.Instance.TotalLoops)
        {
            levelMenu.SetActive(false);
            levelChoserMenu.SetActive(true);
        }
    }
}