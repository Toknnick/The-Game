using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextOfSpeakHead : MonoBehaviour
{
    [SerializeField] private List<string> texts;
    [SerializeField] private Sprite head;

    public void SetText(SpeakingHeadmanager speakingHeadmanager)
    {
        speakingHeadmanager.Open(texts, head);
    }
}
