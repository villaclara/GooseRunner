using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Advertisements;
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
        if(placementId == _androidAdUnityId && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Debug.Log("add fully watched");
            adButton.interactable = false;
            adText.text = "DOUBLED";
            GameManager.gemsCollected *= 2;
            GlobalVariables.gems += GameManager.gemsCollected/2;
            PlayerPrefs.SetInt("GemScore", GlobalVariables.gems);
            gemScoreEndScreen.text = $"+ {GameManager.gemsCollected.ToString()}";
        }
    }
    #endregion
}
