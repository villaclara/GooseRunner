using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GemBehaviour : MonoBehaviour
{
    private float _fallSpeed;
    private const string _Tag = "Character";

    [SerializeField]
    private bool _readyToDestroy = false;


    [SerializeField]
    private GameObject _gemTotalIconObject;
    [SerializeField]
    private Vector3 _gemIconPosition = Vector3.zero;

    [SerializeField]
    private GameObject _gemTotalTextObject;

    private bool _gemPicked = false;
    
    [SerializeField]
    private float speed = 25.0f;

    private Vector3 _startLerpPosition;

    private float _time;
    private float _unitsPerPixel;

	private void Start()
	{
        _gemTotalIconObject = GameObject.Find("gemIcon");
        _gemIconPosition = _gemTotalIconObject.transform.position;

        _unitsPerPixel = 2 * Camera.main.orthographicSize / Screen.height;
        _gemTotalTextObject = GameObject.Find("gemsText");
    }

    void Update()
    {
        if (!PauseMenu.isPaused)
        {
            _fallSpeed = GlobalVariables.fallSpeed;
            Vector3 newPosition = transform.position;
            newPosition.y -= _fallSpeed * Time.deltaTime;
            transform.position = newPosition;
            if (transform.position.y <= -Screen.height * _unitsPerPixel / 2 - GlobalVariables.ladderHeight)
            {
                Destroy(gameObject);
            }

            // Move Picked Gem object towards the GemIcon Object at right top corner 
            if (_gemPicked)
            {
				// Set our position as a fraction of the distance between the markers
				transform.position = Vector3.Lerp(_startLerpPosition, _gemIconPosition, _time);
                _time += Time.deltaTime * 4f;
			}

            // After the Gem reaches the position of GemScoreIcon we play Animation and add Score
            if(transform.position == _gemIconPosition)
            {
                Destroy(gameObject);
				GlobalVariables.gems++;
				GameManager.gemsCollected++;
				_gemTotalIconObject.GetComponent<GemScoreController>().DoScoreUpdAnimation();
                _gemTotalTextObject.GetComponent<GemsTotalTextController>().DoScoreUpdAnimation();
            }
        }
    }
        

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag(_Tag))
		{
            // Get the current Gem position - Start point of Lerp
            _startLerpPosition = transform.position;
            _gemPicked = true;
		}

        
	}

}
