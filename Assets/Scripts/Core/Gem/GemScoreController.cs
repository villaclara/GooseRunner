using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemScoreController : MonoBehaviour
{
    public GemBehaviour _gemBehavior;
    public GemBehaviour currentGem;

	private void Awake()
	{
        
	}

	// Start is called before the first frame update
	void Start()
    {
		//_gemBehavior.GemCollected += Do;
		//Debug.LogError("Subscribe to event");
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
