using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpeakingHeadmanager : MonoBehaviour
{
    public List<string> texts;

    [SerializeField] private TextMeshProUGUI speakingHead_text;
    [SerializeField] private Image speakingHead_Image; 

    public float typingSpeed = 0.01f; // Скорость печати (в секундах за символ)
    private Coroutine typingCoroutine;
    private List<GameObject> panel;

    private int nowText = 0;

    public void SetNewText()
    {
        nowText++;

        if (nowText < texts.Count)
        {
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine); // Останавливаем предыдущую корутину, если она активна
            }
            typingCoroutine = StartCoroutine(TypeText(texts[nowText]));
        }
        else
        {
            if (panel != null)
            {
                foreach (GameObject go in panel)
                {
                    go.SetActive(true);
                }
            }

            gameObject.SetActive(false);
        }
    }

    private IEnumerator TypeText(string fullText)
    {
        speakingHead_text.text = ""; // Очищаем текст перед началом
        foreach (char letter in fullText)
        {
            speakingHead_text.text += letter; // Добавляем по одному символу
            yield return new WaitForSeconds(typingSpeed); // Задержка
        }
        typingCoroutine = null; // Обнуляем ссылку после завершения
    }

    public void Open(List<string> textss, Sprite head,List<GameObject> panel)
    {
        this.panel = panel;
        speakingHead_Image.sprite = head;
        nowText = 0;
        texts = textss;
        speakingHead_text.text = texts[nowText];
        this.gameObject.SetActive(true);
    }

    private void Close()
    {
        gameObject.SetActive(false);
    }
}
