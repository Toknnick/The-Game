using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private MainManager mainManager;
    void OnMouseDown()
    {
        HandleClick();
    }

    private void HandleClick()
    {
        mainManager.ChooseNewBuilding(this.gameObject);
    }
}
