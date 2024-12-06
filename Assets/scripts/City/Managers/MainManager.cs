using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    [SerializeField] private Balancer balancer;
    [SerializeField] private ApiManager apiManager;
    [SerializeField] private BuildingInfoPanel buildingInfoPanel;
    [SerializeField] private GameObject scrollView;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private Transform scrollViewContent;
    [SerializeField] private GameObject prefabOfScrollElement;
    [Space]
    [SerializeField] private List<Building> buildings;
    public SpeakingHeadmanager speakingHead;
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private GameObject plug;
    [SerializeField] private List<GameObject> gjcells;

    [SerializeField] private TextOfSpeakHead TextOfSpeakHead;

    private float nowGPM = 0;
    private float nowGold = 0;
    private float nowGoldInShop = 0;
    private bool isGetResUserFromAPI = false;
    private bool isGetResShopFromAPI = false;

    public void OffCells()
    {
        foreach (var cell in gjcells)
        {
            cell.GetComponent<Collider2D>().enabled = false;
        }
    }

    public void OnCells()
    {
        foreach (var cell in gjcells)
        {
            cell.GetComponent<Collider2D>().enabled = true;
        }
    }

    public void EndTneGame(bool isWin,float gold)
    {
        if (isWin)
        {
            var resources = new Dictionary<string, string>
            {
                { "gold_changed", $"+{gold}" }
            };

            ChangeMoney($"Игрок прошел мини-игру, кол-во пчел: {balancer.mineCount}" , -gold);
        }
    }

    void Start()
    {
        balancer.userName = PlayerPrefs.GetString("PlayerName", "DefaultName");
        balancer.shopName = $"{balancer.userName}`s SHOP";
        plug.SetActive(true);
        goldText.text = nowGold.ToString();
        SetFromBalancer();
        //StartCoroutine(SetGold());
        apiManager.GetShopResources(balancer.userName, balancer.shopName);
        TextOfSpeakHead.SetText(speakingHead);
    }

    IEnumerator SetGold()
    {
        while (true)
        {
            yield return new WaitForSeconds(60); // Ждём 1 минуту

            string comment = "Ежеменутное добавление меда";
            ChangeMoney(comment, nowGPM * -1);
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
            GameObject buildingObject = Instantiate(prefabOfScrollElement, scrollViewContent);
            buildingObject.GetComponentInChildren<Image>().sprite = building.sprites[0];
            buildingObject.GetComponentInChildren<TextMeshProUGUI>().text = "Стоймость: " + building.coast.ToString();

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
            var resourcesShop = new Dictionary<string, string>
            {
                { "shops gold", $"+{scrollElement.building.coast }" }
            };
            nowGoldInShop += scrollElement.building.coast;
            string comment = $"Куплина постройка - {scrollElement.building.type}";
            ChangeMoney(comment, scrollElement.building.coast);
            goldText.text = nowGold.ToString();
            scrollElement.cell.GetComponent<SpriteRenderer>().sprite = scrollElement.building.sprites[0];
            scrollElement.cell.gameObject.name = scrollElement.building.type.ToString();
            scrollElement.building.usingScript.Use(scrollElement);
            scrollView.SetActive(false);

            string commentShop = "";

            if (scrollElement.building.type == Building.BuildingType.Laboratory)
            {
                buildings.Remove(scrollElement.building);
                commentShop = "Игрок купил лабораторию";
                resourcesShop[scrollElement.building.type.ToString()] = scrollElement.building.coast.ToString();
            }
            else
            {
                commentShop = "Игрок купил дом";
            }

            UpdateShop(commentShop, resourcesShop);
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
                soundManager.PlayLvlUp();
                var resourcesShop = new Dictionary<string, string>
                {
                    { "shops gold", $"+{buildingManager.building.lvlUpCoast}" }
                };

                nowGoldInShop += buildingManager.building.lvlUpCoast;
                string comment = $"Обновлена постройка - {buildingManager.building.type}";
                ChangeMoney(comment, buildingManager.building.lvlUpCoast);

                if (buildingManager.building.sprites[buildingManager.nowLVL - 1] != null)
                    buildingManager.cell.GetComponent<SpriteRenderer>().sprite = buildingManager.building.sprites[buildingManager.nowLVL - 1];

                buildingManager.building.usingScript.Upgrade(buildingManager);
                goldText.text = nowGold.ToString();

                string commentShop = "";


                if (buildingManager.building.type == Building.BuildingType.Laboratory)
                {
                    commentShop = "Игрок обновил лабораторию";
                }
                else if (buildingManager.building.type == Building.BuildingType.House)
                {
                    commentShop = "Игрок обновил дом";
                }
                else if (buildingManager.building.type == Building.BuildingType.CityHall)
                {
                    commentShop = "Игрок обновил общий дом";
                }

                UpdateShop(commentShop, resourcesShop);
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

        UpdatePlayer();
    }

    public void ChangeMoney(string comment, float money)
    {
        //TODO: отправить в API инфу
        nowGold -= money;
        goldText.text = Mathf.FloorToInt(nowGold).ToString();

        if (comment != "") {
            string str = "";

            if (money > 0)
                str = $"-{money}";
            else
                str = $"+{money * -1}";

            var resourcesChanged = new Dictionary<string, string>
            {
                { "gold_changed", str }
            };

            if(money != 0)
                SendUserLog(comment, resourcesChanged);
            else
                SendUserLog(comment, null);
        }

        UpdatePlayer();
    }

    public void SetResources(float gold, float gpm)
    {
        nowGold = gold;
        nowGPM = gpm;
        ChangeMoney("", 0);
        isGetResUserFromAPI = true;

        if (isGetResUserFromAPI && isGetResShopFromAPI)
            plug.SetActive(false);
    }

    public void SetShopResources(Dictionary<string, float> resources)
    {
        // Проверяем наличие ресурсов и присваиваем последнее значение как nowGoldInShop
        if (resources != null && resources.Count > 0)
        {
            var lastEntry = resources.Last();
            nowGoldInShop = lastEntry.Value;

            // Удаляем последний элемент из словаря, так как он не является зданием
            resources.Remove(lastEntry.Key);
        }

        List<Building> buildingsToRemove = new List<Building>();

        foreach (var building in buildings)
        {
            string buildingType = building.type.ToString();

            // Если типа здания нет в оставшихся ресурсах, добавляем в список на удаление
            if (!resources.ContainsKey(buildingType))
            {
                buildingsToRemove.Add(building);
            }
        }

        // Удаляем здания, которых нет в ресурсах
        foreach (var buildingToRemove in buildingsToRemove)
            buildings.Remove(buildingToRemove);

        isGetResShopFromAPI = true;

        if (isGetResUserFromAPI && isGetResShopFromAPI)
            plug.SetActive(false);

    }

    private void UpdatePlayer()
    {
        if (nowGPM != 0)
        {
            // Пример вызова создания игрока с ресурсами gold и gpm
            var resources = new Dictionary<string, float>
        {
            { "gold", nowGold },
            { "gpm", nowGPM }
        };

            apiManager.UpdatePlayer(balancer.userName, resources);
        }
    }

    private void UpdateShop(string comment, Dictionary<string, string> resourcesChanged)
    {
        var resourcesShop = new Dictionary<string, float>
        {
            { "shop`s gold", nowGoldInShop } // Сначала добавляем золото в магазин
        };

        foreach (var building in buildings)
        {
            // Используем тип здания в качестве ключа и записываем его данные (например, coast)
            string buildingKey = building.type.ToString();

            if (!resourcesShop.ContainsKey(buildingKey))
            {
                resourcesShop[buildingKey] = building.coast;
            }
            else
            {
                // Если уже есть запись с таким типом, добавляем стоимость
                resourcesShop[buildingKey] += building.coast;
            }
        }

        apiManager.UpdateShopResources(balancer.userName,balancer.shopName,resourcesShop);
        SendShopLog(comment, resourcesChanged);
    }

    private void SendUserLog(string comment, Dictionary<string, string> resourcesChanged)
    {
        apiManager.SendLog(comment, balancer.userName, resourcesChanged);
    }

    private void SendShopLog(string comment, Dictionary<string, string> resourcesChanged)
    {
        apiManager.CreateShopLog(comment,balancer.userName,balancer.shopName, resourcesChanged);
    }
}
