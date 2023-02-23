using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Yes.Game.Chicken
{
    public class CheckPointModel : BaseModel
    {

        public List<CheckPoint> config_levels { get; set; }
        public int page { get; set; }
        public int per_page { get; set; }
        public int total { get; set; }


        public static void GetDeckData(Action<CheckPointModel> callback = null)
        {
            try
            {
                string url = "get_level_lists";
                Dictionary<string, string> param = new Dictionary<string, string>();
                if (Constant.IsDebug)
                {
                    param.Add("debug", "1");
                }
                param.Add("", "");
                BaseHttpHelper.HttpMethod(url, param, (string data) =>
                {
                    //Logs.Log("hero_sign_ins/get_sign_in_lists接口返回数据:" + data);

                    CheckPointModel model = Newtonsoft.Json.JsonConvert.DeserializeObject<CheckPointModel>(data);
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
                Debug.Log("get_level_lists报错:" + ex.Message);
            }
        }

    }
    public class CheckPoint
    {
        public int id { get; set; }
        public int level_id { get; set; }
        public int golds { get; set; }
        public int layer { get; set; }
        public int length { get; set; }
        public int width { get; set; }
        public int card_total { get; set; }
        public int card_kind { get; set; }
    }

}