using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadingStatusScript : MonoBehaviour
{
    public CanvasGroup group;
    public TextMeshProUGUI statusMessageArea;

    private float fadeOutTime = 2;
    private void Awake()
    {
        group.alpha = 1;
        StartCoroutine(FadeOutAndDestroy());
    }

    public void SetMessage(string message)
    {
        statusMessageArea.text = message;
    }

    public void SetFadeTime(float fadeTime)
    {
        fadeOutTime = fadeTime;
    }

    private IEnumerator FadeOutAndDestroy()
    {
        while(group.alpha > 0)
        {
            group.alpha -= Time.deltaTime * (1 / fadeOutTime);
            yield return null;
        }
        Destroy(gameObject);
    }
}
