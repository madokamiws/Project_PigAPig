using System;
using StarkSDKSpace;
using UnityEngine;
using System.Collections.Generic;
namespace Yes.Game.Chicken
{
    public class GameRecorderController : SingletonPatternMonoAutoBase_DontDestroyOnLoad<GameRecorderController>
    {
        public bool isrecoding = false;//当前是否在录制视频
        StarkGameRecorder starkManager;
        void Start()
        {
            starkManager = StarkSDK.API.GetStarkGameRecorder();
        }


        public void StartRecording()
        {
            isrecoding = true;
            Toast.Show("已开启录屏，进入关卡时自动开始录制",3);
            
        }
        /// <summary>
        /// 真正开始录屏
        /// </summary>
        public void StartSCRecord()
        {
            starkManager.StartRecord(true, 600, OnRecordStartCallback, OnRecordErrorCallback, OnRecordCompleteCallback);
        }

        public void StopRecording()
        {
            isrecoding = false;
            Toast.Show("结束录制,仅在游戏胜利界面分享录屏");
            starkManager.StopRecord(OnRecordCompleteCallback, OnRecordErrorCallback);
        }
        public void ShareRecord()
        {
            starkManager.ShareVideo(OnShareVideoSuccessCallback, OnShareVideoFailedCallback,OnShareVideoCancelledCallback);
        }
        void OnRecordStartCallback()
        {
            ErrorLogs.Get.DisplayLog("开始录屏");
        }
        void OnRecordErrorCallback(int errCode, string errMsg)
        {
            string log = string.Format("录屏errCode = {0},  errMsg = {1}", errCode, errMsg);

            ErrorLogs.Get.DisplayLog(log);
        }
        void OnRecordCompleteCallback(string videoPath)
        {
            string log = string.Format("录屏videoPath = {0}, videoPath");

            ErrorLogs.Get.DisplayLog(log);
        }
        void OnShareVideoSuccessCallback(Dictionary<string, object> result)
        {
            foreach (KeyValuePair<string, object> entry in result)
            {
                ErrorLogs.Get.DisplayLog("Key: " + entry.Key + " Value: " + entry.Value.ToString());
            }
        }

        void OnShareVideoFailedCallback(string errMsg)
        {
            string log = string.Format("分享录屏  errMsg = {0}", errMsg);

            ErrorLogs.Get.DisplayLog(log);
        }
        void OnShareVideoCancelledCallback()
        {
            ErrorLogs.Get.DisplayLog("视频分享取消");
        }

    }
}