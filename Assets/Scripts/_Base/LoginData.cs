using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yes.Game.Chicken
{
    public class LoginData
    {
        public static void GetLoginData(string code,Action<Usermodel> callback = null)
        {
            try
            {

                string url = "login";
                Dictionary<string, string> param = new Dictionary<string, string>();
                if (Constant.IsDebug)
                {
                    param.Add("debug", "1");
                }
                //string code_test = "nL1FWUGAPvVJrBrVkOAB78MdBUVOw-kUp7PeN07UL-udCL_h3QziRowaKpmYk3O0-UZKRF2L03RPRK7TBJKvJm8YChKGgfcLltPMEeR1KjNMyqYfybdpBK-t7eQ";
                param.Add("code", code);
                BaseHttpHelper.HttpMethod(url, param, (string data) =>
                {
                    Logs.Log("api/login接口返回数据:" + data);
                    //ErrorLogs.Get.DisplayLog("api/login接口返回数据: = " + data);
                    Usermodel model = Newtonsoft.Json.JsonConvert.DeserializeObject<Usermodel>(data);
                    if (callback != null)
                    {
                        ErrorLogs.Get.DisplayLog("回调成功 token = "+ model.user.token);
                        callback(model);
                    }

                });
            }
            catch (Exception ex)
            {
                Debug.Log("get_login_in_lists报错:" + ex.Message);
            }
        }

        public static void UpdateUser(Action<UpdateUserCallback> callback = null)
        {
            try
            {

                string url = "update_user";
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
                param.Add("avatar_url", Constant.avatarUrl);
                param.Add("nick_name", Constant.nickName);
                param.Add("gender", Constant.gender.ToString());
                param.Add("city", Constant.city);
                param.Add("province", Constant.province);
                param.Add("country", Constant.country);


                //ErrorLogs.Get.DisplayLog(code);
                BaseHttpHelper.HttpMethod(url, param, (string data) =>
                {
                    ErrorLogs.Get.DisplayLog("api/update_user接口返回数据:" + data);
                    UpdateUserCallback model = Newtonsoft.Json.JsonConvert.DeserializeObject<UpdateUserCallback>(data);
                    if (callback != null)
                    {
                        callback(model);
                    }
                });
            }
            catch (Exception ex)
            {
                Debug.Log("update_user报错:" + ex.Message);
            }
        }
    }
    [System.Serializable]
    public class Usermodel : BaseModel
    {
        public LoginUserData user { get; set; }
    }
    public class UpdateUserCallback : BaseModel
    {
        //public LoginUserData user { get; set; }
    }
    [System.Serializable]
    public class LoginUserData
    {
        public string openid { get; set; }
        public string unionid { get; set; }
        public string session_key { get; set; }
        public string token { get; set; }
        //public string updated_at { get; set; }
        //public string created_at { get; set; }
        public int id { get; set; }
    }
}
