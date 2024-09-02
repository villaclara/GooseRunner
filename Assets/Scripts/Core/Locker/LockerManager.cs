using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;
using NUnit.Framework.Internal;

public class LockerManager : MonoBehaviour
{

    [SerializeField] public Skin[] skins;
    private bool[] StockCheck;
    //[SerializeField] public Button[] buttonsBehind;
    public Sprite greenBox, greyBox;
    public Button selectButton;
    public TextMeshProUGUI selectButtonText;
    private int _chosenSkinIndex;
    private Button _prevButtonBehind;
    public CanvasGroup canvasGroup;
    public SceneController sceneController;
    public GameObject costPannel;
    public TextMeshProUGUI costText;
    private int _tappedSkinIndex;
    public TextMeshProUGUI gemsText;
    AudioManager audioManager;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        StartCoroutine(sceneController.FadeOutScreen());
        gemsText.text = PlayerPrefs.GetInt("GemScore").ToString();
        _chosenSkinIndex = PlayerPrefs.GetInt("SkinIndex", 0);

        StockCheck = new bool[skins.Length];
        //for(int i = 1; i<skins.Length; i++) //reset bought skins
        //{
        //    StockCheck[i] = false;
        //}
        //SaveStock();
        //PlayerPrefs.SetInt("GemScore", 200);

        if (PlayerPrefs.HasKey("StockArray"))
            StockCheck = PlayerPrefsX.GetBoolArray("StockArray");

        else
            StockCheck[0] = true;


        for (int i = 0; i < skins.Length; i++)
        {
            if (StockCheck[i] == true)
                skins[i].lockImage.SetActive(false);
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
        costPannel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void SaveStock()
    {
        PlayerPrefsX.SetBoolArray("StockArray", StockCheck);
    }

    public void OnClick(Button clickedButton)
    {
        audioManager.PlaySFX(audioManager.buttonPressed);
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
                if (skins[i].buttonBehind == clickedButton && StockCheck[i])
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
                    costPannel.SetActive(false);
                }
                else if(skins[i].buttonBehind == clickedButton && !StockCheck[i])
                {
                    selectButtonText.text = "BUY";
                    selectButton.interactable = true;
                    _tappedSkinIndex = i;
                    costPannel.SetActive(true);
                    costText.text = skins[i].cost.ToString();
                }
            }

        }
        if(clickedButton ==  selectButton)
        {
            if (selectButtonText.text == "BUY")
            {
                if (int.Parse(gemsText.text) >= skins[_tappedSkinIndex].cost)
                {
                    PlayerPrefs.SetInt("GemScore", int.Parse(gemsText.text) - skins[_tappedSkinIndex].cost);
                    gemsText.text = PlayerPrefs.GetInt("GemScore").ToString();
                    StockCheck[_tappedSkinIndex] = true;
                    skins[_tappedSkinIndex].isBought = true;
                    SaveStock();
                    skins[_tappedSkinIndex].lockImage.SetActive(false);
                    selectButtonText.text = "SELECT";
                    costPannel.SetActive(false);
                }
            }
            else
            {

                selectButtonText.text = "SELECTED";
                selectButton.interactable = false;
                for (int i = 0; i < skins.Length; i++)
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
}



[System.Serializable]
public class Skin
{
    public Button buttonBehind;
    public int cost;
    public bool isBought;
    public bool isChosen;
    public GameObject lockImage;
}


