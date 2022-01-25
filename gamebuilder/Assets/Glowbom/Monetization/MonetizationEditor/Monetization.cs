using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if GLOWBOM_ADS
using GoogleMobileAds.Api;
#endif

/*
 * Created on Mon Dec 16 2019
 *
 * Copyright (c) 2019 Glowbom.
 */
[System.Serializable]
public class Monetization
{
    public string androidBanner;
    public string androidInterstitial;
    public string androidAppId;

    public string iOSBanner;
    public string iOSInterstitial;
    public string iOSAppId;
    public int showAdsIn = 6;

    private int adCounter = 0;
 #if GLOWBOM_ADS

    private BannerView bannerView;
    private InterstitialAd interstitial;
#endif

    private void requestBanner()
    {
#if GLOWBOM_ADS

#if UNITY_ANDROID
        string adUnitId = androidBanner;
#elif UNITY_IPHONE
        string adUnitId = iOSBanner;
#else
        string adUnitId = "unexpected_platform";
#endif

        // Create a 320x50 banner at the top of the screen.
        this.bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.BottomRight);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        this.bannerView.LoadAd(request);

#endif
    }

    private void requestInterstitial()
    {
#if GLOWBOM_ADS
#if UNITY_ANDROID
        string adUnitId = androidInterstitial;
#elif UNITY_IPHONE
        string adUnitId = iOSInterstitial;
#else
        string adUnitId = "unexpected_platform";
#endif

        // Initialize an InterstitialAd.
        this.interstitial = new InterstitialAd(adUnitId);
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        this.interstitial.LoadAd(request);
#endif
    }

    public void destroy()
    {
 #if GLOWBOM_ADS
        if (interstitial != null)
        {
            interstitial.Destroy();
        }
#endif
    }

    public void showInterstitial()
    {
#if GLOWBOM_ADS
        if (interstitial == null)
        {
            requestInterstitial();
        }

        if (interstitial != null && this.interstitial.IsLoaded())
        {
            interstitial.Show();

            showBanner();
        }
#endif
    }

    public void showBanner()
    {
        requestBanner();
    }

    public void initAds()
    {
        MonetizationLoader monetizationLoader = new MonetizationLoader();
        monetizationLoader.load();
        iOSAppId = monetizationLoader.monetization.iOSAppId;
        androidAppId = monetizationLoader.monetization.androidAppId;
        iOSBanner = monetizationLoader.monetization.iOSBanner;
        androidBanner = monetizationLoader.monetization.androidBanner;
        iOSInterstitial = monetizationLoader.monetization.iOSInterstitial;
        androidInterstitial = monetizationLoader.monetization.androidInterstitial;

#if GLOWBOM_ADS
#if UNITY_ANDROID
        string appId = androidAppId;
#elif UNITY_IPHONE
        string appId = iOSAppId;
#else
        string appId = "unexpected_platform";
#endif

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(appId);
#endif
    }

    public void tryShowAds()
    {
        ++adCounter;
        if (adCounter % showAdsIn == 0)
        {
            showInterstitial();
        }

        if (adCounter > 100)
        {
            adCounter = 0;
        }
    }
}
