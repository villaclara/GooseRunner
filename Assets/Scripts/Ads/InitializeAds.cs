using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
public class InitializeAds : MonoBehaviour, IUnityAdsInitializationListener
{
    [SerializeField] private string _androidGameId;
    [SerializeField] private bool _isTesting;


    #region interface functions
    public void OnInitializationComplete(){}
    public void OnInitializationFailed(UnityAdsInitializationError error, string message){}
    #endregion

    private void Awake()
    {
        if(!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(_androidGameId, _isTesting, this);
        }
    }

}
