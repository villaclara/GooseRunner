using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMovement : MonoBehaviour
{
    private bool moveLeft;
    private void Start()
    {
        if (gameObject.transform.position.x < 0)
            moveLeft = false;
        else
            moveLeft = true;
    }

    void Update()
    {
        if (!PauseMenu.isPaused && GameManager.gameStarted)
        {
            if (moveLeft)
            {
                gameObject.transform.position = new Vector2(gameObject.transform.position.x - 0.001f, gameObject.transform.position.y);
            }
            else
                gameObject.transform.position = new Vector2(gameObject.transform.position.x + 0.001f, gameObject.transform.position.y);

            if (gameObject.transform.position.x > 6f || gameObject.transform.position.x < -6f)
                Destroy(gameObject);
        }

    }

    
}
