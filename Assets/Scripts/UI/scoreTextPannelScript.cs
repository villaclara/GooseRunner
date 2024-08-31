using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testScript : MonoBehaviour
{
    public GameObject scorePannel;
    public GameObject gemsPannel;
    private Animator animator;
    AudioManager audioManager;
    // Start is called before the first frame update
    void Start()
    {
        animator = scorePannel.GetComponent<Animator>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GlobalVariables.gameIsRunning)
            animator.SetTrigger("activate");
    }

    public void StartGemsPannelAnim()
    {
        audioManager.PlaySFX(audioManager.pannelSlide);
        animator = gemsPannel.GetComponent<Animator>();
        animator.SetTrigger("activate");
    }

}
