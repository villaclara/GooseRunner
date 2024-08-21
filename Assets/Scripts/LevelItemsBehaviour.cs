using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelItemsBehaviour : MonoBehaviour
{

    private float _fallSpeed = GlobalVariables.fallSpeed;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.isPaused)
        {
            _fallSpeed = GlobalVariables.fallSpeed;
            Vector2 newPosition = transform.position;
            newPosition.y -= _fallSpeed * Time.deltaTime;
            transform.position = newPosition;
            if (transform.position.y <= -5.5f)
            {
                gameObject.SetActive(false);
                if (gameObject.CompareTag("blockBehindLadder"))
                    gameObject.GetComponent<Collider2D>().isTrigger = true;

            }
        }
    }
}
