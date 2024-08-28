using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;


public class MainCharacterMovement : MonoBehaviour
{


    [SerializeField]
	public  static float moveSpeed = 1.5f;


	[SerializeField]
	private bool isControlTapSystem = true;
    private float _fallSpeed = GlobalVariables.fallSpeed;
	private float _unitsPerPixel;

	// RigidBody component of MainCharacter GameObject responsible for physics.
	public Rigidbody2D _body;
	private BoxCollider2D _bodyCollider2D;

	public static bool isDead;
	public GameManager gameManager;


	// 1 -> facing right, -1 -> facing left.
	private Direction _playerdirection = Direction.None;
	private Direction _previousDirectionX = Direction.Right;


	float unitsPerPixel;

    #region Touch Swipe Detection

    private Vector2 _startTouchPos = Vector2.zero;
	private Vector2 _endTouchPos = Vector2.zero;
	private readonly float MIN_SWIPE_DISTANCE = 100f; // might be changed, random value from internet - 0.17f

	private bool _startTouchChanged = false;
	private bool _endTouchChanged = false;

	#endregion


	#region Collision related variables

	//private bool _characterCrossedThroughEdge = false;
	// Shows if the character has collided with ladder and now is inside it.
	private bool _characterInsideLadder = false;

	#endregion


	// Start is called before the first frame update
	private void Start()
	{
		float _screenWidthInUnits = Camera.main.orthographicSize * 2 * Screen.width / Screen.height;
        moveSpeed = 1.5f *  _screenWidthInUnits/GlobalVariables.commonScreenWidthInUnits;
		Debug.Log($"move speed is {moveSpeed}");
		// Get the RigidBody2D component for the Character GameObject.
		_body = GetComponent<Rigidbody2D>();
		_body.freezeRotation = true;
		isDead = false;
		unitsPerPixel = 2 * Camera.main.orthographicSize / Screen.height; // probably low value. multiplying by 2 on start because _camera.orthographicSize gives half of size of view
	
	}

	// Update is called once per frame
	private void Update()
	{
				if (isControlTapSystem)
				{
					CheckTapInputSystem();
				}
				else
				{
					CheckSwipeInputSystem();
				}
				
		if(!PauseMenu.isPaused && GameManager.gameStarted)
		{
			_fallSpeed = GlobalVariables.fallSpeed;
			Vector2 newPosition = transform.position;
			newPosition.y -= _fallSpeed * Time.deltaTime;
			transform.position = newPosition;
			//game over 
			if (gameObject.transform.position.y < -6f && !isDead)
			{
				isDead = true;
				gameManager.GameOver();
				GlobalVariables.gameIsRunning = false;
			}

			if (!isDead)
			{
				CheckKeyboardInputSystem();



				//Debug.Log($"position ({_body.position})");
				//Debug.Log($"Screen width({Screen.width})");
                // screen edge check
                if (_body.position.x <= (-1 * Screen.width * unitsPerPixel / 2 - GetComponent<BoxCollider2D>().size.x/2))
				{
					_body.transform.position = new Vector2(Screen.width * unitsPerPixel / 2, _body.position.y);
				}
				else if (_body.position.x >= (Screen.width * unitsPerPixel / 2 + GetComponent<BoxCollider2D>().size.x/2))
				{
					_body.transform.position = new Vector2(-1 * Screen.width * unitsPerPixel / 2, _body.position.y);
				}

				/// TOUCH INPUT 
				MoveCharacter(_playerdirection);

			}
        }
    }


