using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class BackGroundTextFlying : MonoBehaviour
{
    public GameObject text;
    private float spawnpos = 6f;
    private Vector2 spawnVector;
    public BackgroundText bgtext;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnText", 0.5f, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnText()
    {
        ////TODO
        ///make proper spawn and bouncing text from each other
        float randpos = Random.Range(-6f, 6f);
        int randN = Random.Range(1, 5);
        if (randN == 1)
            spawnVector = new Vector2(spawnpos, randpos);
        else if(randN == 2)
            spawnVector = new Vector2(randpos, spawnpos);
        else if(randN == 3)
            spawnVector = new Vector2(-spawnpos, randpos);
        else if(randN == 4)
            spawnVector = new Vector2(randpos, -spawnpos);



        Instantiate(text, spawnVector, Quaternion.identity);
        
    }
}
