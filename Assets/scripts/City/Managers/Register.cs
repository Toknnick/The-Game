using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Register : MonoBehaviour
{
    [SerializeField] private string gameUuid;
    [SerializeField] private List<Building> buildings;
    [SerializeField] private Balancer balancer;
    [Space]
    [SerializeField] private GameObject panel;
    [SerializeField] private SpeakingHeadmanager sphead;
    [SerializeField] private TextOfSpeakHead textOfSpeakHead;
    [SerializeField] private TextOfSpeakHead textOfErrorSPHead;
    [SerializeField] private TextOfSpeakHead textOfNoPasswordErrorSPHead;
    [SerializeField] private TextOfSpeakHead textOfNoExitPasswordErrorSPHead;
    [SerializeField] private ApiManager apiManager;
    [SerializeField] private TMP_InputField textName;
    [SerializeField] private TMP_InputField textPassword;
    [Space]
    [SerializeField] private Button goCityButton;
    [SerializeField] private Button goApiaryButton;

    private string Name;
    private bool isSee;
    private int nowGoldInShop = 0;
    private string saveFilePath;
    private void Awake()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "buildings.json");
    }

    public void CloseGame()
    {
        Application.Quit();

        // Это сообщение будет видно только в редакторе
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    void Start()
    {
       /* PlayerPrefs.DeleteKey("PlayerName");
        PlayerPrefs.DeleteKey("SavedDate");
        PlayerPrefs.Save();*/
        

        Name = PlayerPrefs.GetString("PlayerName", "defNameOfHero123123121231231231231231$#$^$$@#@#&$%$^$^fdf231233123");

        if (Name == null || Name == "defNameOfHero123123121231231231231231$#$^$$@#@#&$%$^$^fdf231233123")
        {
            textOfSpeakHead.SetText(sphead);

            if (File.Exists(saveFilePath))
            {
                File.Delete(saveFilePath);
            }
        }
        else
        {
            goCityButton.gameObject.SetActive(true);
            goApiaryButton.gameObject.SetActive(true);
        }
    }

    public void IsSee(bool isSee)
    {
        this.isSee = isSee;
    }

    public void CheckIfPlayerExists()
    {
        Name = textName.text;

        string password = textPassword.text;
        if (password == null || password.Length == 0 || password == "")
        {
            textOfNoPasswordErrorSPHead.SetText(sphead);
            return;
        }


        StartCoroutine(CheckIfPlayerExistsCoro(Name));
    }

    private void Hide()
    {
        textName.text = "";
        textPassword.text = "";
    }

    private IEnumerator CheckIfPlayerExistsCoro(string playerName)
    {
        string url = $"https://2025.nti-gamedev.ru/api/games/{gameUuid}/players/";
        bool isCorrectPassword = false;

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                try
                {
                    // Десериализация ответа
                    var players = JsonConvert.DeserializeObject<List<PlayerInfo>>(request.downloadHandler.text);

                    bool exists = players.Exists(player =>
                    {
                        // Разделяем имя игрока на части: "<имя>|<пароль>"
                        string[] parts = player.name.Split('|');

                        if(parts.Length > 1)
                            isCorrectPassword = string.Equals(parts[1], textPassword.text, StringComparison.OrdinalIgnoreCase);

                        // Сравниваем первую часть (имя) с playerName, игнорируя регистр
                        return parts.Length > 0 && string.Equals(parts[0], playerName, StringComparison.OrdinalIgnoreCase);
                    });

                    // Вывод результата в консоль
                    if (exists & !isSee)
                    {
                        textOfErrorSPHead.SetText(sphead);
                        panel.gameObject.SetActive(false);
                        Hide();
                    }
                    if(isSee && exists)
                    {
                        if (isCorrectPassword)
                        {
                            Name = Name + "|" + textPassword.text;
                            PlayerPrefs.SetString("PlayerName", Name);
                            PlayerPrefs.Save();
                            panel.gameObject.SetActive(false);
                            goCityButton.gameObject.SetActive(true);
                            goApiaryButton.gameObject.SetActive(true);
                            Hide();
                        }
                        else
                        {
                            panel.gameObject.SetActive(false);
                            textOfNoExitPasswordErrorSPHead.SetText(sphead);
                            Hide();
                        }
                    }
                    if (!isSee && !exists)
                    {
                        CreateUser();
                    }
                    if(!exists && isSee)
                    {
                        panel.gameObject.SetActive(false);
                        textOfNoExitPasswordErrorSPHead.SetText(sphead);
                        Hide();
                    }
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"Ошибка при обработке ответа API: {ex.Message}");
                }
            }
            else
            {
                Debug.LogError($"Ошибка запроса: {request.responseCode}\n{request.error}");
            }
        }

    }

    private void CreateUser()
    {
        Name = Name + "|" + textPassword.text;
        PlayerPrefs.SetString("PlayerName", Name);
        PlayerPrefs.Save();

        // Пример вызова создания игрока с ресурсами gold и gpm
        var resourcesUser = new Dictionary<string, float>
        {
            { "gold", balancer.startGold },
            { "gpm", balancer.startGpm }
        };

        var resourcesShop = new Dictionary<string, float>
        {
            { "shops gold", nowGoldInShop } // Всегда сначала добавляем золото
        };

        // Добавляем здания в ресурсы
        foreach (var building in buildings)
        {
            string buildingKey = building.type.ToString();

            if (!resourcesShop.ContainsKey(buildingKey))
            {
                resourcesShop[buildingKey] = building.coast;
            }
            else
            {
                resourcesShop[buildingKey] += building.coast;
            }
        }

        var re = resourcesShop;
        apiManager.CreatePlayer(Name, resourcesUser);
        apiManager.CreateShop(Name, $"{Name}`s SHOP", resourcesShop);

        panel.gameObject.SetActive(false);
        goCityButton.gameObject.SetActive(true);
        goApiaryButton.gameObject.SetActive(true);
    }
}

[System.Serializable]
public class PlayerInfo
{
    public string name;
    public Dictionary<string, object> resources;
}
