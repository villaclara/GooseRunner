using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GemsTotalTextController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DoScoreUpdAnimation()
    {
        this.GetComponent<Animator>().SetTrigger("GemPicked");
	}
}
