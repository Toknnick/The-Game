using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LostPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Button exitButton;
    public void EndGame(bool isWin, float gold)
    {
        this.gameObject.SetActive(true);

        if (isWin)
        {
            text.text = $"Ты помог найти всех пчел! Ты получаешь в дар:  {Mathf.FloorToInt(gold).ToString()} меда";
        }
        else
        {
            text.text = "Ты не помог найти всех пчел..";
        }
    }

    public void Hide()
    {
        text.text = "";
        this.gameObject.SetActive(false);
    }
}
