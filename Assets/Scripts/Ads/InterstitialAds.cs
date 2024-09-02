using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class InterstitialAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] private string _androidAdUnityId;

    private void Awake()
    {
        
    }

    public void LoadInterstitialAd()
    {
        Advertisement.Load(_androidAdUnityId, this);
    }

    public void ShowInterstitialAd()
    {
        Advertisement.Show(_androidAdUnityId, this);
        LoadInterstitialAd();
    }


    #region LoadCallbacks
    public void OnUnityAdsAdLoaded(string placementId){}
    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message){}
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
        Debug.Log("add showed");
    }
    #endregion
}
