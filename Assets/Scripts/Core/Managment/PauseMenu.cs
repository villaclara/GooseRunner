using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject pauseButton;
    public static bool isPaused = false;
    private Animator animator;
    public string openAnimationName = "TrOpen";
    public string closeAnimationName = "TrClose";
    public MainCharacterMovement _mainCharacterMovement;
    private Vector2 _characterVelocity;
    AudioManager audioManager;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        animator  = pauseMenu.GetComponent<Animator>();
        isPaused = false;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            PauseGame();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            ResumeGame();
        }
    }

    public void PauseGame()
    {
        pauseButton.SetActive(false);
        animator.SetTrigger("TrOpen");
        isPaused = true;
        _characterVelocity = _mainCharacterMovement._body.velocity;
        _mainCharacterMovement._body.velocity = new Vector2(0f, 0f);
        audioManager.PlaySFX(audioManager.pannelSlide);

    }
    public void ResumeGame()
    {
        animator.SetTrigger("TrClose");
        isPaused = false;
        _mainCharacterMovement._body.velocity = _characterVelocity;
        pauseButton.SetActive(true);    
        audioManager.PlaySFX(audioManager.pannelSlide);
    }

}
