using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void LoadScene(string scenName)
    {
        SceneManager.LoadScene(scenName);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
