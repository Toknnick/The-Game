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
            scrollElement.lVLUpper = scrollElement.cell.AddComponent<LVLUpper>();
            scrollElement.lVLUpper.mainManager = mainManager;
            scrollElement.lVLUpper.building = scrollElement.building;
            scrollElement.lVLUpper.cell = scrollElement.cell;
            scrollElement.lVLUpper.nowGPM = scrollElement.building.gpm;
            scrollElement.lVLUpper.nowLVL = 1;
            Destroy(scrollElement.cell.GetComponent<Cell>());
        }

        Destroy(scrollElement.cell.GetComponent<ScrollElement>());
    }

    public override void Upgrade(LVLUpper lVLUpper)
    {
        FirstStart();
        mainManager.ChangeGPM(lVLUpper.building.addingGpm);
        lVLUpper.nowGPM += lVLUpper.building.addingGpm;
        lVLUpper.nowLVL += 1;
        Debug.Log($"Changed Lvl: {lVLUpper.nowLVL}");
    }
}
