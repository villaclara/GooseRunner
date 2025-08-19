using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gemsTextPannelScript : MonoBehaviour
{
    public GameObject addPannel;
    public GameObject restartPannel;
    private Animator animator;
    AudioManager audioManager;
    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    public void AddPannelAnim()
    {
        audioManager.PlaySFX(audioManager.pannelSlide);
        //animator = addPannel.GetComponent<Animator>();
        animator = restartPannel.GetComponent<Animator>();
        animator.SetTrigger("activate");
    }
}
