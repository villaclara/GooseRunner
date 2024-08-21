using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeSetup : MonoBehaviour
{
    [SerializeField]
    private Direction _direction;

    // collider for given Edge
    private BoxCollider2D _collider;


	//float unitsPerPixel = 2 * mainCamera.orthographicSize  / Screen.height;

	// Start is called before the first frame update
	void Start()
    {

        _collider = GetComponent<BoxCollider2D>();

        // Get how much pixels are inside 1 unity unit.
		float unitsPerPixel = 2 * Camera.main.orthographicSize / Screen.height; // probably low value. multiplying by 2 on start because _camera.orthographicSize gives half of size of view

        // Set the offset for collider
        _collider.offset = _direction switch
        {
            Direction.Left => new Vector2(-1 * (Screen.width * unitsPerPixel / 2 + _collider.size.x/2f), 0),
            Direction.Right => new Vector3(Screen.width * unitsPerPixel / 2 + _collider.size.x/2f, 0),
            Direction.Up => new Vector2(0, Screen.height * unitsPerPixel),
            Direction.Down => new Vector2(0, -1 * (Screen.height * unitsPerPixel)),
            _ => Vector3.zero
		};

       
        // Set the size for collider
        _collider.size = _direction switch
        {
            Direction.Left or Direction.Right => new Vector2(0.1f, Screen.height * unitsPerPixel),
            Direction.Up or Direction.Down => new Vector2(Screen.height * unitsPerPixel, 0.1f),
            _ => Vector2.zero
        };
    
    }

}
