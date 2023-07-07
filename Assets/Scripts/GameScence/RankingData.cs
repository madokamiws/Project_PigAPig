using System;
using System.Collections;
using StarkSDKSpace;
using System.Collections.Generic;
using UnityEngine;
namespace Yes.Game.Chicken
{
    public class RankingData
    {
        //public StarkRank getStarkRank;
        public RankingData()
        {
             //var x = StarkSDK.API.GetStarkRank().SetImRankData();
        }
        public static void GetDeckData(int id, Action<DeckModel> callback = null)
        {
            try
            {

                string url = "get_item_level";
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
                param.Add("config_level_id", id.ToString());
                ErrorLogs.Get.DisplayLog("请求的 id =  " + id);
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
        public void SetRankData(int value)
        {
            string _value = value.ToString();
            //getStarkRank.SetImRankData(0, _value);
        }

    }

    public class RankModel : BaseModel
    {
        public int level_id { get; set; }//第几关

    }
}