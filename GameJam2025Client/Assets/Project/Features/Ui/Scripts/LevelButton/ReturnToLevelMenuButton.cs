using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ReturnToLevelMenuButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private GameObject levelChoserMenu;

    private void Start()
    {
        _button.onClick.AddListener(ReturnToMenu);
    }

   private void ReturnToMenu()
    {
        levelChoserMenu.SetActive(true);
        this.GameObject().SetActive(false);
    }
}
