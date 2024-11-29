using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laboratory : UsingScript
{
    public override void Use(ScrollElement scrollElement)
    {
        if (scrollElement.building.maxLVL > 1)
        {
            scrollElement.buildingManager = scrollElement.cell.AddComponent<BuildingManager>();
            scrollElement.buildingManager.mainManager = mainManager;
            scrollElement.buildingManager.building = scrollElement.building;
            scrollElement.buildingManager.cell = scrollElement.cell;
            scrollElement.buildingManager.nowLVL = 1;
            Destroy(scrollElement.cell.GetComponent<Cell>());
        }
    }

    public override void Upgrade(BuildingManager buildingManager)
    {
        buildingManager.nowGPM += addingGpm;
        buildingManager.building.lvlUpCoast += buildingManager.building.lvlUpCoast + buildingManager.building.coast;
        buildingManager.nowLVL += 1;
        buildingManager.building.usingScript.minPercent += buildingManager.building.usingScript.addingPercent;
        buildingManager.building.usingScript.maxPercent += buildingManager.building.usingScript.addingPercent;
        Debug.Log($"Changed Lvl: {buildingManager.nowLVL}");
    }
}
