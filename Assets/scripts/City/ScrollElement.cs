using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollElement : MonoBehaviour
{
    public MainManager mainManager;
    public LVLUpper lVLUpper;
    public GameObject cell;
    public Building building;

    void OnMouseDown()
    {
        HandleClick();
    }

    private void HandleClick()
    {
        mainManager.AddNewBuilding(this);
    }
}

