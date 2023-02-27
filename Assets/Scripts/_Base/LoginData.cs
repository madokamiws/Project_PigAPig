using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yes.Game.Chicken
{
    public class LoginData
    {
        public static void GetLoginData(string code,Action<LoginUserData> callback = null)
        {
            try
            {

                string url = "login";
                Dictionary<string, string> param = new Dictionary<string, string>();
                if (Constant.IsDebug)
                {
                    param.Add("debug", "1");
                }
                param.Add("code", code);
                ErrorLogs.Get.DisplayLog(code);
                BaseHttpHelper.HttpMethod(url, param, (string data) =>
                {
                    Logs.Log("api/login接口返回数据:" + data);
                    ErrorLogs.Get.DisplayLog("api/login接口返回数据:" + data);
                    LoginUserData model = Newtonsoft.Json.JsonConvert.DeserializeObject<LoginUserData>(data);
                    if (model.error_code >= 0)
                    {

                        if (callback != null)
                            callback(model);
                    }
                    else
                    {
                        //  Utils.CopyDebug("get_sign_in_lists获取签到数据失败！");
                    }
                });
            }
            catch (Exception ex)
            {
                Debug.Log("get_login_in_lists报错:" + ex.Message);
            }
        }
    }
    public class LoginUserData : BaseModel
    {
        public string openid { get; set; }
        public string unionid { get; set; }
        public string session_key { get; set; }
        public string token { get; set; }
        public int updated_at { get; set; }
        public int created_at { get; set; }
        public int id { get; set; }
    }
}
