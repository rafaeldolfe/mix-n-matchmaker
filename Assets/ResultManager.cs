using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    public GameObject container;
    public Image image;
    public TextMeshProUGUI firstPersonText;
    public TextMeshProUGUI secondPersonText;
    public TextMeshProUGUI venueInfoText;
    internal void ShowResult(Person firstPerson, Person secondPerson, VenueName venueName, Sprite venueSprite)
    {
        container.SetActive(true);
        firstPersonText.text = firstPerson.name;
        secondPersonText.text = secondPerson.name;
        venueInfoText.text = $"On {venueName}";
        image.sprite = venueSprite;
    }

    internal void Hide()
    {
        container.SetActive(false);
    }
}
