using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PriorityScript : MonoBehaviour
{
    public TextMeshProUGUI amount;

    internal void SetNumber(int count)
    {
        amount.text = count.ToString();
    }
}
