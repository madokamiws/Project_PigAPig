using System;
using System.Collections.Generic;
using StarkSDKSpace;
using UnityEngine;


namespace Yes.Game.Chicken
{
public class AdController: SingletonPatternBase<AdController>
    {
        private static readonly string videoAdId = "o2rfpjvnbi3me7479l";
        private static readonly string interstitialAdId = "2ola8sqldo9f93o6h3";
        private static readonly string bannerAdId = "1d4sorsmjwt5bajk29";

        private StarkAdManager.BannerStyle m_style;
        private StarkAdManager.BannerAd m_bannerAdIns;
        private StarkAdManager.InterstitialAd interstitialAd;
        private StarkAdManager starkManager;

        private bool videoAdResult;

        private AdController()
        {
            starkManager = StarkSDK.API.GetStarkAdManager();
            m_style = new StarkAdManager.BannerStyle();
            ErrorLogs.Get.DisplayLog("什么时候");
        }
        #region 激励视频
        //public delegate void VideoCloseCallback(bool isWatchedTimeGreaterThanEffectiveTime);

        //public event VideoCloseCallback OnVideoCloseCallback;
        public void ShowRewardVideoAd(Action<bool> Callback)
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
        public void ShowInterstitialAd()
        {
            DestoryInterstitialAd();
            interstitialAd = starkManager.CreateInterstitialAd(
                interstitialAdId, OnInsAdError/*, OnInsAdClose, OnInsAdLoaded*/);

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
        public void CreateBannerAd()
        {
            m_style.width = 320;
            m_style.left = 10;
            m_style.top = 100;
            m_bannerAdIns = starkManager.CreateBannerAd(bannerAdId, m_style, 60, ErrorCallback, LoadedCallback, ResizeCallback);
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

        /// <summary>
        /// 广告数据提交
        /// </summary>
        /// <param name="type">必选[1、2、3 ] (1是激励,2是插屏,3是banner)</param>
        /// <param name="status">必选参数 是否正常播放 1是正常, 2异常</param>
        /// <param name="reason">可选参数 广告异常 提交异常描述</param>
        /// <param name="callback"></param>
        public void SubmitADData(int type, int status, string reason = null, Action<AdModel> callback = null)
        {
            try
            {
                int duration = 0;
                string url = "ads/report";
                Dictionary<string, string> param = new Dictionary<string, string>();
                if (Constant.IsDebug)
                {
                    param.Add("debug", "1");
                }
                if (PlayerPrefs.HasKey("user_token"))
                {
                    string _token = PlayerPrefs.GetString("user_token");
                    param.Add("token", _token);
                }
                else
                    ErrorLogs.Get.DisplayLog("token没有获取到");
                param.Add("type", type.ToString());
                param.Add("status", status.ToString());
                if (reason!=null)
                {
                    param.Add("reason", reason);
                }
                Loading.Show();
                BaseHttpHelper.HttpMethod(url, param, (string data) =>
                {
                    Loading.Hide();
                    ErrorLogs.Get.DisplayLog("ads/report接口返回数据:" + data);
                    AdModel model = Newtonsoft.Json.JsonConvert.DeserializeObject<AdModel>(data);
                    if (model.error_code >= 0)
                    {
                        ErrorLogs.Get.DisplayLog("序列化成功");
                        if (callback != null)
                            callback(model);
                    }
                    else
                    {

                    }
                });
            }
            catch (Exception ex)
            {
                ErrorLogs.Get.DisplayLog("finish报错:" + ex.Message);
            }
        }

    }
    public class AdModel : BaseModel
    { 
    
    }


}