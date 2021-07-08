using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullscreenScript : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(FullscreenSetup());
    }
    private IEnumerator FullscreenSetup()
    {
        yield return new WaitForSeconds(0.5f);
        Screen.SetResolution(800, 600, false);
    }
}
