using System.Collections;
using System.Collections.Generic;
using StarkSDKSpace;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace Yes.Game.Chicken
{
public class AdController : MonoBehaviour
    {
        private StarkAdManager.BannerStyle m_style;
        private StarkAdManager.BannerAd m_bannerAdIns;
        private StarkAdManager.InterstitialAd m_InterAdIns;

        private static AdController instance;
        //private StarkAdManager adManager = StarkSDK.API.GetStarkAdManager();


        public static AdController Instance { get; private set; }
        public static AdController Get
        {
            get
            {
                if (!Instance)
                {
                    Instance = new AdController();
                }
                return Instance;
            }
        }
        private void Awake()
        {
            Instance = this;
        }
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
            m_bannerAdIns = StarkSDK.API.GetStarkAdManager().CreateBannerAd(adId, m_style, 60, ErrorCallback, LoadedCallback, ResizeCallback);


        }
        public void ShowRewardVideoAd(string videoAdId/*, Action<bool> closeCallback = null, Action<int, string> errorCallback = null, VideoAdCallback adCallback = null*/)
        {
            StarkSDK.API.GetStarkAdManager().ShowVideoAdWithId(videoAdId, CloseCallback, ErrorCallback/*adCallback*/);
        }

        void CloseCallback(bool closeCallback)
        {
            if (closeCallback)
            {
                ErrorLogs.Get.DisplayLog("closeCall返回true");
            }
            else
            {
                ErrorLogs.Get.DisplayLog("closeCall返回false");
            }
        }
        //void VideoAdCallback.OnError(int errCode, string errorMessage)
        //{
        //    // 在这里处理广告失败的情况
        //    ErrorLogs.Get.DisplayLog("广告失败：" + errorMessage + "，错误码：" + errCode);
        //}

        //void VideoAdCallback.OnVideoClose(int watchedTime, int effectiveTime, int duration)
        //{
        //    // 在这里处理广告关闭的情况
        //    ErrorLogs.Get.DisplayLog("广告关闭，已播放时长：" + watchedTime + "，有效播放时长：" + effectiveTime + "，视频总时长：" + duration);
        //}

        //void VideoAdCallback.OnVideoLoaded()
        //{
        //    // 在这里处理广告加载成功的情况
        //    ErrorLogs.Get.DisplayLog("广告加载成功");
        //}

        //void VideoAdCallback.OnVideoShow(long timestamp)
        //{
        //    // 在这里处理广告播放成功的情况
        //    ErrorLogs.Get.DisplayLog("广告播放成功，触发时间：" + timestamp);
        //}


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

        void ErrorCallback(int errorCode, string errorDescription)
        {
            // 在此处理广告错误回调
            // errorCode 表示错误码
            // errorDescription 表示错误描述
            ErrorLogs.Get.DisplayLog("errorCode = " + errorCode + "errorDescription = " + errorDescription);


        }

        void LoadedCallback()
        {
            // 在此处理广告加载完成后的回调
            ShowBannerAd();
        }

        void ResizeCallback(int width, int height)
        {
            // 在此处理广告样式改变时的回调
            // width 表示广告宽度
            // height 表示广告高度
        }
    }

}