using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullscreenToggleScript : MonoBehaviour
{
    List<(int, int, bool)> listOfModes = new List<(int, int, bool)> { (640, 480, false), (800, 600, false), (1280, 720, false), (1920, 1080, false), (1920, 1080, true) };
    int currentIndex = 2;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Screen.SetResolution(listOfModes[currentIndex].Item1, listOfModes[currentIndex].Item2, listOfModes[currentIndex].Item3);
            currentIndex++;
            if (currentIndex > listOfModes.Count - 1)
            {
                currentIndex = 0;
            }
        }
    }
}
