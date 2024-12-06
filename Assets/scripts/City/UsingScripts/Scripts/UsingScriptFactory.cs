using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UsingScriptFactory
{
    public static UsingScript CreateUsingScript(Building.BuildingType buildingType)
    {
        return buildingType switch
        {
            Building.BuildingType.House => new House(),
            Building.BuildingType.CityHall => new MainHouse(),
            Building.BuildingType.Laboratory => new Laboratory(),
            _ => throw new ArgumentException($"Неизвестный тип здания: {buildingType}")
        };
    }
}
