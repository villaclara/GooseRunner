using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Image = UnityEngine.UI.Image;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject pauseButton;
    public GameObject volumeButton;
    public Sprite volumeOnImg, volumeOffImg;
    public static bool isPaused = false;
    private Animator animator;
    public string openAnimationName = "TrOpen";
    public string closeAnimationName = "TrClose";
    public MainCharacterMovement _mainCharacterMovement;
    private Vector2 _characterVelocity;
    AudioManager audioManager;
    private int _volumeState;
    private Image volumeButtonImg;


    // Start is called before the first frame update
    void Start()
    {

        _volumeState = PlayerPrefs.GetInt("volumeState", 1); //1 - volume turned on, 2 = volume off
        volumeButtonImg = volumeButton.GetComponent<Image>();
        Debug.Log(volumeButtonImg);
        if (_volumeState == 1)
            volumeButtonImg.sprite = volumeOnImg;
        else
            volumeButtonImg.sprite = volumeOffImg;

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


    public void ChangeVolumeState()
    {
        if (_volumeState == 1)
        {
            _volumeState = 2;
            PlayerPrefs.SetInt("volumeState", 2);
            volumeButtonImg.sprite = volumeOffImg;
        }
        else
        {
            _volumeState = 1;
            PlayerPrefs.SetInt("volumeState", 1);
            volumeButtonImg.sprite = volumeOnImg;
        }
    }
}
