using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class House : UsingScript
{
    private MainManager mainManager;
    void FirstStart()
    {
        mainManager = GameObject.Find("Canvas(MainManager)").GetComponent<MainManager>();
    }

    public override void Use(ScrollElement scrollElement)
    {
        FirstStart();
        mainManager.ChangeGPM(scrollElement.building.gpm);

        if (scrollElement.building.maxLVL > 1)
        {
            scrollElement.buildingManager = scrollElement.cell.AddComponent<BuildingManager>();
            scrollElement.buildingManager.mainManager = mainManager;
            scrollElement.buildingManager.building = scrollElement.building;
            scrollElement.buildingManager.cell = scrollElement.cell;
            scrollElement.buildingManager.nowGPM = scrollElement.building.gpm;
            scrollElement.buildingManager.nowLVL = 1;
            Destroy(scrollElement.cell.GetComponent<Cell>());
        }

        Destroy(scrollElement.cell.GetComponent<ScrollElement>());
    }

    public override void Upgrade(BuildingManager buildingManager)
    {
        FirstStart();
        mainManager.ChangeGPM(buildingManager.building.addingGpm);
        buildingManager.nowGPM += buildingManager.building.addingGpm;
        buildingManager.nowLVL += 1;
        Debug.Log($"Changed Lvl: {buildingManager.nowLVL}");
    }
}
