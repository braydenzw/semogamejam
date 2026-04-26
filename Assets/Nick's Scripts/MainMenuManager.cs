using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public float fadeDuration = 5f;
    public GameObject creditsMenu;
    public void LoadScene(string sceneName)
    {
        StartCoroutine(FadeOutToWhite(sceneName));
    }

    public void ExitGame()
    {
        StartCoroutine(FadeOutToBlack());
    }

    public void TransitionToCredits()
    {
        GetComponent<Animator>().SetBool("FadeCredits", true);
        GetComponent<Animator>().SetBool("FadeToMenu", false);
        creditsMenu.SetActive(true);
    }

    public void TransistionFromCreditsToMainMenu()
    {
        StartCoroutine(FadeToMenu());
    }

    IEnumerator FadeOutToWhite(string sceneName)
    {
        GetComponent<Animator>().SetTrigger("FadeToWhite");
        yield return new WaitForSeconds(fadeDuration);
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator FadeOutToBlack()
    {
        GetComponent<Animator>().SetTrigger("FadeToBlack");
        yield return new WaitForSeconds(fadeDuration);
        Application.Quit();
    }

    IEnumerator FadeToMenu()
    {
        GetComponent<Animator>().SetBool("FadeCredits", false);
        GetComponent<Animator>().SetBool("FadeToMenu", true);
        yield return new WaitForSeconds(3f);
        creditsMenu.SetActive(false);
    }
}
