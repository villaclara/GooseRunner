using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class addPannelScript : MonoBehaviour
{
    public GameObject restartPannel;
    private Animator animator;

    public void RestartPannelAnim()
    {
        animator = restartPannel.GetComponent<Animator>();
        animator.SetTrigger("activate");
    }
}
