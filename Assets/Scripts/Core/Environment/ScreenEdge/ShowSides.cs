using System.Collections;
using System;
using UnityEngine;

public class ShowSides : MonoBehaviour
{
    public CanvasGroup canvGroupSpeedUp;
    public CanvasGroup canvGroupSlowDown;

    private void Awake()
    {
        OrbsBehaviour.OnSpeedUpOrbPickUp += ShowSideSpeedUp;
        OrbsBehaviour.OnSlowDownOrbPickUp += ShowSideSlowDown;
    }
    public void ShowSideSpeedUp()
    {
        StartCoroutine("showSidesCoroutine", canvGroupSpeedUp);
    }

    public void ShowSideSlowDown()
    {
        StartCoroutine("showSidesCoroutine", canvGroupSlowDown);
    }
    public IEnumerator showSidesCoroutine(CanvasGroup canvGroup)
    {
        while (canvGroup.alpha < 1)
        {
            canvGroup.alpha += Time.deltaTime * 5f;
            yield return null;
            yield return new WaitUntil(() => !PauseMenu.isPaused);
        }
        yield return new WaitForSeconds(1);
        while (canvGroup.alpha > 0)
        {
            canvGroup.alpha -= Time.deltaTime * 5f;
            yield return null;
            yield return new WaitUntil(() => !PauseMenu.isPaused);
        }
    }
}
