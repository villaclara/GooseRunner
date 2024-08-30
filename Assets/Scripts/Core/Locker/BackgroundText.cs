
using TMPro;
using UnityEngine;

public class BackgroundText : MonoBehaviour
{
    private Vector2 spawnpos;
    private Vector2 newpos;
    string hexColor1 = "#d3fc7e";
    string hexColor2 = "#ea323c";
    string hexColor3 = "#ffeb57";
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
            Color newColor;
            string hexColor = "#ffffff";
            int randColor = Random.Range(1, 4);
            if (randColor == 1)
                hexColor = hexColor1;
            if (randColor == 2)
                hexColor = hexColor2;
            if (randColor == 3)
                hexColor = hexColor3;
            UnityEngine.ColorUtility.TryParseHtmlString(hexColor, out newColor);
            textMeshPro.color = newColor;
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
