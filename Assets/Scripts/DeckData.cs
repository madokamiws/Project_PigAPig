using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yes.Game.Chicken
{
    public class DeckData
    {
        public static void GetDeckData(Action<DeckModel> callback = null)
        {
            try
            {

                string url = "robots.txt";
                Dictionary<string, string> param = new Dictionary<string, string>();
                if (Constant.IsDebug)
                {
                    param.Add("debug", "1");
                }
                param.Add("", "");
                BaseHttpHelper.HttpMethod(url, param, (string data) =>
                {
                    //Logs.Log("hero_sign_ins/get_sign_in_lists接口返回数据:" + data);

                    DeckModel model = Newtonsoft.Json.JsonConvert.DeserializeObject<DeckModel>(data);
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
                Debug.Log("get_sign_in_lists报错:" + ex.Message);
            }
        }
    }
    public class DeckModel:BaseModel
    {
        public int checkpoint_type { get; set; }//关卡类型 1 闯关 2挑战
        public int checkpoint_num { get; set; }//第几关
        public int[,,] center_deck { get; set; } //主排队三维数组
        public int totalcard_num { get; set; }//卡牌总数
        public int card_element { get; set; }//卡牌元素
        public int[] around_deck { get; set; }//是否生成周围牌堆 上左1，上右2，下左3，下右4 

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