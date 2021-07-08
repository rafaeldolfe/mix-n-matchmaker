using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AreYouSureManager : MonoBehaviour
{
    public GameObject container;
    public TextMeshProUGUI careText;
    private VenueManager currentManager;
    internal void Show(VenueManager manager)
    {
        container.SetActive(true);
        currentManager = manager;
        careText.text = $"Are you sure you wish to match two people for {manager.venue.name}?";
    }
    public void Confirm()
    {
        container.SetActive(false);
        currentManager.Confirm();
    }
    public void Cancel()
    {
        container.SetActive(false);
    }
}
