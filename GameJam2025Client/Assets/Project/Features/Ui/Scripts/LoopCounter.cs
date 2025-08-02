using System;
using Project.Features.LoopsExplorer.Scripts;
using TMPro;
using UnityEngine;

public class LoopCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI loopFoundDisplay;

    void Start()
    {
        LoopsExplorerManager.Instance.FoundLoop += UpdateFoundLoopsDisplay;
    }

    private void UpdateFoundLoopsDisplay(int Zatichka)
    {
        loopFoundDisplay.text = Convert.ToString(LoopsExplorerManager.Instance.FoundLoops) + " found from" + " " +
                                LoopsExplorerManager.Instance.TotalLoops;
    }
}