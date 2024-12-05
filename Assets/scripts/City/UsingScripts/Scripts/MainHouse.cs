using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainHouse : UsingScript
{
    public override void Use(ScrollElement scrollElement)
    {
        
    }

    public override void Upgrade(BuildingManager buildingManager)
    {
        buildingManager.building.lvlUpCoast += buildingManager.building.lvlUpCoast + buildingManager.building.coast;
        buildingManager.nowLVL += 1;
        Debug.Log($"Changed Lvl: {buildingManager.nowLVL}");
    }
}
