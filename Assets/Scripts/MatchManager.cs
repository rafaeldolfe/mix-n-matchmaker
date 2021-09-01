using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MatchManager : MonoBehaviour
{
    public Match match;
    public Image venueSprite;
    public TextMeshProUGUI person1Text;
    public TextMeshProUGUI person2Text;
    public TextMeshProUGUI venueNameText;

    internal void SetMatch(Match match)
    {
        this.match = match;
        venueSprite.sprite = match.venueSprite;
        person1Text.text = $"{match.person1.name}";
        person2Text.text = $"{match.person2.name}";
        venueNameText.text = $"On {match.venueName}";
    }
    public override string ToString()
    {
        return $"{match.person1} vs {match.person2} on {match.venueName}";
    }
}
