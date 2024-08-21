using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    private Animator animator;
    private string animBoolName = "isRunning";
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        animator.SetBool(animBoolName, false);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.gameStarted)
        {

            if (animator.GetBool(animBoolName))
            {
                if (!GlobalVariables.gameIsRunning)
                    animator.SetBool(animBoolName, false);
                if (PauseMenu.isPaused)
                {
                    animator.SetBool(animBoolName, false);
                    animator.speed = 0;
                }
            }
            else
            {
                if (GlobalVariables.gameIsRunning && !PauseMenu.isPaused)
                    animator.SetBool(animBoolName, true);
                if (!PauseMenu.isPaused)
                    animator.speed = 1;
            }
        }


    }
}
