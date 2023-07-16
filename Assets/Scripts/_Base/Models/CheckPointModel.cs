using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Yes.Game.Chicken
{
    public class CheckPointModel : BaseModel
    {

        public List<CheckPoint> config_levels { get; set; }
        public int page { get; set; }//当前页
        public int per_page { get; set; }//每页多个数据
        public int total { get; set; }//总条数

        public int level_id { get; set; }//第几关

        public int config_level_id { get; set; }//关卡配置后台记录id

        /// <summary>
        ///  获取关卡列表
        /// </summary>
        /// <param name="callback"></param>
        public static void GetPointData(int page,int per_page,Action<CheckPointModel> callback = null)
        {
            try
            {
                string url = "get_level_lists";
                Dictionary<string, string> param = new Dictionary<string, string>();
                if (Constant.IsDebug)
                {
                    param.Add("debug", "1");
                }
                if (PlayerPrefs.HasKey("user_token"))
                {
                    string _token = PlayerPrefs.GetString("user_token");
                    param.Add("token", _token);
                    ErrorLogs.Get.DisplayLog("有token = "+ _token);
                    //ErrorLogs.Get.DisplayLog(_token);
                }
                else
                {
                    //没有token的逻辑
                    ErrorLogs.Get.DisplayLog("token没有获取到");
                }

                //param.Add("page", page.ToString());
                //param.Add("per_page", per_page.ToString());

                BaseHttpHelper.HttpMethod(url, param, (string data) =>
                {
                    Logs.Log("get_level_lists接口返回数据:" + data);
                    ErrorLogs.Get.DisplayLog(data);
                    CheckPointModel model = Newtonsoft.Json.JsonConvert.DeserializeObject<CheckPointModel>(data);
                    if (model.error_code >= 0)
                    {
                        ErrorLogs.Get.DisplayLog("序列化成功");
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
                Debug.Log("get_level_lists报错:" + ex.Message);
            }
        }

    }
    public class CheckPoint
    {
        public int id { get; set; }
        public int unlock { get; set; }
    }

}