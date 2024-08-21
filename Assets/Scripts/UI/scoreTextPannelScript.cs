using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testScript : MonoBehaviour
{
    public GameObject scorePannel;
    public GameObject gemsPannel;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = scorePannel.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GlobalVariables.gameIsRunning)
            animator.SetTrigger("activate");
    }

    public void StartGemsPannelAnim()
    {
        animator = gemsPannel.GetComponent<Animator>();
        animator.SetTrigger("activate");
    }

}
