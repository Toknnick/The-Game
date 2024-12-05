using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ApiManager : MonoBehaviour
{
    [SerializeField] private MainManager mainManager;
    [SerializeField] private Balancer balancer;
    [SerializeField] private string gameUuid;

    private void Start()
    {
        if (balancer != null)
        {
            balancer.userName = PlayerPrefs.GetString("PlayerName", "DefaultName");
            StartCoroutine(GetPlayerResourcesCoro(balancer.userName));
        }
    }

    public void CreateShopLog(string comment, string playerName, string shopName, Dictionary<string, string> resourcesChanged)
    {
        StartCoroutine(CreateShopLogCoro(comment, playerName, shopName, resourcesChanged));
    }

    public void GetShopResources(string username, string shopName)
    {
        StartCoroutine(GetShopResourcesCoro(username, shopName));
    }

    public void SendLog(string comment, string playerName, Dictionary<string, string> resourcesChanged)
    {
        StartCoroutine(SendLogCoro(comment, playerName, resourcesChanged));
    }

    public void CreatePlayer(string name, Dictionary<string, float> resources = null)
    {
        StartCoroutine(CreatePlayerCoro(name, resources));
    }

    public void UpdatePlayer(string name, Dictionary<string, float> resources)
    {
        StartCoroutine(UpdatePlayerResourcesCoro(name, resources));
    }

    public void CreateShop(string username, string shopName, Dictionary<string, float> resources = null)
    {
        StartCoroutine(CreateShopCoro(username, shopName, resources));
    }

    public void UpdateShopResources(string username, string shopName, Dictionary<string, float> resources)
    {
        StartCoroutine(UpdateShopResourcesCoro(username, shopName, resources));
    }

    private IEnumerator CreatePlayerCoro(string name, Dictionary<string, float> resources = null)
    {
        // Формируем URL
        string url = $"https://2025.nti-gamedev.ru/api/games/{gameUuid}/players/";

        // Формируем тело запроса
        var requestBody = new Dictionary<string, object>
        {
            { "name", name }
        };
        if (resources != null && resources.Count > 0)
        {
            requestBody.Add("resources", resources);
        }

        // Сериализуем в JSON
        string jsonBody = JsonConvert.SerializeObject(requestBody);

        // Создаем запрос POST
        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            // Указываем заголовки
            request.SetRequestHeader("Content-Type", "application/json");

            // Записываем тело запроса
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonBody);
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.downloadHandler = new DownloadHandlerBuffer();

            // Отправляем запрос
            yield return request.SendWebRequest();

            // Обрабатываем ответ
            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("API:    Игрок успешно создан: " + request.downloadHandler.text);
            }
            else
            {
                Debug.LogError($"API:    Ошибка создания игрока: {request.responseCode}\n{request.downloadHandler.text}");
            }
        }
    }

    IEnumerator UpdatePlayerResourcesCoro(string username, Dictionary<string, float> updatedResources)
    {
        string url = $"https://2025.nti-gamedev.ru/api/games/{gameUuid}/players/{username}/";

        // Формируем тело запроса
        var requestBody = new
        {
            resources = updatedResources
        };

        // Сериализуем в JSON
        string jsonBody = JsonConvert.SerializeObject(requestBody);

        using (UnityWebRequest request = new UnityWebRequest(url, "PUT"))
        {
            // Устанавливаем заголовки
            request.SetRequestHeader("Content-Type", "application/json");

            // Добавляем тело запроса
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonBody);
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.downloadHandler = new DownloadHandlerBuffer();

            // Отправляем запрос
            yield return request.SendWebRequest();

            // Обрабатываем результат
            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("API:    Ресурсы игрока успешно обновлены: " + request.downloadHandler.text);
            }
            else
            {
                Debug.LogError($"API:    Ошибка обновления ресурсов игрока: {request.responseCode}\n{request.downloadHandler.text}");
            }
        }
    }

    IEnumerator GetPlayerResourcesCoro(string username)
    {
        string url = $"https://2025.nti-gamedev.ru/api/games/{gameUuid}/players/{username}/";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("API:    Ресурсы игрока успешно получены: " + request.downloadHandler.text);

                try
                {
                    // Десериализация ответа
                    var response = JsonConvert.DeserializeObject<PlayerResponse>(request.downloadHandler.text);

                    // Передача значений в MainManager
                    if (response.resources != null)
                    {
                        float gold = response.resources.ContainsKey("gold") ? response.resources["gold"] : 0f;
                        float gpm = response.resources.ContainsKey("gpm") ? response.resources["gpm"] : 0f;

                        mainManager.SetResources(gold, gpm);
                    }
                }
                catch (System.Exception ex)
                {
                    Debug.LogError("API:    Ошибка обработки ответа: " + ex.Message);
                }
            }
            else
            {
                Debug.LogError($"API:    Ошибка получения ресурсов игрока: {request.responseCode}\n{request.downloadHandler.text}");
            }
        }
    }

    IEnumerator SendLogCoro(string comment, string playerName, Dictionary<string, string> resourcesChanged)
    {
        string url = $"https://2025.nti-gamedev.ru/api/games/{gameUuid}/logs/";

        // Формируем тело запроса
        var requestBody = new
        {
            comment = comment,
            player_name = playerName,
            resources_changed = resourcesChanged
        };

        // Сериализуем в JSON
        string jsonBody = JsonConvert.SerializeObject(requestBody);

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            // Устанавливаем заголовки
            request.SetRequestHeader("Content-Type", "application/json");

            // Добавляем тело запроса
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonBody);
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.downloadHandler = new DownloadHandlerBuffer();

            // Отправляем запрос
            yield return request.SendWebRequest();

            // Обрабатываем результат
            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("API:    Лог успешно отправлен: " + request.downloadHandler.text);
            }
            else
            {
                Debug.LogError($"API:    Ошибка отправки лога: {request.responseCode}\n{request.downloadHandler.text}");
            }
        }
    }

    IEnumerator CreateShopCoro(string username, string shopName, Dictionary<string, float> resources)
    {
        string url = $"https://2025.nti-gamedev.ru/api/games/{gameUuid}/players/{username}/shops/";

        // Формируем тело запроса
        var requestBody = new
        {
            name = shopName,
            resources = resources.Count > 0 ? resources : null
        };

        // Сериализуем в JSON
        string jsonBody = JsonConvert.SerializeObject(requestBody);

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            // Устанавливаем заголовки
            request.SetRequestHeader("Content-Type", "application/json");

            // Добавляем тело запроса
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonBody);
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.downloadHandler = new DownloadHandlerBuffer();

            // Отправляем запрос
            yield return request.SendWebRequest();

            // Обрабатываем результат
            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("API:    Магазин успешно создан: " + request.downloadHandler.text);
            }
            else
            {
                Debug.LogError($"API:    Ошибка создания магазина: {request.responseCode}\n{request.downloadHandler.text}");
            }
        }
    }

    private IEnumerator UpdateShopResourcesCoro(string username, string shopName, Dictionary<string, float> updatedResources)
    {
        string url = $"https://2025.nti-gamedev.ru/api/games/{gameUuid}/players/{username}/shops/{shopName}/";

        // Формируем тело запроса
        var requestBody = new
        {
            resources = updatedResources
        };

        // Сериализуем в JSON
        string jsonBody = JsonConvert.SerializeObject(requestBody);

        using (UnityWebRequest request = new UnityWebRequest(url, "PUT"))
        {
            // Устанавливаем заголовки
            request.SetRequestHeader("Content-Type", "application/json");

            // Добавляем тело запроса
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonBody);
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.downloadHandler = new DownloadHandlerBuffer();

            // Отправляем запрос
            yield return request.SendWebRequest();

            // Обрабатываем результат
            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("API:    Ресурсы магазина успешно обновлены: " + request.downloadHandler.text);
            }
            else
            {
                Debug.LogError($"API:    Ошибка обновления ресурсов магазина: {request.responseCode}\n{request.downloadHandler.text}");
            }
        }
    }

    private IEnumerator GetShopResourcesCoro(string username, string shopName)
    {
        string url = $"https://2025.nti-gamedev.ru/api/games/{gameUuid}/players/{username}/shops/{shopName}/";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("API:    Успешное получение ресурсов магазина: " + request.downloadHandler.text);

                // Десериализуем ответ
                var response = JsonConvert.DeserializeObject<ShopResponse>(request.downloadHandler.text);

                if (response != null && response.resources != null && response.resources.Count > 0)
                {
                    // Передаем данные в MainManager
                    mainManager.SetShopResources(response.resources);
                }
            }
            else
            {
                Debug.LogError($"API:    Ошибка получения ресурсов магазина: {request.responseCode}\n{request.downloadHandler.text}");
            }
        }
    }

    private IEnumerator CreateShopLogCoro(string comment, string playerName, string shopName, Dictionary<string, string> resourcesChanged)
    {
        string url = $"https://2025.nti-gamedev.ru/api/games/{gameUuid}/logs/";

        // Формируем тело запроса
        var requestBody = new
        {
            comment = comment,
            player_name = playerName,
            shop_name = shopName,
            resources_changed = resourcesChanged
        };

        // Сериализуем тело запроса в JSON
        string jsonBody = JsonConvert.SerializeObject(requestBody);

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            // Устанавливаем заголовки
            request.SetRequestHeader("Content-Type", "application/json");

            // Добавляем тело запроса
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonBody);
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.downloadHandler = new DownloadHandlerBuffer();

            // Отправляем запрос
            yield return request.SendWebRequest();

            // Обрабатываем ответ
            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("API:    Лог магазина успешно создан: " + request.downloadHandler.text);
            }
            else
            {
                Debug.LogError($"API:    Ошибка создания лога магазина: {request.responseCode}\n{request.downloadHandler.text}");
            }
        }
    }

    // Класс для десериализации ответа
    private class ShopResponse
    {
        public string name { get; set; }
        public Dictionary<string, float> resources { get; set; }
    }

    // Класс для десериализации ответа API
    private class PlayerResponse
    {
        public string name { get; set; }
        public Dictionary<string, float> resources { get; set; }
    }
}

