using System;
using StarkSDKSpace;
using UnityEngine;
namespace Yes.Game.Chicken
{
    public class GameRecorderController : SingletonPatternMonoAutoBase_DontDestroyOnLoad<GameRecorderController>
    {
        StarkGameRecorder starkManager = StarkSDK.API.GetStarkGameRecorder();

        public void StartRecord()
        {
            starkManager.StartRecord(true, 600, OnRecordStartCallback, OnRecordErrorCallback, OnRecordCompleteCallback);
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
    }
}