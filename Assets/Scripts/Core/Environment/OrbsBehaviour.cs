using System;
using UnityEditor;
using UnityEngine;

public class OrbsBehaviour : MonoBehaviour
{
    private float _fallSpeed;
    private const string _Tag = "Character";
    private const string _speedUpTag = "SpeedUpOrb";
    private const string _slowDownTag = "SlowDownOrb";

    public static event Action OnSpeedUpOrbPickUp;
    public static event Action OnSlowDownOrbPickUp;
    ShowSides showSides;
    AudioManager audioManager;
    Transform characterTransform;

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        showSides = GameObject.FindGameObjectWithTag("slowDownSides").GetComponent<ShowSides>();
        characterTransform = GameObject.FindGameObjectWithTag("Character").GetComponent<Transform>();
        OnSpeedUpOrbPickUp += showSides.ShowSideSpeedUp;
        OnSlowDownOrbPickUp += showSides.ShowSideSlowDown;
    }


    void Update()
    {
        if (!PauseMenu.isPaused)
        {
            _fallSpeed = GlobalVariables.fallSpeed;
            Vector3 newPosition = transform.position;
            newPosition.y -= _fallSpeed * Time.deltaTime;
            transform.position = newPosition;
            if (transform.position.y <= -5.5)
            {
                Destroy(gameObject);
            }
        }
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(_Tag) && gameObject.CompareTag(_speedUpTag))
        {
            audioManager.PlaySFX(audioManager.speedUpOrbPickUp);
            Debug.Log($"Speed was {GlobalVariables.fallSpeed}, characters: {MainCharacterMovement.moveSpeed}");
            OnSpeedUpOrbPickUp?.Invoke();
            OnSpeedUpOrbPickUp -= showSides.ShowSideSpeedUp;
            Destroy(gameObject);
            GlobalVariables.fallSpeed += 0.10f;
            MainCharacterMovement.moveSpeed += 0.02f + Screen.width * 0.0001f;
            Debug.Log($"Speed now {GlobalVariables.fallSpeed}, characters: {MainCharacterMovement.moveSpeed}");
        }

        if (other.gameObject.CompareTag(_Tag) && gameObject.CompareTag(_slowDownTag))
        {
            audioManager.PlaySFX(audioManager.slowDownOrbPickUp);
            Debug.Log($"Speed was {GlobalVariables.fallSpeed}, characters: {MainCharacterMovement.moveSpeed}");
            OnSlowDownOrbPickUp?.Invoke();
            OnSlowDownOrbPickUp -= showSides.ShowSideSlowDown;
            Destroy(gameObject);
            
            GlobalVariables.fallSpeed -= 0.16f;
            MainCharacterMovement.moveSpeed -= 0.04f + Screen.width * 0.0001f;
            
            Debug.Log($"Speed now {GlobalVariables.fallSpeed}, characters: {MainCharacterMovement.moveSpeed}");
        }
    }
    private void OnDestroy()
    {
        OnSpeedUpOrbPickUp -= showSides.ShowSideSpeedUp;
        OnSlowDownOrbPickUp -= showSides.ShowSideSlowDown;
    }

}
