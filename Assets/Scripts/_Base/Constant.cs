using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Linq;

namespace Yes.Game.Chicken
{

    public class Constant
    {

        public static readonly string app_version = Application.version;// "0.15";

        // areaYield

        public static float foodConsumption = 0.0001158f; //  正常计算 0.0001158f
        public static float foodConsumptionSimulation = 0.0333f; // 0.1666f ; // 一天2斤
        public static int daySecond = 60;
        public static int reliefFood = 200000;
        public static float areaYield = 600;

        public static int hint = 4;

        public static float foodStore = 100000;
        public static float goldStore = 100000;

        public static float landUseRate = 0.1f; // 0.05f ; // * 1500
        public static bool isLineUp = false;


        public static int BuffQDGold = 200;
        public static int TreasureBoxRewardGold = 200;

        // Gold

        // 0.0001158f
        // public static Vector2[] vector2s = { Vector2.left, Vector2.right, Vector2.up, Vector2.down } ;
        // GameType

        public const string UserAgreement = "http://123le.com/idiomempire/protocoluser2.html";
        public const string PrivacyPolicy = "http://123le.com/idiomempire/protocolPrivacy2.html";


        public static string ckey = "com.yes.IdiomEmpire";
        public static string fr = "yingyongbao";//ios
        public static string code = "idiom_empire";

        public static string share_url = "http://dev.hotkk.cn/m/hotel/";

        public static readonly string HTTP_DATA_EMPTY = "";
        public static readonly Vector2[] Vector2s = { Vector2.left, Vector2.right, Vector2.up, Vector2.down };
        public static Vector2[] Vector2Directions = { Vector2.left, Vector2.right, Vector2.up, Vector2.down };


        // Frame DroppedExp
        // Boss Player Enemy HeroWeapon
        public readonly static string TagHeroWeapon = "HeroWeapon";
        public readonly static string TagEnemy = "HeroEnemy";
        public readonly static string TagEnemyBird = "HeroEnemy";


        public readonly static string TagPlayer = "Player";
        public readonly static string TagBoss = "Boss";
        public readonly static string TagFrame = "Frame";

        public readonly static string TagDroppedExp = "HeroDroppedExp";
        public readonly static string TagDroppedItem = "HeroDroppedItem";

        public readonly static string TagDroppedBox = "HeroDroppedBox";
        public readonly static string TagDroppedBoxItem = "HeroDroppedBoxItem";
        // BlockDroppedBoxItem

        public readonly static string TagArrowBomb = "ArrowBomb";
        public readonly static string TagTreasureBox = "TreasureBox";
        public readonly static string TagHeroMagicBox = "HeroMagicBox";
        public readonly static string TagHealingGrape = "HealingGrape";
        public readonly static string TagCharacter = "Character";

        public readonly static string TagHeroBlockFrame = "HeroBlockFrame";


        #region 2022年08月12日新增

        public static readonly bool IsDebug = true;

        public static string[] IP_LIST = GetAllUrl();
        public static string[] GetAllUrl()
        {
            if (!IsDebug)
            {

#if !UNITY_EDITOR
                return new string[]{
                    "https://api.h2hh.cn/api/",
                    "https://api.h3hh.cn/api/",
                    "https://api.h5hh.cn/api/"
                };
#endif
                return new string[] { "https://api.h2hh.cn/api/" };
            }
            else
            {
                return new string[]{
                    "http://dev.hh3h.cn/api/"
                };
            }

        }

        public static Dictionary<string, IPConfig> IPS = new Dictionary<string, IPConfig>();
        public static string IP
        {
            get
            {
                if (IPS.Count == 0)
                {

                    for (int i = 0; i < IP_LIST.Length; i++)
                    {
                        string ip = IP_LIST[i];
                        if (!IPS.ContainsKey(ip))
                            IPS.Add(ip, new IPConfig(ip));
                    }
                }
                var query = from d in IPS orderby d.Value.error_count select d.Value;
                var list = IPS.OrderBy(p => p.Value.error_count).FirstOrDefault();

                // Logs.Log(string.Format(" GET list.Key={0} , list.Value.error_count={1} ", list.Key, list.Value.error_count));
                return list.Key;
            }
            set
            {
                string ip = value;
                if (IPS.ContainsKey(ip))
                {
                    IPS[ip].error_count += 1;
                    // Logs.Log(string.Format(" ip ={0} , IPS[ip].error_count={1} ", ip, IPS[ip].error_count));
                }
            }
        }
        #endregion



        // HeroBlockFrame
        // Character
        // TreasureBox
        // ArrowBomb

        Constant()
        {

        }


        public static void DeleteAll()
        {
            PlayerPrefs.DeleteAll();
        }
    }

    #region 8月12日新增
    public class IPConfig
    {

        public int error_count { get; set; }
        public string api { get; set; }
        public string ip { get; set; }

        public IPConfig(string ip, string api = "")
        {
            this.api = api;
            this.ip = ip;
            this.error_count = error_count;
        }
    }
    #endregion
}

