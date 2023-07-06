using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Yes.Game.Chicken
{
    public class GameFinishData
    {
        public static void SubmitLevelData(int id,int is_pass, Action<FinishCallBackModel> callback = null)
        {
            try
            {
                int duration = 0;
                string url = "finish";
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
                if (CountdownController.Instance)
                {
                    duration = (int)CountdownController.Instance.GetTotalTime();
                }
                string _h = string.Format("{0}chicken.", is_pass);
                ErrorLogs.Get.DisplayLog("_h:" + _h);
                string h = BaseHttpHelper.GetMD5(_h);
                ErrorLogs.Get.DisplayLog("getmd5 之后 h:" + h);


                param.Add("user_level_record_id", id.ToString());
                param.Add("is_pass", is_pass.ToString());
                param.Add("h", h);
                param.Add("duration", duration.ToString());
                BaseHttpHelper.HttpMethod(url, param, (string data) =>
                {
                    ErrorLogs.Get.DisplayLog("finish接口返回数据:" + data);
                    FinishCallBackModel model = Newtonsoft.Json.JsonConvert.DeserializeObject<FinishCallBackModel>(data);
                    if (model.error_code >= 0)
                    {
                        ErrorLogs.Get.DisplayLog("序列化成功");
                        if (callback != null)
                            callback(model);
                    }
                    else
                    {

                    }
                });
            }
            catch (Exception ex)
            {
                ErrorLogs.Get.DisplayLog("finish报错:" + ex.Message);
            }
        }
    }

    public class FinishCallBackModel : BaseModel
    {
        public int level_id { get; set; }//第几关
        public int config_level_id { get; set; }//关卡配置后台记录id
    }
}
