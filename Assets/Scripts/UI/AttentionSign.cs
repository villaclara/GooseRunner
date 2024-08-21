using System.Collections;
using UnityEngine;

public class AttentionSign : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    private bool _fadein = true;
    
    public IEnumerator ShowSignCoroutine()
    {
        while (GlobalVariables.timesSignShowed < 3)
        {
            if (_fadein)
            {
                while (canvasGroup.alpha < 1)
                {
                    canvasGroup.alpha += Time.deltaTime * 5f;
                    yield return null;
                    yield return new WaitUntil(() => !PauseMenu.isPaused);
                }
                _fadein = false;
            }
            else
            {
                while (canvasGroup.alpha > 0)
                {
                    canvasGroup.alpha -= Time.deltaTime * 5f;
                    yield return null;
                    yield return new WaitUntil(() => !PauseMenu.isPaused);
                }
                _fadein = true;
                GlobalVariables.timesSignShowed++;
            }
        }
        Debug.Log("Show Sign coroutine finished");
    }
}
