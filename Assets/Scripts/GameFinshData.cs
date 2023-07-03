﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Yes.Game.Chicken
{
    public class GameFinshData
    {
        public static void SubmitLevelData(int id,int is_pass, Action<FinishCallBackModel> callback = null)
        {
            try
            {

                string url = "finish";
                Dictionary<string, string> param = new Dictionary<string, string>();
                if (Constant.IsDebug)
                {
                    param.Add("debug", "1");
                }
                param.Add("user_level_record_id", id.ToString());
                param.Add("is_pass", is_pass.ToString());
                //param.Add("h", ));

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
        //public int level_id { get; set; }
    }
}
