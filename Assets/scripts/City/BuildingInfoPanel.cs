using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

public class BuildingInfoPanel : MonoBehaviour
{
    [SerializeField] private Image BuildingImage;
    [SerializeField] private TextMeshProUGUI GpmText;
    [SerializeField] private TextMeshProUGUI UpgradeCoastText;
    [SerializeField] private TextMeshProUGUI BuildingInfoText;

    private BuildingManager buildingManager;

    public void GetBuildingManager(BuildingManager buildingManager)
    {
        this.buildingManager = buildingManager;
        setInfo();
    }

    public void ClosePanel()
    {
        this.gameObject.SetActive(false);
    }

    public void Upgrade()
    {
        buildingManager.building.usingScript.Upgrade(buildingManager);
        ClosePanel();
    }

    public void StartExperiment()
    {

    }

    private void setInfo()
    {

    }
}
