﻿using System.Collections;
using System.Collections.Generic;
using StarkSDKSpace;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScreenRecording : MonoBehaviour
{
    public Button startBtn;
    public Button endBtn;
    public Button shareBtn;

    private AsyncOperation ao;
    public bool ifLoadGameScene;

    
    void Start()
    {
       
       StarkSDK.API.GetAccountManager().Login(OnLoginSuccessCallback,
                   OnLoginFailedCallback,true);

        
        startBtn.onClick.AddListener(StartVideo);
        //endBtn.onClick.AddListener(StopVideo);
        shareBtn.onClick.AddListener(ShareVideo);
        ao = SceneManager.LoadSceneAsync(1);
        ao.allowSceneActivation = false;
    }

    /// <summary>
    /// 登录成功回调
    /// </summary>
    /// <param name="code">临时登录凭证, 有效期 3 分钟。可以通过在服务器端调用 登录凭证校验接口 换取 openid 和 session_key 等信息。</param>
    /// <param name="anonymousCode">用于标识当前设备, 无论登录与否都会返回, 有效期 3 分钟</param>
    /// <param name="isLogin">判断在当前 APP(头条、抖音等)是否处于登录状态</param>
    void OnLoginSuccessCallback(string code, string anonymousCode, bool isLogin)
    {
        Debug.Log("OnLoginSuccessCallback ... code：" + code + " ，anonymousCode：" + anonymousCode + " ，isLogin：" + isLogin);
    }
    /// <summary>
    /// 检查Session接口调用失败的回调函数
    /// </summary>
    /// <param name="errMsg">错误原因</param>
    void OnLoginFailedCallback(string errMsg)
    {
        Debug.Log("OnLoginFailedCallback ... errMsg：" + errMsg);
    }



    void StartVideo()
    {
        Debug.Log("抖音 开启录制视频 ...");
        // <param name="isRecordAudio">是否录制声音，默认为录制声音</param>
        // <param name="maxRecordTimeSec">最大录制时长，单位 s。小于等于 0 则无限制。默认为10分钟</param>
        // <param name="startCallback">视频录制开始回调</param>
        // <param name="errorCallback">视频录制失败回调</param>
        bool isStart = StarkSDK.API.GetStarkGameRecorder().StartRecord(true, 200,
            StartCallback, FailedCallback, SuccessCallback);
        Debug.Log("视频开启录制结果 ..." + isStart);
        if (ifLoadGameScene)
        {
            ao.allowSceneActivation = true;
        }
        else
        {
            Destroy(gameObject);
        }


    }

    void StopVideo()
    {
        Debug.Log("抖音 停止录制视频 ...");
        bool isStop = StarkSDK.API.GetStarkGameRecorder().StopRecord(SuccessCallback, FailedCallback, null, false);
        Debug.Log("停止录制视频状态 ..." + isStop);
    }

    void StartCallback()
    {
        Debug.Log("视频开始录制回调执行 ...");
        // 开始回调逻辑，比如：显示录屏中按钮
    }

    void FailedCallback(int errCode, string errMsg)
    {
        Debug.Log("录制视频失败回调执行 ... 错误码是：" + errCode + " ，错误消息是：" + errMsg);
        // 失败回调逻辑，比如：隐藏录屏中按钮
    }

    void SuccessCallback(string videoPath)
    {
        Debug.Log("视频录制完成实际路径：" + videoPath);
        // 成功回调逻辑，比如：隐藏录屏中按钮
    }

    void ShareVideo()
    {
        Debug.Log("ShareVideo ShareVideo ...");
        // 只有回调，不带预定义标题和话题
        StarkSDK.API.GetStarkGameRecorder().ShareVideo(SuccessCallback, FailedCallback, CancelledCallback);

        // <param name="successCallback">分享成功回调</param>
        // <param name="failedCallback">分享失败回调</param>
        // <param name="cancelledCallback">分享取消回调</param>
        // <param name="title">分享视频的标题，如不需要设置标题，可以传null或空字符串</param>
        // <param name="topics">分享视频的话题，如不需要设置话题，可以传null或空列表</param>
        //StarkSDK.API.GetStarkGameRecorder().ShareVideoWithTitleTopics(SuccessCallback, FailedCallback,
        //    CancelledCallback, "自定义标题", new List<string>() {"自定义话题1", "自定义话题2"});

        void SuccessCallback(Dictionary<string, object> dictionary)
        {
            Debug.Log("视频分享成功回调 ...");
            // 成功回调逻辑，比如：弹窗提示并发放奖励
        }

        void CancelledCallback()
        {
            Debug.Log("取消分享回调 ...");
            // 取消回调逻辑，比如：弹窗提示
        }

        void FailedCallback(string errMsg)
        {
            Debug.Log("分享视频失败回调执行 ... " + " ，错误消息是：" + errMsg);
            // 失败回调逻辑，比如：弹窗提示
        }
    }
}