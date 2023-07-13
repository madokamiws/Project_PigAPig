using System;
using System.Collections;
using System.Collections.Generic;
using StarkSDKSpace;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace Yes.Game.Chicken
{
	public class LoginController : MonoBehaviour
	{
        public Text tx_Logs;
        public Text api_Log;
        void Start()
		{

            //StarkSDK.API.FollowDouYinUserProfile(OnFollowCallback, OnFollowError);
            try
			{
                CheckSession();
            }
			catch (Exception ex)
			{
				Logs.Log("初始化错误:ex = " + ex.Message);
			}

		}
        void CheckSession()
        {
            StarkSDK.API.GetAccountManager().CheckSession(OnCheckSessionSuccessCallback, OnCheckSessionFailedCallback);
        }

        void OnCheckSessionSuccessCallback()
        {
            //登录游戏逻辑
            ErrorLogs.Get.DisplayLog("CheckSession 接口调用成功 ");
            if (PlayerPrefs.HasKey("user_token"))
            {
                GameStart.Get.StartGame();
            }
            else
            {
                StarkSDK.API.GetAccountManager().Login(OnLoginSuccessCallback,
OnLoginFailedCallback);
            }
        }
        void OnCheckSessionFailedCallback(string errMsg)
        {
            ErrorLogs.Get.DisplayLog("CheckSession 接口调用失败，进入登录流程");
            StarkSDK.API.GetAccountManager().Login(OnLoginSuccessCallback,
OnLoginFailedCallback);

        }
        string sucesslog = "";
        string failedlog = "";
        /// <summary>
        /// 登录成功回调
        /// </summary>
        /// <param name="code">临时登录凭证, 有效期 3 分钟。可以通过在服务器端调用 登录凭证校验接口 换取 openid 和 session_key 等信息。</param>
        /// <param name="anonymousCode">用于标识当前设备, 无论登录与否都会返回, 有效期 3 分钟</param>
        /// <param name="isLogin">判断在当前 APP(头条、抖音等)是否处于登录状态</param>
        void OnLoginSuccessCallback(string code, string anonymousCode, bool isLogin)
        {
            ErrorLogs.Get.DisplayLog("OnLoginSuccessCallback ... code：" + code + " ，anonymousCode：" + anonymousCode + " ，isLogin：" + isLogin);
            Debug.Log("OnLoginSuccessCallback ... code：" + code + " ，anonymousCode：" + anonymousCode + " ，isLogin：" + isLogin);
            sucesslog = string.Format("登录成功\n OnLoginSuccessCallback ... code：" + code + " ，anonymousCode：" + anonymousCode + " ，isLogin：" + isLogin + "\n");
            ErrorLogs.Get.DisplayLog(sucesslog);
            tx_Logs.text = sucesslog;
            Loading.Show();
            LoginData.GetLoginData(code, (result) =>
            {
                Loading.Hide();
                PlayerPrefs.SetString("user_token", result.user.token);
                PlayerPrefs.Save();
                //记录 result
                ErrorLogs.Get.DisplayLog("记录 token = " + result.user.token);
                api_Log.text = string.Format("api/login接口返回数据:{0}" + result);

                StarkSDK.API.GetAccountManager().GetScUserInfo((ref ScUserInfo scUserInfo) =>
                {
                    ErrorLogs.Get.DisplayLog("GetScUserInfo：");
                    Constant.avatarUrl = scUserInfo.avatarUrl;
                    Constant.nickName = scUserInfo.nickName;
                    Constant.gender = scUserInfo.gender;
                    Constant.city = scUserInfo.city;
                    Constant.province = scUserInfo.province;
                    Constant.country = scUserInfo.country;
                    Constant.language = scUserInfo.language;

                    Loading.Show();
                    LoginData.UpdateUser((updateUserresult) =>
                    {
                        Loading.Hide();
                    });

                }, OnGetScUserInfoFailedCallback);
            });


            //CopyDebug.OnClickCopyText(sucesslog);
        }
        /// <summary>
        /// 检查Session接口调用失败的回调函数
        /// </summary>
        /// <param name="errMsg">错误原因</param>
        void OnLoginFailedCallback(string errMsg)
        {
            Debug.Log("OnLoginFailedCallback ... errMsg：" + errMsg);
            ErrorLogs.Get.DisplayLog("OnLoginFailedCallback ... errMsg：" + errMsg);
            failedlog = string.Format(sucesslog + "登录失败\n OnLoginFailedCallback ... errMsg：" + errMsg + "\n");
            tx_Logs.text = failedlog;
        }

        void OnFollowCallback()
        {
            ErrorLogs.Get.DisplayLog("OnFollowCallback");
        }
        void OnFollowError(int num1, string str_2)
        {
            ErrorLogs.Get.DisplayLog("num1 ="+ num1+ "-----str_2=" + str_2);
        }
        void OnGetScUserInfoFailedCallback(string errMsg)
        {
            ErrorLogs.Get.DisplayLog("errMsg =" + errMsg);
        }

    }

}