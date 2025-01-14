using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

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
    RewardedAd _rewardedAd;           // 보상형 광고
    AdRequest _adRequest;
    Action _rewardedCallback;       // 보상형 광고 시청 후 액션


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

        RewardedAd.Load(reward, _adRequest, OnAdRewardCallback);

        // 전면 광고
        //InterstitialAd.Load(interstitial, _adRequest, OnAdLoadCallback);
    }

    // 보상형 광고 콜백
    private void OnAdRewardCallback(RewardedAd ad, LoadAdError error)
    {
        if (error != null || ad == null)
        {
            Debug.LogError("보상형 광고 로드 실패 : " + error);
            return;
        }

        Debug.Log("보상형 광고 준비 성공" + ad.GetResponseInfo());
        _rewardedAd = ad;
        RegisterEventHandlers(_rewardedAd);
    }

    // 보상형 광고 콜백 상태 여부
    private void RegisterEventHandlers(RewardedAd ad)
    {
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("보상형 광고 닫힘");
            PrepareADS();
        };

        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("보상형 광고 실패 : " + error);
            PrepareADS();
        };

        // 지급할 보상
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log("보상형 광고 완료 : " + adValue);
            if (_rewardedCallback != null)
            {
                _rewardedCallback?.Invoke();
                _rewardedCallback = null;
            }
        };
    }

    // 보상형 광고 실행
    // OnadPaid에서 보상 지급이 원활히 이루어 지지 않아 추가 콜백 실행
    public void ShowRewardedAds(Action rewardCallback)
    {
        _rewardedCallback = rewardCallback;
        // 광고가 준비됐거나 열렸다면
        if (_rewardedCallback != null && _rewardedAd.CanShowAd())
        {
            _rewardedAd.Show((Reward reward) =>
            {
                Debug.Log(reward.Type + " : " + reward.Amount);

                // 이 부분은 광고를 열고 닫으면 바로 보상을 취득할 수 있기에
                // 실 배포 시에는 지우기 (테스트에서만 넣어야함)
                if (_rewardedCallback != null)
                {
                    _rewardedCallback?.Invoke();
                    _rewardedCallback = null;
                }
            });
        }
        else
        {
            PrepareADS();
        }
    }

        /*  전면 광고
        // 준비된 광고가 있다면 전면광고 실행
        public void ShowInterstitialAds()
        {
            if (_interstitialAd != null && _interstitialAd.CanShowAd())
            {
                _interstitialAd.Show();
            }
            else
            {
                // 없다면 준비
                PrepareADS();
            }
        }


        // 광고 호출 콜백
        private void OnAdLoadCallback(InterstitialAd ad, LoadAdError error)
        {
            // error가 null이 아니거나 ad가 null이면 광고 로드 실패
            if (error != null || ad == null)
            {
                Debug.Log("전면광고 로드 실패 : " + error);
                return;
            }

            Debug.Log("전면 광고 로드 : " + ad.GetResponseInfo());

            // 전면광고 준비
            _interstitialAd = ad;

            // Handler 호출
            RegisterReloadHandler(_interstitialAd);
        }

        // 전면 광고 준비 핸들러
        private void RegisterReloadHandler(InterstitialAd ad)
        {
            ad.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("전면 광고 닫힘");
                PrepareADS();
            };

            ad.OnAdFullScreenContentFailed += (AdError error) =>
            {
                Debug.LogError("전면광고 실패 : " + error);
                PrepareADS();
            };
        }*/
        /* 배너 광고
            // 배너 광고 실행
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
            }*/
    }
