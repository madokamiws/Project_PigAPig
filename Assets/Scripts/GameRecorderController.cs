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

        }
        void OnRecordErrorCallback(int errCode, string errMsg)
        {
            string log = string.Format("errCode = {0},  errMsg = {1}", errCode, errMsg);

            ErrorLogs.Get.DisplayLog(log);
        }
        void OnRecordCompleteCallback(string videoPath)
        {
            string log = string.Format("videoPath = {0}, videoPath");

            ErrorLogs.Get.DisplayLog(log);
        }
    }
}