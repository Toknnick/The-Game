using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    [SerializeField] private Balancer balancer;
    [SerializeField] private BuildingInfoPanel buildingInfoPanel;
    [SerializeField] private GameObject scrollView;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private Transform scrollViewContent;
    [Space]
    [SerializeField] private List<Building> buildings;
    [SerializeField] private SpeakingHeadmanager speakingHead;

    private float nowGPM = 0;
    private float nowGold = 0;
    void Start()
    {
        nowGold = balancer.startGold;
        nowGPM = balancer.startGpm;
        goldText.text = nowGold.ToString();
        SetFromBalancer();
        StartCoroutine(SetGold());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SetGold()
    {
        while (true)
        {
            yield return new WaitForSeconds(2); // Ждём 1 минуту
            nowGold += nowGPM;
            goldText.text = Mathf.FloorToInt(nowGold).ToString();
            Debug.Log($"Gold: {nowGold}");
            //TODO: отправить в API инфу
        }
    }

    private void SetFromBalancer()
    {
        foreach (var building in buildings)
        {
            building.usingScript.mainManager = this;

            if (building.type == Building.BuildingType.House)
            {
                building.coast = balancer.house_coast;
                building.maxLVL = balancer.house_maxLVL;
                building.lvlUpCoast = balancer.house_lvlUpCoast;
                building.usingScript.gpm = balancer.house_gpm;
                building.usingScript.addingGpm = balancer.house_addingGpm;
            }

            if (building.type == Building.BuildingType.Laboratory)
            {
                building.coast = balancer.laboratory_coast;
                building.maxLVL = balancer.laboratory_maxLVL;
                building.lvlUpCoast = balancer.laboratory_lvlUpCoast;
                building.usingScript.coastForOneExperement = balancer.laboratory_coastForOneExperement;
                building.usingScript.minPercent = balancer.laboratory_minPercent;
                building.usingScript.maxPercent = balancer.laboratory_maxPercent;
                building.usingScript.addingPercent = balancer.laboratory_addingPercent;
            }
        }
    }

    public void OpenBuildingInfoPanel(BuildingManager BuildingManager)
    {
        buildingInfoPanel.gameObject.SetActive(true);
        buildingInfoPanel.GetBuildingManager(BuildingManager);
    }

    public void ChooseNewBuilding(GameObject cell)
    {
        // Включаем ScrollView
        scrollView.SetActive(true);

        // Очищаем предыдущий контент
        foreach (Transform child in scrollViewContent)
        {
            Destroy(child.gameObject);
        }

        // Создаем элементы
        foreach (Building building in buildings)
        {
            // Создаем объект-контейнер
            GameObject buildingObject = new GameObject("BuildingElement");
            RectTransform containerRect = buildingObject.AddComponent<RectTransform>();
            VerticalLayoutGroup layout = buildingObject.AddComponent<VerticalLayoutGroup>();
            layout.childAlignment = TextAnchor.UpperCenter;
            layout.spacing = 15;

            // Добавляем компонент Image
            GameObject imageObject = new GameObject("Image");
            Image image = imageObject.AddComponent<Image>();
            image.sprite = building.sprites[0];
            image.preserveAspect = true;
            imageObject.transform.SetParent(buildingObject.transform, false);

            // Добавляем компонент TextMeshProUGUI
            GameObject textObject = new GameObject("BuildingText");
            TextMeshProUGUI textComponent = textObject.AddComponent<TextMeshProUGUI>();
            textComponent.text = "Цена: \n" + building.coast.ToString();
            textComponent.autoSizeTextContainer = true;
            textComponent.fontSizeMin = 0;
            textComponent.fontSizeMax = 38;
            textComponent.enableAutoSizing = true;
            textComponent.alignment = TextAlignmentOptions.Center;
            textObject.transform.SetParent(buildingObject.transform, false);

            // Добавляем BoxCollider2D
            BoxCollider2D boxCollider = buildingObject.AddComponent<BoxCollider2D>();
            boxCollider.size = new Vector2(70, 100); // Примерный размер
            buildingObject.transform.localScale = new Vector3(2,2,2);
            buildingObject.transform.SetParent(scrollViewContent, false);

            // Добавляем script
            ScrollElement scrollElement = buildingObject.AddComponent<ScrollElement>();
            scrollElement.mainManager = this;
            scrollElement.building = building;
            scrollElement.cell = cell;
        }
    }

    public void AddNewBuilding(ScrollElement scrollElement)
    {
        if (nowGold >= scrollElement.building.coast)
        {
            ChangeMoney(scrollElement.building.coast);
            goldText.text = nowGold.ToString();
            scrollElement.cell.GetComponent<SpriteRenderer>().sprite = scrollElement.building.sprites[0];
            scrollElement.cell.gameObject.name = scrollElement.building.type.ToString();
            scrollElement.building.usingScript.Use(scrollElement);
            scrollView.SetActive(false);

            if(scrollElement.building.type == Building.BuildingType.Laboratory)
            {
                buildings.Remove(scrollElement.building);
            }
        }
        else
        {
            scrollView.SetActive(false);
            Debug.Log($"нужно больше меда");
        }
    }

    public void Upgrade(BuildingManager buildingManager)
    {
        if (nowGold >= buildingManager.building.lvlUpCoast)
        {
            if (buildingManager.nowLVL != buildingManager.building.maxLVL)
            {
                ChangeMoney(buildingManager.building.lvlUpCoast);

                if(buildingManager.building.sprites[buildingManager.nowLVL-1] != null)
                    buildingManager.cell.GetComponent<SpriteRenderer>().sprite = buildingManager.building.sprites[buildingManager.nowLVL-1];

                buildingManager.building.usingScript.Upgrade(buildingManager);
                goldText.text = nowGold.ToString();
            }
        }
        else
        {
            scrollView.SetActive(false);
            Debug.Log($"нужно больше меда");
        }
    }

    public void ChangeGPM(float gpm, bool isLaba)
    {
        //TODO: отправить в API инфу
        if (!isLaba)
            nowGPM += gpm;
        else
            nowGold += nowGold * gpm;

        Debug.Log($"Changed gpm: {nowGPM}\n");
    }

    public void ChangeMoney(float money)
    {
        //TODO: отправить в API инфу
        nowGold -= money;
        Debug.Log($"Changed money: {nowGPM}\n");
    }

}
