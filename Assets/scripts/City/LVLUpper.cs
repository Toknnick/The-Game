using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LVLUpper : MonoBehaviour
{
    public MainManager mainManager;
    public GameObject cell;
    public Building building;
    public int nowGPM;
    public int nowLVL;

    void OnMouseDown()
    {
        HandleClick();
    }

    private void HandleClick()
    {
        mainManager.Upgrade(this);
    }
}
