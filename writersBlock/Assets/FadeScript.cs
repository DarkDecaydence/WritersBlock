using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeScript : MonoBehaviour {

    CanvasGroup canvasGroup;
    bool isReady = false;

    public float fadeTime = 2f;

	// Use this for initialization
	void Awake () {

        canvasGroup = gameObject.GetComponent<CanvasGroup>();
    }

    public void startFade()
    {
        StartCoroutine(Fade());
    }

    public void startFadeIn()
    {
        StartCoroutine(FadeIn());
    }

    public void startFadeOut()
    {
        StartCoroutine(FadeOut());
    }

    private IEnumerator Fade()
    {
        float timer = 0;
        while(timer < fadeTime)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = timer / fadeTime;
            yield return null;
        }

        canvasGroup.alpha = 1;
        yield return new WaitForSeconds(1f);

        timer = 0;
        while (timer < fadeTime)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = 1 - timer / fadeTime;
            yield return null;
        }
    }

    private IEnumerator FadeIn()
    {
        float timer = 0;
        while (timer < fadeTime)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = timer / fadeTime;
            yield return null;
        }

        isReady = true;
        canvasGroup.alpha = 1;
    }

    private IEnumerator FadeOut()
    {
        while (!isReady)
            yield return new WaitForSeconds(0.1f);

        float timer = 0;
        while (timer < fadeTime)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = 1 - timer / fadeTime;
            yield return null;
        }

        isReady = false;
    }

}
