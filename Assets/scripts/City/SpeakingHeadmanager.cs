using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpeakingHeadmanager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI speakingHead_text;
    [SerializeField] private Image speakingHead_Image;

    public void SetNewText()
    {

    }

    public void SetNewImage()
    {

    }

    public void Open()
    {
        this.gameObject.SetActive(true);
    }

    private void Close()
    {
        gameObject.SetActive(false);
    }
}
