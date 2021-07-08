using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridLayoutCellSizeScript : MonoBehaviour
{
    public int cellWidthsPerScreen;
    public int cellHeightsPerScreen;

    public GridLayoutGroup gridLayout;

    private void Update()
    {
        int screenWidth = Screen.width;
        int screenHeight = Screen.height;

        gridLayout.cellSize = new Vector3(screenWidth / cellWidthsPerScreen, screenHeight / cellHeightsPerScreen);
    }
}
