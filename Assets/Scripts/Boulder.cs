using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.IK;

public class Boulder : MonoBehaviour
{
    private float _fallSpeed = 15f;
    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.isPaused)
        {
            Vector2 newPosition = transform.position;
            newPosition.y -= _fallSpeed * Time.deltaTime;
            transform.position = newPosition;
            if (gameObject.transform.position.y < -6)
                Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Character")
        {
            GlobalVariables.gameIsRunning = false;
            MainCharacterMovement.isDead = true;
        }

    }
}
