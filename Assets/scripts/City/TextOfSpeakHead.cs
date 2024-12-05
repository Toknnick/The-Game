using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextOfSpeakHead : MonoBehaviour
{
    [SerializeField] private List<string> texts;
    [SerializeField] private Sprite head;
    [SerializeField] private List<GameObject> panel;

    public void SetText(SpeakingHeadmanager speakingHeadmanager)
    {
        speakingHeadmanager.gameObject.SetActive(true);
        speakingHeadmanager.Open(texts, head, panel);
    }
}
