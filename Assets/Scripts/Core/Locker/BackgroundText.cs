using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class BackgroundText : MonoBehaviour
{
    private Vector2 spawnpos;
    private Vector2 newpos;
    // Start is called before the first frame update
    void Start()
    {
            
        spawnpos = transform.position;
       
        Transform firstChildTransform = transform.GetChild(0);
        GameObject firstChildObject = firstChildTransform.gameObject;
        Transform secondChildTransform = firstChildObject.transform.GetChild(0);
        GameObject secondChildObject = secondChildTransform.gameObject;

        TextMeshProUGUI textMeshPro = secondChildObject.GetComponent<TextMeshProUGUI>();
        if (textMeshPro != null)
        {
            int randColor = Random.Range(1, 4);
            if (randColor == 1)
                textMeshPro.color = Color.red;
            if (randColor == 2)
                textMeshPro.color = Color.green;
            if (randColor == 3)
                textMeshPro.color = Color.yellow;
        }
    }

    // Update is called once per frame
    void Update()
    {
      
        if(spawnpos.x > 0 && spawnpos.y > 0)
        {
            newpos.x = transform.position.x - 0.7f * Time.deltaTime;
            newpos.y = transform.position.y - 0.7f * Time.deltaTime;
        }
        else if(spawnpos.x > 0 && spawnpos.y < 0)
        {
            newpos.x = transform.position.x - 0.7f * Time.deltaTime;
            newpos.y = transform.position.y + 0.7f * Time.deltaTime;
        }
        else if (spawnpos.x < 0 && spawnpos.y > 0)
        {
            newpos.x = transform.position.x + 0.7f * Time.deltaTime;
            newpos.y = transform.position.y - 0.7f * Time.deltaTime;
        }
        else if (spawnpos.x < 0 && spawnpos.y < 0)
        {
            newpos.x = transform.position.x + 0.7f * Time.deltaTime;
            newpos.y = transform.position.y + 0.7f * Time.deltaTime;
        }

        transform.position = newpos;


        if (Mathf.Abs(transform.position.x) > 10)
            Destroy(gameObject);
        if (Mathf.Abs(transform.position.y) > 10)
            Destroy(gameObject);
    }

    
}
