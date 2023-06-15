﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yes.Game.Chicken
{
    public class DeckData
    {
        public static void GetDeckData(int id, Action<SinglePointData> callback = null)
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
                    //没有token的逻辑
                    ErrorLogs.Get.DisplayLog("token没有获取到");
                }
                param.Add("id", id.ToString());
                BaseHttpHelper.HttpMethod(url, param, (string data) =>
                {
                    Logs.Log("get_item_level接口返回数据:" + data);
                    ErrorLogs.Get.DisplayLog(data);
                    SinglePointData model = Newtonsoft.Json.JsonConvert.DeserializeObject<SinglePointData>(data);
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
    }
    public class SinglePointData : BaseModel
    {
        public int id { get; set; }//第几关
        public int level_id { get; set; }//第几关

        public DeckModel level_map_data { get; set; }
    }
    public class DeckModel
    {
        //public int checkpoint_type { get; set; }//关卡类型 1 闯关 2挑战
        //public int checkpoint_num { get; set; }//第几关
        public int[,,] centerDeck { get; set; } //主排队三维数组
        public int[,,] centerCardIndex { get; set; } //元素索引
        public int totalCardNum { get; set; }//卡牌总数
        //public int card_element { get; set; }//卡牌元素
        //public int[] around_deck { get; set; }//是否生成周围牌堆 上左1，上右2，下左3，下右4 

    }

    /// <summary>
    /// 主牌堆卡牌的生成状态枚举
    /// </summary>
    public enum CREATESTATE
    {
        NONE = 0,//该位置不生成卡牌
        CREATE = 1,//生成并且位置可能偏移
        RANDOM = 2,//可能生成也可能偏移

        ONLYCREATE = 3,//生成一定不偏移  center

        UPPERCREATE = 4,//上
        LOWERCREATE = 5,//下
        LEFTCREATE = 6,//左
        RIGHTCREATE = 7,//右
        UPPERLEFTCREATE = 8,//左上
        UPPERRIGHTCREATE = 9,//右上
        LOWERRLEFTCREATE = 10,//左下
        LOWERRIGHTCREATE = 11,//右下
    }
}