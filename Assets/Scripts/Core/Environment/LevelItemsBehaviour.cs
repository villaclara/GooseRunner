using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class levelItemsBehaviour : MonoBehaviour
{

    private float _fallSpeed = GlobalVariables.fallSpeed;
    private float _unitsPerPixel;
    private float ladderHieght;
    // Start is called before the first frame update
    void Start()
    {
        _unitsPerPixel = 2 * Camera.main.orthographicSize / Screen.height;
    }

    // Update is called once per frame
    void Update()
    {
        ladderHieght = GlobalVariables.ladderHeight;
        if (!PauseMenu.isPaused)
        {
            _fallSpeed = GlobalVariables.fallSpeed;
            Vector2 newPosition = transform.position;
            newPosition.y -= _fallSpeed * Time.deltaTime;
            transform.position = newPosition;
            if (transform.position.y <= -Screen.height * _unitsPerPixel/2 - GlobalVariables.ladderHeight)
            {
                gameObject.SetActive(false);
                if (gameObject.CompareTag("blockBehindLadder"))
                    gameObject.GetComponent<Collider2D>().isTrigger = true;

            }
        }
    }

	//private void OnCollisionEnter2D(Collision2D collision)
	//{
	//	Debug.Log("ONCOLLISION ENTER EMPTY");

	//	if (this.tag == "wall")
 //       {
 //           if(CompareTag("Character"))
 //           {
 //               Debug.Log("ONCOLLISION ENTER CHARACTER");
 //               collision.transform.SetParent(transform);
 //           }
 //       }
	//}


	//private void OnCollisionExit2D(Collision2D collision)
	//{
 //       collision.transform.SetParent(null);
	//}
}
