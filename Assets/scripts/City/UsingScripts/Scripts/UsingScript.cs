using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UsingScript : MonoBehaviour
{
    public abstract void Use(ScrollElement scrollElement);
    public abstract void Upgrade(BuildingManager buildingManager);
    [HideInInspector] public MainManager mainManager;

    [HideInInspector] public float gpm;
    [HideInInspector] public float addingGpm;

    [HideInInspector] public int minPercent;
    [HideInInspector] public int maxPercent;
    [HideInInspector] public int addingPercent;
    [HideInInspector] public int coastForOneExperement;
}
