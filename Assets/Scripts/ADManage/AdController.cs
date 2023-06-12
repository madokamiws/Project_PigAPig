using System.Collections;
using System.Collections.Generic;
using StarkSDKSpace;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AdController : MonoBehaviour
{
    private StarkAdManager.BannerStyle m_style;
    private StarkAdManager.BannerAd m_bannerAdIns;
    private StarkAdManager.InterstitialAd m_InterAdIns;

    private void Start()
    {
        m_style = new StarkAdManager.BannerStyle();
    }

    private int px2dp(int px) => (int)(px * (160 / Screen.dpi));

    // 创建Banner广告
    public void CreateBannerAd(string adId)
    {
        m_style.width = 320;
        m_style.left = 10;
        m_style.top = 100;
        m_bannerAdIns = StarkSDK.API.GetStarkAdManager().CreateBannerAd(adId, m_style, 60);
    }

    // 展示Banner广告
    public void ShowBannerAd()
    {
        if (m_bannerAdIns != null)
        {
            m_bannerAdIns.Show();
        }
    }

    // 调整Banner广告位置和大小
    public void ResizeBannerAd()
    {
        int w = m_style.width;
        int h = m_style.height;
        int sw = px2dp(Screen.width);
        int sh = px2dp(Screen.height);

        m_style.top = sh - h;
        m_style.left = sw / 2 - w / 2;
        m_style.width = w;

        m_bannerAdIns.ReSize(m_style);
    }

    // 创建插屏广告
    public void CreateInterstitialAd(string adId)
    {
        m_InterAdIns = StarkSDK.API.GetStarkAdManager().CreateInterstitialAd(adId);
    }

    // 加载插屏广告
    public void LoadInterstitialAd()
    {
        if (m_InterAdIns != null)
        {
            m_InterAdIns.Load();
        }
    }

    // 展示插屏广告
    public void ShowInterstitialAd()
    {
        if (m_InterAdIns != null)
        {
            m_InterAdIns.Show();
        }
    }

    // 销毁插屏广告
    public void DestroyInterstitialAd()
    {
        if (m_InterAdIns != null)
        {
            m_InterAdIns.Destory();
            m_InterAdIns = null;
        }
    }
}