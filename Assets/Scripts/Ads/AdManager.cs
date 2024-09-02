using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdManager : MonoBehaviour
{
    public InitializeAds initializeAds;
    public InterstitialAds interstitialAds;
    public RewardedAds rewardedAds;

    public static AdManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);


        interstitialAds.LoadInterstitialAd();
        rewardedAds.LoadRewardedAd();
    }
}
