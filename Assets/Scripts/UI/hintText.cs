
using TMPro;
using UnityEngine;

public class hintText : MonoBehaviour
{
    public TextMeshProUGUI text;
    public RectTransform rect; // Reference to the TextMeshPro object
    public float speed = GlobalVariables.fallSpeed;      // Speed of the movement
    
    void Update()
    {
        speed = GlobalVariables.fallSpeed;
        if (!PauseMenu.isPaused)
        {
            // Calculate the new position
            Vector3 newPosition = rect.position - new Vector3(0, speed * Time.deltaTime * 252, 0);

            // Set the new position
            rect.position = newPosition;
        }
        if (rect.position.y <= -600f)
        {
            Destroy(text.gameObject);
        }
    }

}
