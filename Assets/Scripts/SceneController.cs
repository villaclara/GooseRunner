using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public void LoadLockerScene()
    {
        StartCoroutine(FadeInScreen());
        SceneManager.LoadScene(1);
    }

    public void LoadMainScene()
    {
        StartCoroutine(FadeInScreen());
        SceneManager.LoadScene(0);
    }

    public IEnumerator FadeInScreen()
    {

        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime * 4f;
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
    }


    public IEnumerator FadeOutScreen()
    {
        yield return new WaitForSeconds(0.5f);

        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime * 4f;
            yield return null;
        }
    }
}
