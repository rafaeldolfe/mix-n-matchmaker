using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ToggleManager : MonoBehaviour
{
    public TextMeshProUGUI buttonText;
    public GameObject page1;
    public GameObject page2;
    public string buttonText1;
    public string buttonText2;
    public void Toggle()
    {
        page2.SetActive(page1.activeSelf);
        page1.SetActive(!page1.activeSelf);
        if (page2.activeSelf)
        {
            buttonText.text = buttonText1;
        }
        else
        {
            buttonText.text = buttonText2;
        }
    }
}
