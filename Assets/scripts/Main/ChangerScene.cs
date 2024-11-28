using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangerScene : MonoBehaviour
{
    public void LoadCity()
    {
        SceneManager.LoadScene("City");
    }

    public void LoadApiary()
    {
        SceneManager.LoadScene("Apiary");
    }
}
