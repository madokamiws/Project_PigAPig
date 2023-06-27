using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarkSDKSpace;
using System;
namespace Yes.Game.Chicken
{
    public class MyVideoAdCallbacks : StarkAdManager.VideoAdCallback
    {
        //private Action<bool> onVideoCloseCallback;

        //public MyVideoAdCallbacks(Action<bool> onVideoClose)
        //{
        //    onVideoCloseCallback = onVideoClose;
        //}
        public void OnError(int errCode, string errorMessage)
        {
            // 处理广告错误
            string log = string.Format("广告出错: 错误码 {0}, 错误消息 {1}", errCode, errorMessage);
            ErrorLogs.Get.DisplayLog(log);
        }

        public void OnVideoClose(int watchedTime, int effectiveTime, int duration)
        {
            bool isWatchedTimeGreater = watchedTime > effectiveTime;
            // 处理广告关闭
            string log = string.Format("广告已关闭. 已观看时间: {0}, 有效播放时间: {1}, 视频总时长: {2}", watchedTime, effectiveTime, duration);
            ErrorLogs.Get.DisplayLog(log);
            //onVideoCloseCallback?.Invoke(isWatchedTimeGreater);
        }

        public void OnVideoLoaded()
        {
            // 处理广告加载成功
            ErrorLogs.Get.DisplayLog("广告已加载");
        }

        public void OnVideoShow(long timestamp)
        {
            // 处理广告播放
            string log = string.Format("广告已播放, 时间戳: {0}", timestamp);
            ErrorLogs.Get.DisplayLog(log);
        }
    }
}
