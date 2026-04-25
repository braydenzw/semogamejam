using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public float fadeDuration = 5f;
    public void LoadScene(string sceneName)
    {
        StartCoroutine(FadeOut(sceneName));
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    IEnumerator FadeOut(string sceneName)
    {
        GetComponent<Animator>().SetTrigger("FadeToWhite");
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(sceneName);
    }
}
