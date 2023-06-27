using System;
using StarkSDKSpace;
using UnityEngine;

namespace Yes.Game.Chicken
{
public class AdController
    {
        private StarkAdManager.BannerStyle m_style;
        private StarkAdManager.BannerAd m_bannerAdIns;
        private StarkAdManager.InterstitialAd interstitialAd;
        StarkAdManager starkManager = StarkSDK.API.GetStarkAdManager();

        private bool videoAdResult;
        private static AdController instance;

        public static AdController Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AdController();
                }
                return instance;
            }
        }
        private AdController()
        {
            m_style = new StarkAdManager.BannerStyle();
            ErrorLogs.Get.DisplayLog("什么时候");
        }
        #region 激励视频
        //public delegate void VideoCloseCallback(bool isWatchedTimeGreaterThanEffectiveTime);

        //public event VideoCloseCallback OnVideoCloseCallback;
        public void ShowRewardVideoAd(string videoAdId, Action<bool> Callback)
        {
            //MyVideoAdCallbacks rewardcallbacks = new MyVideoAdCallbacks(onVideoClose);
            MyVideoAdCallbacks rewardcallbacks = new MyVideoAdCallbacks();
            starkManager.ShowVideoAdWithId(
                videoAdId, (CloseCallback)=>
                {
                    Callback(CloseCallback);
                }, OnRewardAdError, rewardcallbacks);
        }
        private void OnRewardAdError(int errorCode, string errorMsg)
        {
            ErrorLogs.Get.DisplayLog("errorCode = " + errorCode + "errorDescription = " + errorMsg);
        }
        void CloseCallback(bool closeCallback, Action<bool> Callback)
        {
            if (closeCallback)
            {
                ErrorLogs.Get.DisplayLog("closeCall返回true");
            }
            else
            {
                ErrorLogs.Get.DisplayLog("closeCall返回false");
            }
            Callback(closeCallback);
        }
#endregion

#region 插屏广告
        public void ShowInterstitialAd(string ad_id)
        {
            DestoryInterstitialAd();
            interstitialAd = starkManager.CreateInterstitialAd(
                ad_id, OnInsAdError/*, OnInsAdClose, OnInsAdLoaded*/);

            if (interstitialAd != null)
            {
                interstitialAd.Load(); // 将加载插屏广告的操作放在这里，不在加载完成后立即显示
            }
        }
        private void OnInsAdError(int errorCode, string errorMsg)
        {
            ErrorLogs.Get.DisplayLog("errorCode = " + errorCode + "errorDescription = " + errorMsg);
            //interstitialAd = null; // Dispose ad after error
        }
        private void OnInsAdClose()
        {
            //if (interstitialAd != null)
            //{
            //    interstitialAd.Destory();
            //    interstitialAd = null;
            //}
        }

        private void OnInsAdLoaded()
        {
        }
        public void DisplayInterstitialAd()
        {
            ErrorLogs.Get.DisplayLog("显示插屏AD");
            if (interstitialAd != null)
                interstitialAd.Show();
            else
            {
                ErrorLogs.Get.DisplayLog("插屏AD未创建");
            }
        }
        void DestoryInterstitialAd()
        {
            ErrorLogs.Get.DisplayLog("销毁插屏AD");
            if (interstitialAd != null)
                interstitialAd.Destory();
            interstitialAd = null;
        }
        #endregion

        #region banner广告
        private int px2dp(int px) => (int)(px * (160 / Screen.dpi));
        // 创建Banner广告
        public void CreateBannerAd(string adId)
        {
            m_style.width = 320;
            m_style.left = 10;
            m_style.top = 100;
            m_bannerAdIns = StarkSDK.API.GetStarkAdManager().CreateBannerAd(adId, m_style, 60, ErrorCallback, LoadedCallback, ResizeCallback);


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
        #endregion
    }


}