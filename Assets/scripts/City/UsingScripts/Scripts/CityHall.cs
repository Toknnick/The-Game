using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityHall : UsingScript
{
    private MainManager mainManager;
    void FirstStart()
    {
        mainManager = GameObject.Find("Canvas(MainManager)").GetComponent<MainManager>();
    }

    public override void Use(ScrollElement scrollElement)
    {
        FirstStart();
    }

    public override void Upgrade(BuildingManager buildingManager)
    {
        FirstStart();
    }
}
