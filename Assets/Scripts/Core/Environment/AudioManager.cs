
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
    private void Start()
    {
        _volumeState = PlayerPrefs.GetInt("volumeState", 1);
        ambientSource.clip = background;
        ambientSource.loop = true;
        if (_volumeState == 1)
            ambientSource.Play();
    }

    private void Update()
    {
        _volumeState = PlayerPrefs.GetInt("volumeState", 1);
    }

    public void PlaySFX(AudioClip clip)
    {
        if(_volumeState == 1)
            SFXSource.PlayOneShot(clip);
    }
}
