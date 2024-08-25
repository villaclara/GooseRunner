using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
    public static float fallSpeed = 0.4f;//if want to change look at the gameManager start method
    public static int score = 0;
    public static int gems;
    public static int timesSignShowed = 3;

    public static bool gameIsRunning = true;
    public static float commonScreenWidthInUnits = 4.62f;
    public static float ladderHeight;
}
