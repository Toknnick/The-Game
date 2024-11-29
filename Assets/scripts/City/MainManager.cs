using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    [SerializeField] private int startGold = 1000;
    [SerializeField] private BuildingInfoPanel buildingInfoPanel;
    [SerializeField] private GameObject scrollView;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private Transform scrollViewContent;
    [SerializeField] private List<Building> buildings;

    private int nowGPM = 0;
    private int nowGold = 0;
    void Start()
    {
        nowGold = startGold;
        goldText.text = nowGold.ToString();
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
            yield return new WaitForSeconds(60); // Ждём 1 минуту
            nowGold += nowGPM;
            goldText.text = nowGold.ToString();
            Debug.Log($"Gold: {nowGold}");
            //TODO: отправить в API инфу
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
            layout.spacing = 10;
            buildingObject.transform.SetParent(scrollViewContent, false);

            // Добавляем компонент Image
            GameObject imageObject = new GameObject("Image");
            Image image = imageObject.AddComponent<Image>();
            image.sprite = building.sprites[0];
            image.preserveAspect = true;
            imageObject.transform.SetParent(buildingObject.transform, false);

            // Добавляем компонент TextMeshProUGUI
            GameObject textObject = new GameObject("BuildingText");
            TextMeshProUGUI textComponent = textObject.AddComponent<TextMeshProUGUI>();
            textComponent.text = building.gpm.ToString();
            textComponent.alignment = TextAlignmentOptions.Center;
            textObject.transform.SetParent(buildingObject.transform, false);

            // Добавляем BoxCollider2D
            BoxCollider2D boxCollider = buildingObject.AddComponent<BoxCollider2D>();
            boxCollider.size = new Vector2(70, 100); // Примерный размер
            buildingObject.transform.localScale = new Vector3(2,2,2);

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
            //TODO: отправить в API инфу
            nowGold -= scrollElement.building.coast;
            goldText.text = nowGold.ToString();
            scrollElement.cell.GetComponent<SpriteRenderer>().sprite = scrollElement.building.sprites[0];
            scrollElement.building.usingScript.Use(scrollElement);
            scrollView.SetActive(false);
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
                //TODO: отправить в API инфу
                nowGold -= buildingManager.building.lvlUpCoast;

                if(buildingManager.building.sprites[buildingManager.nowLVL] != null)
                    buildingManager.cell.GetComponent<SpriteRenderer>().sprite = buildingManager.building.sprites[buildingManager.nowLVL];

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

    public void ChangeGPM(int gpm)
    {
        //TODO: отправить в API инфу
        nowGPM += gpm;
        Debug.Log($"Changed gpm: {nowGPM}");
    }

}
