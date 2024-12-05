using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private MainManager mainManager;
    [SerializeField] private MineSweeper mineSweeper;

    public void Show()
    {
        if(mainManager != null) 
            mainManager.OffCells();

        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        if (mainManager != null)
            mainManager.OnCells();
        this.gameObject.SetActive(false);
    }

    public void PlayTheGame()
    {
        if (mineSweeper != null)
            mineSweeper.StartGame();
        this.gameObject.SetActive(false);
    }

    public void Exit()
    {
        SceneManager.LoadScene("Main");
    }
}
