using Project.Features.Cameras;
using Project.Features.LoopsExplorer.Scripts;
using TMPro;
using UnityEngine;

public class LoopCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI loopFoundDisplay;
    [SerializeField] private GameObject levelChoserMenu;
    [SerializeField] private GameObject levelMenu;

    void Start()
    {
        LoopsExplorerManager.Instance.FoundLoop += UpdateFoundLoopsDisplay;
        LoopsExplorerManager.Instance.NewDataInitialized += OnNewData;
        UpdateFoundLoopsDisplay(0);
    }

    private void OnNewData()
    {
        UpdateFoundLoopsDisplay(0);
    }

    private void UpdateFoundLoopsDisplay(int Zatichka)
    {
        loopFoundDisplay.text = $"{LoopsExplorerManager.Instance.FoundLoops}/{LoopsExplorerManager.Instance.TotalLoops}";

        if (LoopsExplorerManager.Instance.FoundLoops == LoopsExplorerManager.Instance.TotalLoops)
        {
            CameraManager.Instance.Show();
        }
    }
}