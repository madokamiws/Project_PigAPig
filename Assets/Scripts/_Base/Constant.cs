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

        public static readonly string HTTP_DATA_EMPTY = "";

        public static readonly bool IsDebug = false;

        public static string avatarUrl { get; set; }
        public static string nickName { get; set; }
        public static int gender { get; set; }
        public static string city { get; set; }
        public static string province { get; set; }
        public static string country { get; set; }
        public static string language { get; set; }

        public static string[] IP_LIST = GetAllUrl();
        public static string[] GetAllUrl()
        {
            if (!IsDebug)
            {

#if !UNITY_EDITOR
                return new string[]{
                    "https://api.glgj.top/api/",
                    "https://api.glgj.top/api/",
                    "https://api.glgj.top/api/"
                };
#endif
                return new string[] { "https://api.glgj.top/api/" };
            }
            else
            {
                return new string[]{
                    "https://api.glgj.top/api/"
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
    }

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
}

