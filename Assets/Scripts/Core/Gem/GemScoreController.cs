using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemScoreController : MonoBehaviour
{
    public GemBehaviour _gemBehavior;
    public GemBehaviour currentGem;

	public void DoScoreUpdAnimation()
    {
        this.GetComponent<Animator>().SetTrigger("GemPicked");
    }
}
