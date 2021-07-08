using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PersonListEntryScript : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public Person person;
    public Image image;
    public Color highlightColor;
    public Color pickedColor;

    private Color originalColor;
    private Color originalTextColor;
    private void Awake()
    {
        originalColor = image.color;
        originalTextColor = nameText.color;
    }
    public void SetName(string name)
    {
        nameText.text = name;
    }
    internal void SetPerson(Person person)
    {
        this.person = person;
        nameText.text = person.name;
    }
    internal void HighlightAsCandidate()
    {
        image.color = highlightColor;
        nameText.color = highlightColor;
    }
    internal void ChooseCandidate()
    {
        image.color = pickedColor;
        nameText.color = pickedColor;
    }
    internal void Unhighlight()
    {
        image.color = originalColor;
        nameText.color = originalTextColor;
    }
}
