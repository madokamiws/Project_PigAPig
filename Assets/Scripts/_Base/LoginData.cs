using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Yes.Game.Chicken
{
    public class LoginData
    {
        public static void GetLoginData(string code,Action<LoginModel> callback = null)
        {
            try
            {

                string url = "code";
                Dictionary<string, string> param = new Dictionary<string, string>();
                if (Constant.IsDebug)
                {
                    param.Add("debug", "1");
                }
                param.Add("code", code);
                BaseHttpHelper.HttpMethod(url, param, (string data) =>
                {
                    //Logs.Log("hero_sign_ins/get_sign_in_lists接口返回数据:" + data);

                    LoginModel model = Newtonsoft.Json.JsonConvert.DeserializeObject<LoginModel>(data);
                    //if (model.error_code >= 0)
                    //{

                    //    if (callback != null)
                    //        callback(model);
                    //}
                    //else
                    //{
                    //    //  Utils.CopyDebug("get_sign_in_lists获取签到数据失败！");
                    //}
                });
            }
            catch (Exception ex)
            {
                Debug.Log("get_sign_in_lists报错:" + ex.Message);
            }
        }
    }
    public class LoginModel : BaseModel
    {
        public int code { get; set; }


    }
}
