
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource ambientSource;
    [SerializeField] AudioSource SFXSource;


    public AudioClip background;
    public AudioClip gemPickUp;
    public AudioClip speedUpOrbPickUp;
    public AudioClip slowDownOrbPickUp;
    public AudioClip buttonPressed;
    public AudioClip pannelSlide;
    private int _volumeState;
    private bool _isNotPlaying;
    private void Start()
    {
        _isNotPlaying = true;
        _volumeState = PlayerPrefs.GetInt("volumeState", 1);
        ambientSource.clip = background;
        ambientSource.loop = true;
        if (_volumeState == 1)
        {
            ambientSource.volume = Mathf.Clamp01(1.0f);
            SFXSource.volume = Mathf.Clamp01(1.0f);
        }
        else
        {
            ambientSource.volume = Mathf.Clamp01(0.0f);
            SFXSource.volume = Mathf.Clamp01(0.0f);
        }

    }

    private void Update()
    {
        _volumeState = PlayerPrefs.GetInt("volumeState", 1);
        if (GameManager.gameStarted && _isNotPlaying)
        {
            PlayAmbientMusic();
            _isNotPlaying = false;
        }
        if (_volumeState == 1)
        {
            ambientSource.volume = Mathf.Clamp01(1.0f);
            SFXSource.volume = Mathf.Clamp01(1.0f);
        }
        else
        {
            ambientSource.volume = Mathf.Clamp01(0.0f);
            SFXSource.volume = Mathf.Clamp01(0.0f);
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if(_volumeState == 1)
            SFXSource.PlayOneShot(clip);
    }
    public void PlayAmbientMusic()
    {
        ambientSource.Play();
    }
}
