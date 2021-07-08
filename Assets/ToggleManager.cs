using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ToggleManager : MonoBehaviour
{
    public TextMeshProUGUI buttonText;
    public GameObject adminPage;
    public GameObject userPage;
    public void Toggle()
    {
        userPage.SetActive(adminPage.activeSelf);
        adminPage.SetActive(!adminPage.activeSelf);
        if (userPage.activeSelf)
        {
            buttonText.text = "Admin";
        }
        else
        {
            buttonText.text = "User";
        }
    }
}
