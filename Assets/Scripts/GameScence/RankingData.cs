using System;
using System.Collections;
using StarkSDKSpace;
using System.Collections.Generic;
using UnityEngine;
namespace Yes.Game.Chicken
{
    public class RankingData : SingletonPatternMonoAutoBase_DontDestroyOnLoad<RankingData>
    {
        public StarkRank getStarkRank;
        public RankingData()
        {
            getStarkRank = StarkSDK.API.GetStarkRank();
        }
        public void GetRankingData(int id, Action<DeckModel> callback = null)
        {
            try
            {

                string url = "ranking";
                Dictionary<string, string> param = new Dictionary<string, string>();
                if (Constant.IsDebug)
                {
                    param.Add("debug", "1");
                }
                if (PlayerPrefs.HasKey("user_token"))
                {
                    string _token = PlayerPrefs.GetString("user_token");
                    param.Add("token", _token);
                    //ErrorLogs.Get.DisplayLog("get_item_level接口 有token");
                }
                else
                {
                    ErrorLogs.Get.DisplayLog("token没有获取到");
                }

                BaseHttpHelper.HttpMethod(url, param, (string data) =>
                {
                    Logs.Log("get_item_level接口返回数据:" + data);
                    ErrorLogs.Get.DisplayLog(data);
                    DeckModel model = Newtonsoft.Json.JsonConvert.DeserializeObject<DeckModel>(data);
                    if (model.error_code >= 0)
                    {
                        ErrorLogs.Get.DisplayLog("序列化成功");
                        if (callback != null)
                            callback(model);
                    }
                    else
                    {
                        //  Utils.CopyDebug("gget_item_level获取签到数据失败！");
                    }
                });
            }
            catch (Exception ex)
            {
                Debug.Log("get_sign_in_lists报错:" + ex.Message);
            }
        }
        public void SetSDKRankData(int value)
        {
            string _value = value.ToString();

            StarkSDK.API.GetStarkRank().SetImRankData(0, _value,0,null, (boolx, stringx) =>
            {
                string log = string.Format("SetImRankData的回调数据  boolx = {0}，stringx = {1}", boolx, stringx);
                ErrorLogs.Get.DisplayLog(log);
            });
        }

        public void GetSDKRankData()
        {
            ErrorLogs.Get.DisplayLog("进入GetSDKRankData");
            StarkSDK.API.GetAccountManager().Login(OnLoginSuccessCallback, OnLoginFailedCallback);

        }

        void OnLoginSuccessCallback(string code, string anonymousCode, bool isLogin)
        {
            ErrorLogs.Get.DisplayLog("排行榜前的登录成功OnLoginSuccessCallback ... code：" + code + " ，anonymousCode：" + anonymousCode + " ，isLogin：" + isLogin);
            getStarkRank.GetImRankList("all", 0, "default", "金币", "金币排行榜", (boolx, stringx) =>
            {
                string log = string.Format("GetImRankList的回调数据  boolx = {0}，stringx = {1}", boolx, stringx);
                ErrorLogs.Get.DisplayLog(log);
            });

        }
        void OnLoginFailedCallback(string errMsg)
        {
            ErrorLogs.Get.DisplayLog("OnLoginFailedCallback ... errMsg：" + errMsg);
        }


    }

    public class RankModel : BaseModel
    {
        public int level_id { get; set; }//第几关

    }
}