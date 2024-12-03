using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameTitle : MonoBehaviour
{
    public bool hasMine;
    public bool isRevealed;
    public TextMeshProUGUI cellText;
    public Button button;

    public void Initialize()
    {
        hasMine = false;
        isRevealed = false;
        cellText.text = "";
        cellText.gameObject.SetActive(false);
        button.interactable = true;
    }

    public void Reveal()
    {
        isRevealed = true;
        button.interactable = false;

        if (hasMine)
        {
            cellText.text = "💣";
            cellText.color = Color.red;
            cellText.gameObject.SetActive(true);
        }
    }

    public void SetMineCount(int mineCount)
    {
        if (mineCount > 0)
        {
            cellText.text = mineCount.ToString();
            cellText.color = Color.black;
            cellText.gameObject.SetActive(true);
        }
        else
        {
            cellText.gameObject.SetActive(false);
        }
        button.interactable = false;
    }
}
