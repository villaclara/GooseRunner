using System.Collections;
using System.Collections.Generic;
using Unity.Hierarchy;
using UnityEngine;

public class LevelEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    private float _fallSpeed = GlobalVariables.fallSpeed;
    private float _unitsPerPixel;
    private float _moveSpeed = 0.7f;
    private float _additionalFallSpeed = 0.3f;
    private bool gameOver;
    // Start is called before the first frame update
    void Start()
    {
        gameOver = false;
        if (transform.position.x > 0)
            _moveSpeed = -_moveSpeed;

        _unitsPerPixel = 2 * Camera.main.orthographicSize / Screen.height;
        InvokeRepeating("ChangeSign", 0.5f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.isPaused && GameManager.gameStarted && gameOver)
        {
            _fallSpeed = GlobalVariables.fallSpeed + _additionalFallSpeed;
            Vector2 newPosition = transform.position;
            newPosition.y -= _fallSpeed * Time.deltaTime;
            newPosition.x += _moveSpeed * Time.deltaTime;
            transform.position = newPosition;
            if (transform.position.y <= -Screen.height * _unitsPerPixel / 2 - GlobalVariables.ladderHeight)
            {
                gameObject.SetActive(false);

            }

            if (Mathf.Abs(transform.position.x) >= Screen.width * _unitsPerPixel / 2)
                _moveSpeed = -_moveSpeed;
        }
    }

    private void ChangeSign()
    {
        StartCoroutine(ChangeSignCoroutine());
    }

    public IEnumerator ChangeSignCoroutine()
    {
        _additionalFallSpeed = -_additionalFallSpeed;
        yield return null;
        yield return new WaitUntil(() => !PauseMenu.isPaused);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Character")
        {
            GlobalVariables.gameIsRunning = false;
            MainCharacterMovement.isDead = true;
            gameOver = true;
        }
    }
}
