using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

public class LockerManager : MonoBehaviour
{

    [SerializeField] public Skin[] skins;
    //[SerializeField] public Button[] buttonsBehind;
    public Sprite greenBox, greyBox;
    public Button selectButton;
    public TextMeshProUGUI selectButtonText;
    private int _chosenSkinIndex;
    private Button _prevButtonBehind;
    public CanvasGroup canvasGroup;
    public SceneController sceneController;

    public TextMeshProUGUI gemsText;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(sceneController.FadeOutScreen());
        gemsText.text = PlayerPrefs.GetInt("GemScore").ToString();
        _chosenSkinIndex = PlayerPrefs.GetInt("SkinIndex", 0);

        for(int i = 0; i < skins.Length; i++)
        {
            if (i == _chosenSkinIndex)
            {
                skins[i].isChosen = true;
                Image bttnIm = skins[i].buttonBehind.GetComponent<Image>();
                bttnIm.sprite = greenBox;
                _prevButtonBehind = skins[i].buttonBehind;
            }
        }

        selectButtonText.text = "SELECTED";
        selectButton.interactable = false;

    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void OnClick(Button clickedButton)
    { 
        if (clickedButton.tag == "skinButton")
        {
            if (clickedButton != _prevButtonBehind)
            {
                Image bttnIm = clickedButton.GetComponent<Image>();
                bttnIm.sprite = greenBox;

                bttnIm = _prevButtonBehind.GetComponent<Image>();
                bttnIm.sprite = greyBox;
                _prevButtonBehind = clickedButton;
            }

            for(int i = 0; i <skins.Length; i++)
            {
                if (skins[i].buttonBehind == clickedButton)
                {
                    if(!skins[i].isChosen)
                    {
                        selectButtonText.text = "SELECT";
                        selectButton.interactable = true;
                    }
                    else
                    {
                        selectButtonText.text = "SELECTED";
                        selectButton.interactable = false;
                    }
                }
            }

        }
        if(clickedButton ==  selectButton)
        {
            selectButtonText.text = "SELECTED";
            selectButton.interactable = false;
            for (int i = 0; i<skins.Length; i++)
            {
                if (skins[i].isChosen == true)
                    skins[i].isChosen = false;
            }
            for (int i = 0; i < skins.Length; i++)
            {
                if (skins[i].buttonBehind == _prevButtonBehind)
                {
                    skins[i].isChosen = true;
                    _chosenSkinIndex = i;
                    PlayerPrefs.SetInt("SkinIndex", _chosenSkinIndex);
                }
            }
        }

    }
}



[System.Serializable]
public class Skin
{
    public Button buttonBehind;
    public int cost;
    public bool isBought;
    public bool isChosen;
}