    private void MoveCharacter(Direction direction)
	{
			if (direction == Direction.Left || direction == Direction.Right)
			{
				// Face proper side and move character via its component.
				_body.transform.localScale = new Vector3(Mathf.Abs(_body.transform.localScale.x) * (int)direction, _body.transform.localScale.y, _body.transform.localScale.z); // Mathf.Abs to get positive value
				_body.velocity = new Vector2(moveSpeed * (int)direction, _body.velocity.y);
				return;
			}

		if (direction == Direction.Up || direction == Direction.Down)
		{
			var currentPos = _body.transform.position;

			currentPos.y = direction == Direction.Up ? currentPos.y + (_fallSpeed + 5f) * Time.deltaTime : currentPos.y - 5f * Time.deltaTime;
			_body.transform.position = currentPos;
			return;
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		// Reverses direction of character when got collision with vertical wall.
		if (collision.gameObject.CompareTag("VerticalWall"))
		{
			_playerdirection = _playerdirection == Direction.Left ? Direction.Right : Direction.Left;
			_previousDirectionX = _playerdirection;
			_body.transform.localScale = new Vector3(Mathf.Abs(_body.transform.localScale.x) * (int)_playerdirection, _body.transform.localScale.y, _body.transform.localScale.z); // Mathf.Abs to get positive value
			//Debug.Log($"Change character direction to ({_playerdirection}).");
		}
    }



	private void OnTriggerEnter2D(Collider2D other)
	{
		
		// Ladder Collision to be able to move upside.
		if(other.gameObject.CompareTag("Ladder"))
		{
			//Debug.Log($"Character inside ladder({_characterInsideLadder}).");
			_characterInsideLadder = true;
		}

	}


	private void OnTriggerExit2D(Collider2D other)
	{

		// If character is inside Ladder then we allow him to go UP. Also saving previous direction.
		if (other.gameObject.CompareTag("Ladder"))
		{

			//Debug.Log($"Character inside ladder({_characterInsideLadder}).");
			_characterInsideLadder = false;
			_playerdirection = _previousDirectionX;


			// After character moves above the Ladder we set Ladder to have default collider (not trigger).
			WallBehindLadderScript wallBScript = other.GetComponent<WallBehindLadderScript>();
			var otherCollider = other.GetComponent<BoxCollider2D>();
			if (otherCollider != null && otherCollider.gameObject.transform.position.y <= _body.transform.position.y)
			{
				wallBScript.wall.GetComponent<BoxCollider2D>().isTrigger = false;

				GlobalVariables.score++;
			}

			GameObject otherWallBehind = wallBScript.otherWall;
			if (otherWallBehind != null && otherCollider.gameObject.transform.position.y <= _body.transform.position.y)
			{
				otherWallBehind.GetComponent<BoxCollider2D>().isTrigger = false;
            }


        }
	}


	private void CheckKeyboardInputSystem()
	{
		// Change direction of the movement
		if (Input.GetKey(KeyCode.A))
		{
			_playerdirection = Direction.Left;
			_previousDirectionX = Direction.Left;
			//Debug.Log($"A pressed. Direction ({_playerdirection}). localscale({transform.localScale}).");
		}

		if (Input.GetKey(KeyCode.D))
		{
			_playerdirection = Direction.Right;
			_previousDirectionX = Direction.Right;
			//Debug.Log($"D pressed. Direction ({_playerdirection}). localscale({transform.localScale}).");
		}

		if (Input.GetKey(KeyCode.W))
		{
			if (_characterInsideLadder)
			{
				_playerdirection = Direction.Up;
			}

		}
	}

	/// <summary>
	/// Tap Input Sytem.
	/// </summary>
	private void CheckTapInputSystem()
	{
		if (Input.touchCount > 0)
		{
			if (Input.GetTouch(0).phase == TouchPhase.Began)
			{
				// do up if inside ladder
				if (_characterInsideLadder)
				{
					_playerdirection = Direction.Up;
					//Debug.Log($"Inside ladder. Direction({_playerdirection}), previous({_previousDirectionX}).");
				}
				// else do left/right
				else
				{
					//Debug.Log($"previousdirection({_previousDirectionX}.");
					_playerdirection = _playerdirection == Direction.Left ? Direction.Right : Direction.Left;
					_previousDirectionX = _playerdirection;
					//Debug.Log($"Direction({_playerdirection}), previous({_previousDirectionX}).");
				}
			}
		}
	}

	/// <summary>
	/// Swipe Input System.
	/// </summary>
	private void CheckSwipeInputSystem()
	{

		// Change direction of the movement
		if (Input.GetKey(KeyCode.A)) 
		{
			_playerdirection = Direction.Left;
			_previousDirectionX = Direction.Left;
			Debug.Log($"A pressed. Direction ({_playerdirection}). localscale({transform.localScale}).");
		}

		if (Input.GetKey(KeyCode.D))
		{
			_playerdirection = Direction.Right;
			_previousDirectionX = Direction.Right;
			Debug.Log($"D pressed. Direction ({_playerdirection}). localscale({transform.localScale}).");
		}	

		if(Input.GetKey(KeyCode.W))
		{
			if(_characterInsideLadder)
			{
				_playerdirection = Direction.Up;
			}

		}


		//// TOUCH INPUT
		


		// Change direction of the movement
		if (Input.GetKey(KeyCode.A)) 
		{
			_playerdirection = Direction.Left;
			_previousDirectionX = Direction.Left;
			//Debug.Log($"A pressed. Direction ({_playerdirection}). localscale({transform.localScale}).");
		}

		if (Input.GetKey(KeyCode.D))
		{
			_playerdirection = Direction.Right;
			_previousDirectionX = Direction.Right;
			//Debug.Log($"D pressed. Direction ({_playerdirection}). localscale({transform.localScale}).");
		}	

		if(Input.GetKey(KeyCode.W))
		{
			if(_characterInsideLadder)
			{
				_playerdirection = Direction.Up;
	
			}

		}


		//// TOUCH INPUT
		


		// Change direction of the movement
		if (Input.GetKey(KeyCode.A)) 
		{
			_playerdirection = Direction.Left;
			_previousDirectionX = Direction.Left;
			Debug.Log($"A pressed. Direction ({_playerdirection}). localscale({transform.localScale}).");
		}

		if (Input.GetKey(KeyCode.D))
		{
			_playerdirection = Direction.Right;
			_previousDirectionX = Direction.Right;
			Debug.Log($"D pressed. Direction ({_playerdirection}). localscale({transform.localScale}).");
		}	

		if(Input.GetKey(KeyCode.W))
		{
			if(_characterInsideLadder)
			{
				_playerdirection = Direction.Up;
			}

		}


		//// TOUCH INPUT
		

		// Reset positions variables if no input received at current Frame.
		if (Input.touchCount == 0)
		{
			_startTouchPos = Vector2.zero;
			_endTouchPos = Vector2.zero;
			_startTouchChanged = false;
			_endTouchChanged = false;
		}

		// Assign variables according to the state of touch.
		foreach (Touch touch in Input.touches)
		{

			if (touch.phase == TouchPhase.Began)
			{
				_startTouchPos = touch.position;
				//Debug.Log($"startTouchPos - {touch.position}");

				_startTouchChanged = true;
			}
			if (touch.phase == TouchPhase.Ended)
			{
				_endTouchPos = touch.position;
				//Debug.Log($"endTouchPos - {touch.position}");

				_endTouchChanged = true;
			}
		}

		// Axis Movement
		// Both startTouchPosition and EndTouchPosition has been assigned with new values.
		if (_startTouchChanged && _endTouchChanged)
		{
			// Move Right
			if (_startTouchPos.x < _endTouchPos.x && (_endTouchPos.x - _startTouchPos.x > MIN_SWIPE_DISTANCE))
			{
				_playerdirection = Direction.Right;
				//Debug.Log($"XstartTouch < XendTouch. Direction ({_playerdirection}). localscale({transform.localScale}).");
				_previousDirectionX = _playerdirection;

			}

			// Move Left
			else if (_startTouchPos.x > _endTouchPos.x && (_startTouchPos.x - _endTouchPos.x > MIN_SWIPE_DISTANCE))
			{
				_playerdirection = Direction.Left;
				//Debug.Log($"XstartTouch > XendTouch. Direction ({_playerdirection}). localscale({transform.localScale}).");
				_previousDirectionX = _playerdirection;

			}


			// If the next value is false - we do not allow performing MoveUp movement.
			if (!_characterInsideLadder)
			{
				//Debug.Log($"character not in the Ladder. return");
			}

			// Move Up
			else if (_startTouchPos.y < _endTouchPos.y && (_endTouchPos.y - _startTouchPos.y > MIN_SWIPE_DISTANCE))
			{
				_playerdirection = Direction.Up;
				//Debug.Log($"YstartTouch < YendTouch. Direction ({_playerdirection}). localscale({transform.localScale}).");

			}

			// Move Down
			//else if (_startTouchPos.y > _endTouchPos.y && (_endTouchPos.y - _startTouchPos.y > MIN_SWIPE_DISTANCE))
			//{
			//	_playerdirection = Direction.Down;
			//	Debug.Log($"YstartTouch > YendTouch. Direction ({_playerdirection}). localscale({transform.localScale}).");

			//}



			// Reset startTouchPos and endTouchPos -> need to assign both of them again to move.
			_startTouchChanged = false;
			_endTouchChanged = false;

		}
	}

}
