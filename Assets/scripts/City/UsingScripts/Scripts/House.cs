using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class House : UsingScript
{
    public override void Use(ScrollElement scrollElement,int index)
    {
        mainManager.ChangeGPM(gpm,false);

        if (scrollElement.building.maxLVL > 1)
        {
            scrollElement.buildingManager = scrollElement.cell.AddComponent<BuildingManager>();
            scrollElement.buildingManager.index = index;
            scrollElement.buildingManager.mainManager = mainManager;
            scrollElement.buildingManager.building = scrollElement.building;
            scrollElement.buildingManager.cell = scrollElement.cell;
            scrollElement.buildingManager.nowGPM = gpm;
            scrollElement.buildingManager.nowLVL = 1;
            Destroy(scrollElement.cell.GetComponent<Cell>());
        }

    }

    public override void Upgrade(BuildingManager buildingManager)
    {
        mainManager.ChangeGPM(addingGpm, false);
        buildingManager.nowGPM += addingGpm;
        buildingManager.building.lvlUpCoast += buildingManager.building.lvlUpCoast + buildingManager.building.coast;
        buildingManager.nowLVL += 1;
        Debug.Log($"Changed Lvl: {buildingManager.nowLVL}");
    }
}
