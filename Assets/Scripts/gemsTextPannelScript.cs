using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gemsTextPannelScript : MonoBehaviour
{
    public GameObject addPannel;
    private Animator animator;
    public void AddPannelAnim()
    {
        animator = addPannel.GetComponent<Animator>();
        animator.SetTrigger("activate");
    }
}
