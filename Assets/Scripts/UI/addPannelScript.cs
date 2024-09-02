using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class addPannelScript : MonoBehaviour
{
    public GameObject restartPannel;
    private Animator animator;
    AudioManager audioManager;
    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    public void RestartPannelAnim()
    {
        animator = restartPannel.GetComponent<Animator>();
        animator.SetTrigger("activate");
        StartCoroutine("wait");
    }

    //not working(
    private IEnumerable wait()
    {
        yield return new WaitForSeconds(1f);
        audioManager.PlaySFX(audioManager.pannelSlide);
        yield return null;  
    }
}
