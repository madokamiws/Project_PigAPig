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
        void Start()
		{
			try
			{
				StarkSDK.API.GetAccountManager().Login(OnLoginSuccessCallback,
			OnLoginFailedCallback, true);

			}
			catch (Exception ex)
			{
				Logs.Log("初始化错误:ex = " + ex.Message);
			}

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
            Debug.Log("OnLoginSuccessCallback ... code：" + code + " ，anonymousCode：" + anonymousCode + " ，isLogin：" + isLogin);
            sucesslog = string.Format("登录成功\n OnLoginSuccessCallback ... code：" + code + " ，anonymousCode：" + anonymousCode + " ，isLogin：" + isLogin + "\n");
            tx_Logs.text = sucesslog;
            LoginData.GetLoginData(code, (result) =>
            {

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
            failedlog = string.Format(sucesslog + "登录失败\n OnLoginFailedCallback ... errMsg：" + errMsg + "\n");
            tx_Logs.text = failedlog;
        }


    }
}