using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public int index;
    public MainManager mainManager;
    public GameObject cell;
    public Building building;
    public float nowGPM;
    public int nowLVL;

    void OnMouseDown()
    {
        HandleClick();
    }

    private void HandleClick()
    {
        mainManager.OpenBuildingInfoPanel(this);
    }
}
