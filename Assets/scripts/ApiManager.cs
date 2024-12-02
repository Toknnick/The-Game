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
        StartCoroutine(GetPlayerResourcesCoro(balancer.userName));
    }

    public void CreatePlayer(string name, Dictionary<string, float> resources = null)
    {
        StartCoroutine(CreatePlayerCoro(name, resources));
    }

    public void UpdatePlayer(string name, Dictionary<string, float> resources)
    {
        StartCoroutine(UpdatePlayerResourcesCoro(name, resources));
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
                Debug.Log("API: Ресурсы игрока успешно получены: " + request.downloadHandler.text);

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

    // Класс для десериализации ответа API
    private class PlayerResponse
    {
        public string name { get; set; }
        public Dictionary<string, float> resources { get; set; }
    }
}

