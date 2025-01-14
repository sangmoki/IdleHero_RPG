using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class ADS_Manager
{
    private bool TestMode = true;

    // 실제 광고 Key값
    public readonly string banner_Android_id = "ca-app-pub-1387955594217908/1960728208";
    public readonly string interstitial_Android_id = "ca-app-pub-1387955594217908/2387237646";
    public readonly string reward_Android_id = "ca-app-pub-1387955594217908/4462736647";

    // 테스트 광고 Key값
    public readonly string banner_Android_test = "ca-app-pub-3940256099942544/6300978111";
    public readonly string interstitial_Android_test = "ca-app-pub-3940256099942544/1033173712";
    public readonly string reward_Android_test = "ca-app-pub-3940256099942544/5224354917";

    BannerView _bannerAd;           // 배너
    InterstitialAd _interstitialAd; // 전면광고
    RewardedAd _rewardAd;           // 보상형 광고
    AdRequest _adRequest;

    // 광고를 사용할 때
    // 필수적으로 호출해야하는 초기화 함수
    public void Init()
    {
        MobileAds.Initialize(initStatus => { });
        PrepareADS();
    }

    // 광고 준비(초기화)
    private void PrepareADS()
    {
        string banner;
        string interstitial;
        string reward;

        // 테스트 용도 광고 key값
        if (TestMode)
        {
            banner = banner_Android_test;
            interstitial = interstitial_Android_test;
            reward = reward_Android_test;
        }
        // 실제 광고 key값
        else
        {
            banner = banner_Android_id;
            interstitial = interstitial_Android_id;
            reward = reward_Android_id;
        }

        _adRequest = new AdRequest();
        _adRequest.Keywords.Add("unity-admob-sample");

        BannerView(banner);
    }

    // 배너 초기화
    public void BannerView(string banner_id)
    {
        if (_bannerAd != null)
        {
            _bannerAd.Destroy();
            _bannerAd = null;
        }

        // 가로 풀사이즈
        AdSize adaptiveSize = AdSize.GetLandscapeAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);

        // 하단 배너
        _bannerAd = new BannerView(banner_id, adaptiveSize, AdPosition.Bottom);
        _bannerAd.LoadAd(_adRequest);
    }
}
