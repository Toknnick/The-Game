using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingInfoPanel : MonoBehaviour
{
    [SerializeField] private MainManager mainManager;

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Image BuildingImage;
    [SerializeField] private TextMeshProUGUI GpmText;
    [SerializeField] private Button UpgradeButton;
    [SerializeField] private TextMeshProUGUI UpgradeCoastText;
    [SerializeField] private TextMeshProUGUI BuildingInfoText;
    [SerializeField] private Button StartExperemnt;

    private BuildingManager buildingManager;

    public void GetBuildingManager(BuildingManager buildingManager)
    {
        this.buildingManager = buildingManager;
        SetInfo();
    }

    public void ClosePanel()
    {
        this.gameObject.SetActive(false);
    }

    public void Upgrade()
    {
        buildingManager.mainManager.Upgrade(buildingManager);
        ClosePanel();
    }

    public void StartExperiment()
    {
        mainManager.ChangeMoney("Начат эксперемент в лаборатории",buildingManager.building.usingScript.coastForOneExperement);
        float gold = Random.Range(buildingManager.building.usingScript.minPercent, buildingManager.building.usingScript.maxPercent) / 100.0f;
        mainManager.ChangeGPM(gold,true);
        ClosePanel();
    }

    private void SetInfo()
    {
        nameText.gameObject.SetActive(true);

        if (buildingManager.building.type == Building.BuildingType.House)
            nameText.text = "Дом\nУровень: " + buildingManager.nowLVL;
        else if (buildingManager.building.type == Building.BuildingType.Laboratory)
            nameText.text = "Лаборатория\nУровень: " + buildingManager.nowLVL;
        else if (buildingManager.building.type == Building.BuildingType.CityHall)
            nameText.text = "Общий дом\nУровень: " + buildingManager.nowLVL;
        else
            nameText.gameObject.SetActive(false);

        BuildingImage.sprite = buildingManager.building.sprites[buildingManager.nowLVL - 1];
        StartExperemnt.gameObject.SetActive(false);
        BuildingInfoText.gameObject.SetActive(false);

        if (buildingManager.nowGPM > 0)
        {
            GpmText.text = "Меда в минуту: " + buildingManager.nowGPM.ToString();
            GpmText.gameObject.SetActive(true);
        }
        else
        {
            GpmText.gameObject.SetActive(false);
        }

        if(buildingManager.nowLVL < buildingManager.building.maxLVL)
        {
            UpgradeCoastText.text = "Стоймость улучшения: " + buildingManager.building.lvlUpCoast.ToString();
            UpgradeButton.enabled = true;
            UpgradeButton.GetComponentInChildren<TextMeshProUGUI>().text = "Улучшить";
        }
        else
        {
            UpgradeCoastText.text = "Уровень максимальный";
            UpgradeButton.enabled = false;
            UpgradeButton.GetComponentInChildren<TextMeshProUGUI>().text = "max";
        }

        if(buildingManager.building.type == Building.BuildingType.Laboratory)
        {
            StartExperemnt.gameObject.SetActive(true);
            BuildingInfoText.gameObject.SetActive(true);
            BuildingInfoText.text = $"Для проведения опыта требуется {buildingManager.building.usingScript.coastForOneExperement} меда,\r\nрезультат улучшения: на {buildingManager.building.usingScript.minPercent}%-{buildingManager.building.usingScript.maxPercent}% меда в минуту";
        }
        if (buildingManager.building.type == Building.BuildingType.CityHall)
        {
            BuildingInfoText.gameObject.SetActive(true);
            BuildingInfoText.text = $"Скоро здесь будет проводиться голосование...";
        }
    }
}
