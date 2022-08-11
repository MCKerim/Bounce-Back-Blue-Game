using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Audio;

public class AdManager : MonoBehaviour, IUnityAdsListener
{
    private string playStoreID = "3800061";
    private string appStoreID = "3800060";

    private string interstitialAd = "video";
    private string rewardedVideoAd = "rewardedVideo";
    private string doubleCoins = "doubleCoins";
    private string adForCoins = "adForCoins";

    public bool isTargetPlayStore;
    public bool isTestAd;

    public event Action doubleCoinsAdFinished;
    public event Action adForCoinsAdFinished;

    [SerializeField] private AudioMixer masterMixer;

    public static AdManager current;
    private void Awake()
    {
        if(current != null)
        {
            Destroy(gameObject);
        }
        else
        {
            current = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        Advertisement.AddListener(this);
        InitializeAdvertisement();
    }

    private void InitializeAdvertisement()
    {
        if(isTargetPlayStore)
        {
            Advertisement.Initialize(playStoreID, isTestAd);
        }
        else
        {
            Advertisement.Initialize(appStoreID, isTestAd);
        }
    }

    public void PlayAd(string adType)
    {
        if (Advertisement.IsReady(adType))
        {
            Advertisement.Show(adType);
        }
    }

    public void PlayInterstitialAd()
    {
        if (Advertisement.IsReady(interstitialAd))
        {
            Advertisement.Show(interstitialAd);
        }
    }

    public void OnUnityAdsReady(string placementId)
    {
    }

    public void OnUnityAdsDidError(string message)
    {
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        masterMixer.SetFloat("masterVol", -80f);
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        switch (showResult)
        {
            case ShowResult.Failed:
                
                break;

            case ShowResult.Skipped:
                
                break;

            case ShowResult.Finished:
                if(placementId == doubleCoins)
                {
                    doubleCoinsAdFinished?.Invoke();
                }
                else if (placementId == adForCoins)
                {
                    adForCoinsAdFinished?.Invoke();
                }
                break;
        }
       masterMixer.SetFloat("masterVol", 0f);
    }
}
