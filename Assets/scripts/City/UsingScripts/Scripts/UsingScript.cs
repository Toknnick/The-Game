using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UsingScript : MonoBehaviour
{
    public abstract void Use(ScrollElement scrollElement,int index);
    public abstract void Upgrade(BuildingManager buildingManager);
    [HideInInspector] public MainManager mainManager;

    [HideInInspector] public float gpm;
    [HideInInspector] public float addingGpm;

    [HideInInspector] public float minPercent;
    [HideInInspector] public float maxPercent;
    [HideInInspector] public float addingPercent;
    [HideInInspector] public float coastForOneExperement;
}
