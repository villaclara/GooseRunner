
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource anmbientSource;
    [SerializeField] AudioSource SFXSource;


    public AudioClip background;
    public AudioClip gemPickUp;
    public AudioClip speedUpOrbPickUp;
    public AudioClip slowDownOrbPickUp;
    public AudioClip buttonPressed;
    public AudioClip pannelSlide;


    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
