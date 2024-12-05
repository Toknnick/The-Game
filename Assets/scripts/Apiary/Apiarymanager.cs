using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Apiarymanager : MonoBehaviour
{
    [SerializeField] private Balancer balancer;
    [SerializeField] private ApiManager apiManager;
    [SerializeField] private TextMeshProUGUI goldText;
    public SpeakingHeadmanager speakingHead;
    [SerializeField] private GameObject plug;
    [SerializeField] private TextOfSpeakHead firstTimeText;
    [SerializeField] private TextOfSpeakHead canGetGoldText;
    [SerializeField] private TextOfSpeakHead notCanGetGoldText;
    [SerializeField] private Button getGoldButton;

    private float nowGPM = 0;
    private float nowGold = 0;
    private bool isGetResUserFromAPI = false;
    private bool isGetResShopFromAPI = false;
    private string date;

    void Start()
    {
        balancer.userName = PlayerPrefs.GetString("PlayerName", "DefaultName");
        balancer.shopName = $"{balancer.userName}`s SHOP";
        plug.SetActive(true);
        goldText.text = nowGold.ToString();
        StartCoroutine(SetGold());
        CheckCanGetGold();
    }

    public void GetGold()
    {
        float gold = balancer.countOfHives * balancer.goldForOne;
        System.DateTime currentDate = System.DateTime.Now;
        getGoldButton.enabled = false;

        // Сохраняем текущую дату
        PlayerPrefs.SetString("SavedDate", currentDate.ToString("yyyy-MM-dd HH:mm:ss"));
        PlayerPrefs.Save();

        ChangeMoney($"Игрок собрал мед на пасеке: всего ульев: {balancer.countOfHives}", -gold);
    }

    private void CheckCanGetGold()
    {
        System.DateTime currentDate = System.DateTime.Now;

        if (PlayerPrefs.HasKey("SavedDate"))
        {
            string savedDate = PlayerPrefs.GetString("SavedDate");
            Debug.Log("Сохраненная дата " + savedDate);
            Debug.Log("Сейчас " + currentDate);

            if (System.DateTime.TryParse(savedDate, out System.DateTime parsedDate))
            {
                System.TimeSpan difference = currentDate - parsedDate;
                Debug.Log("Разница " + difference.TotalDays.ToString());

                if (difference.TotalDays > 1) // Если разница больше 1 дня
                {
                    getGoldButton.enabled = true;
                    canGetGoldText.SetText(speakingHead);
                }
                else
                {
                    getGoldButton.enabled = false;
                    notCanGetGoldText.SetText(speakingHead);
                }
            }
        }
        else
        {
            getGoldButton.enabled = true;
            firstTimeText.SetText(speakingHead);
        }
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

    public void ChangeMoney(string comment, float money)
    {
        //TODO: отправить в API инфу
        nowGold -= money;
        goldText.text = Mathf.FloorToInt(nowGold).ToString();

        if (comment != "")
        {
            string str = "";

            if (money > 0)
                str = $"-{money}";
            else
                str = $"+{money * -1}";

            var resourcesChanged = new Dictionary<string, string>
            {
                { "gold_changed", str }
            };

            if (money != 0)
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

    private void SendUserLog(string comment, Dictionary<string, string> resourcesChanged)
    {
        apiManager.SendLog(comment, balancer.userName, resourcesChanged);
    }
}
