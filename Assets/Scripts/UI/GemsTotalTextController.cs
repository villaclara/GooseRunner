using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GemsTotalTextController : MonoBehaviour
{

    public void DoScoreUpdAnimation()
    {
        this.GetComponent<Animator>().SetTrigger("GemPicked");
	}
}
