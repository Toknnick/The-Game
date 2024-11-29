using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingInfoPanel : MonoBehaviour
{
    [SerializeField] private Image BuildingImage;
    [SerializeField] private TextMeshProUGUI GpmText;
    [SerializeField] private Button UpgradeButton;
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
        buildingManager.mainManager.Upgrade(buildingManager);
        ClosePanel();
    }

    public void StartExperiment()
    {

    }

    private void setInfo()
    {
        BuildingInfoText.gameObject.SetActive(false);
        BuildingImage.sprite = buildingManager.building.sprites[buildingManager.nowLVL - 1];

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
    }
}
