using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Saver : MonoBehaviour
{
    [SerializeField] private MainManager mainManager;
    private string saveFilePath;

    private void Awake()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "buildings.json");
    }

    public void SaveBuildings(List<BuildingManager> buildings)
    {
        var data = new BuildingsData();

        foreach (var building in buildings)
        {
            var buildingData = new BuildingData
            {
                index = building.index,
                buildingType = building.building.type.ToString(),
                level = building.nowLVL,
                maxLvl = building.building.maxLVL,
                cost = building.building.coast,
                LvlUpCoast = building.building.lvlUpCoast,
                gpm = building.nowGPM,
                addingGpm = building.building.usingScript.addingGpm,
                minPercent = building.building.usingScript.minPercent,
                maxPercent = building.building.usingScript.maxPercent,
                lvlUpPercent = building.building.usingScript.addingPercent,
                costForOneExperiment = building.building.usingScript.coastForOneExperement,
                spriteNames = ConvertSpritesToNames(building.building.sprites)
            };
            data.buildings.Add(buildingData);
        }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log($"Buildings saved to {saveFilePath}");
    }

    public List<BuildingManager> LoadBuildings()
    {
        if (!File.Exists(saveFilePath))
        {
            Debug.LogWarning("No save file found.");
            return new List<BuildingManager>();
        }

        string json = File.ReadAllText(saveFilePath);
        var data = JsonUtility.FromJson<BuildingsData>(json);

        var buildingsdata = new List<BuildingManager>();
        foreach (var buildingData in data.buildings)
        {
            var buildingManager = new BuildingManager
            {
                index = buildingData.index,
                nowGPM = buildingData.gpm,
                nowLVL = buildingData.level,
                mainManager = mainManager
            };

            // Создаем объект здания
            GameObject newBuildingObject = new GameObject("BuildingObject");
            Building buildingScript = newBuildingObject.AddComponent<Building>();
            buildingManager.building = buildingScript;
            buildingManager.building.type = Enum.Parse<Building.BuildingType>(buildingData.buildingType);
            buildingManager.building.sprites = LoadSpritesByName(buildingData.spriteNames);
            buildingManager.building.coast = buildingData.cost;
            buildingManager.building.maxLVL = buildingData.maxLvl;
            buildingManager.building.lvlUpCoast = buildingData.LvlUpCoast;

            // Создаем объект UsingScript
            GameObject newUsingScriptObject = new GameObject("UsingScript");
            UsingScript usingScriptObject = UsingScriptFactory.CreateUsingScript(buildingManager.building.type);
            usingScriptObject.mainManager = mainManager;
            usingScriptObject.gpm = buildingData.gpm;
            usingScriptObject.addingGpm = buildingData.addingGpm;
            usingScriptObject.minPercent = buildingData.minPercent;
            usingScriptObject.maxPercent = buildingData.maxPercent;
            usingScriptObject.addingPercent = buildingData.lvlUpPercent;
            usingScriptObject.coastForOneExperement = buildingData.costForOneExperiment;

            buildingManager.building.usingScript = usingScriptObject;
            buildingsdata.Add(buildingManager);
        }

        Debug.Log("Buildings loaded.");
        return buildingsdata;
    }

    private List<string> ConvertSpritesToNames(List<Sprite> sprites)
    {
        var namesList = new List<string>();
        foreach (var sprite in sprites)
        {
            if (sprite != null)
            {
                namesList.Add(sprite.name);
            }
        }
        return namesList;
    }

    private List<Sprite> LoadSpritesByName(List<string> spriteNames)
    {
        var sprites = new List<Sprite>();
        foreach (var spriteName in spriteNames)
        {
            var sprite = Resources.Load<Sprite>($"Sprites/{spriteName}");
            if (sprite != null)
            {
                sprites.Add(sprite);
            }
            else
            {
                Debug.LogError($"Sprite {spriteName} not found in Resources/Sprites");
            }
        }
        return sprites;
    }
}

[Serializable]
public class BuildingData
{
    public int index;
    public string buildingType; // Значение енама в строковом виде
    public int level;
    public int maxLvl;
    public float cost;
    public float LvlUpCoast;
    public float gpm;
    public float addingGpm;
    public float minPercent;
    public float maxPercent;
    public float lvlUpPercent;
    public float costForOneExperiment;
    public List<string> spriteNames; // Для хранения списка имен спрайтов
}

[Serializable]
public class BuildingsData
{
    public List<BuildingData> buildings = new List<BuildingData>();
}

