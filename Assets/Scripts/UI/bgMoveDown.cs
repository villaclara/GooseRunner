using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class bgMoveDown : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.gameStarted && !PauseMenu.isPaused && GlobalVariables.gameIsRunning)
        {
            Vector2 pos = gameObject.transform.position;
            pos.y -= 0.0002f;
            gameObject.transform.position = pos;
        }
    }
}
