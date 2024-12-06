using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameTitle : MonoBehaviour, IPointerClickHandler
{
    public bool hasMine;
    public bool isRevealed;
    public TextMeshProUGUI cellText;
    public Button button;
    public Image bombImage; // Поле для изображения бомбы
    public Image dangerImage;  // Поле для изображения Danger

    private bool isMarkedAsDanger; // Флаг для Danger

    public void Initialize()
    {
        hasMine = false;
        isRevealed = false;
        cellText.text = "";
        cellText.gameObject.SetActive(false);
        button.interactable = true;
        isMarkedAsDanger = false;

        if (bombImage != null)
        {
            bombImage.gameObject.SetActive(false); // Скрываем изображение бомбы
        }

        if (dangerImage != null)
        {
            dangerImage.gameObject.SetActive(false); // Скрываем изображение Danger
        }
    }

    public void Reveal()
    {
        isRevealed = true;
        button.interactable = false;

        if (hasMine && bombImage != null)
        {
            bombImage.gameObject.SetActive(true);
        }
        else
        {
            dangerImage.gameObject.SetActive(false);
        }
    }

    public void HideImage()
    {
        dangerImage.gameObject.SetActive(false); // Скрываем изображение Danger
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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right && !isRevealed)
        {
            ToggleDangerMark();
        }
    }

    private void ToggleDangerMark()
    {
        if (dangerImage == null) return;

        isMarkedAsDanger = !isMarkedAsDanger;

        if (isMarkedAsDanger)
        {
            dangerImage.gameObject.SetActive(true);
        }
        else
        {
            dangerImage.gameObject.SetActive(false);
        }
    }
}
