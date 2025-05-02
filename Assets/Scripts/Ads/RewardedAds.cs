using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RewardedAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] private string _androidAdUnityId;
    public Button adButton;
    public TextMeshProUGUI adText;
    public TextMeshProUGUI gemScoreEndScreen;

    private void Awake()
    {

    }

    
    public void LoadRewardedAd()
    {
        Advertisement.Load(_androidAdUnityId, this);
    }

    public void ShowRewardedAd()
    {
        Advertisement.Show(_androidAdUnityId, this);
        LoadRewardedAd();
    }


    #region LoadCallbacks
    public void OnUnityAdsAdLoaded(string placementId) { }
    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message) { }
    #endregion

    #region ShowCallbacks
    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
    }

    public void OnUnityAdsShowStart(string placementId)
    {
    }

    public void OnUnityAdsShowClick(string placementId)
    {
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (placementId == _androidAdUnityId && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            adButton = GameObject.FindGameObjectWithTag("adButton").GetComponent<Button>();
            adText = GameObject.FindGameObjectWithTag("adText").GetComponent<TextMeshProUGUI>();
            gemScoreEndScreen = GameObject.FindGameObjectWithTag("gemsCollected").GetComponent<TextMeshProUGUI>();
            Debug.Log("Ad fully watched");

            if (adButton == null)
            {
                Debug.LogError("adButton is not assigned.");
                return;
            }

            if (adText == null)
            {
                Debug.LogError("adText is not assigned.");
                return;
            }

            if (gemScoreEndScreen == null)
            {
                Debug.LogError("gemScoreEndScreen is not assigned.");
                return;
            }

            adButton.interactable = false;
            adText.text = "DOUBLED";

            // Ensure gemsCollected has a default value
            if (GameManager.gemsCollected == null)
            {
                Debug.LogError("GameManager.gemsCollected is not assigned.");
                return;
            }

            GameManager.gemsCollected *= 2;

            // Ensure GlobalVariables.gems is not null
            if (GlobalVariables.gems == null)
            {
                Debug.LogError("GlobalVariables.gems is not assigned.");
                return;
            }

            GlobalVariables.gems += GameManager.gemsCollected / 2;
            PlayerPrefs.SetInt("GemScore", GlobalVariables.gems);
            gemScoreEndScreen.text = $"+ {GameManager.gemsCollected.ToString()}";
        }
    }
    #endregion
}
