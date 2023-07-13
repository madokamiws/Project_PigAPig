using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using BestHTTP;
using UnityEngine;
using Yes.Game.Chicken;
public class BaseHttpHelper
{
    static int response_error = 0;
    public static void HttpMethod(string api, Dictionary<string, string> param, Action<string> callback = null, bool isReload = false)
    {
        if (string.IsNullOrEmpty(api))
        {
            return;
        }

        string api_url = "";
        string ip = Constant.IP;

        if (api.Contains("http://") || api.Contains("https://"))
        {
            api_url = api;
        }
        else
        {
            api_url = string.Format("{0}{1}", ip, api);
        }

        Logs.Log(" *****HttpMethod请求数据***** url =" + api_url);

        Uri uri = new Uri(api_url);

        try
        {

            var requests = new HTTPRequest(uri, HTTPMethods.Post, (request, response) => {

                // Logs.Log(" *****HttpMethod返回数据***** url =" + api_url);

                if (response == null || !response.IsSuccess)
                {

                    if (response != null)
                    {
                        ErrorLogs.Get.DisplayLog(string.Format(" ErrorLogs:{0}-> HttpMethod responseStatusCode: {1} Message: {2} ", api_url, response.StatusCode, response.Message));
                        Logs.Log(string.Format(" {0}-> HttpMethod responseStatusCode: {1} Message: {2} ", api_url, response.StatusCode, response.Message));
                    }

                    response_error++;
                    if (response_error >= 3)
                    {
                        //Constant.IP_INDEX += 1;
                    }

                    bool isReturn = CheckDic(api);
                    if (isReturn)
                    {
                        ErrorLogs.Get.DisplayLog(string.Format(" HttpMethod response : " + response));
                        Logs.Log(" HttpMethod response : " + response);
                        return;
                    }

                    api_url = string.Format("{0}{1}", Constant.IP, api);

                    // Logs.Log(string.Format(" --------->>> HttpMethod 重新加载-{0} ,param-{1} ", api_url, Newtonsoft.Json.JsonConvert.SerializeObject(param)));

                    Constant.IP = ip;
                    HttpMethod(api, param, callback, true);

                    // if ( callback != null ) callback(null);

                    Logs.Log(" HttpMethod response : " + response);
                    return;
                }

                SetApinum(api);

                string resultText = response.DataAsText;

                if (response.StatusCode == 401)
                {
                    ErrorLogs.Get.DisplayLog("返回首页进行登录流程");
                    callback(response.DataAsText);
                }

                if (response.StatusCode == 200)
                {
                    callback(response.DataAsText);
                }
                else
                {
                    if (string.IsNullOrEmpty(resultText) || !resultText.Contains("error_reason"))
                    {
                        response_error++;
                        bool isReturn = CheckDic(api);
                        if (isReturn)
                        {
                            return;
                        }

                        api_url = string.Format("{0}{1}", Constant.IP, api);
                        //Logs.Log(string.Format(" --------->>> HttpMethod 重新加载-{0} ,param-{1} ", api_url, param));
                        Constant.IP = ip;
                        HttpMethod(api, param, callback, true);

                    }
                    else
                    {
                        //Logs.Log(" HttpMethod response.DataAsText : " + response.DataAsText);
                        callback(response.DataAsText);
                    }

                }



                response_error = 0;

            });

            requests.SetHeader("Content-Type", "text/html; charset=utf-8");
            requests.Timeout = TimeSpan.FromSeconds(30);

            Dictionary<string, string> new_param = new Dictionary<string, string>();
            if (param == null) param = new Dictionary<string, string>();
            foreach (var item in param)
            {
                if (!new_param.ContainsKey(item.Key))
                {
                    new_param.Add(item.Key, item.Value);
                    requests.AddField(item.Key, item.Value);
                }


                // Debug.Log(string.Format ( "  HttpMethod item.Key={0}, item.Value={1}", item.Key, item.Value ) ) ;
            }
            //string h = GetAppSignEx(new_param);

            /*
            Dictionary<string, string> list = BaseModel.GetCommonHttpFields();

            // Logs.Log("公共参数Json:" + Newtonsoft.Json.JsonConvert.SerializeObject(list));

            Dictionary<string, string> new_param = new Dictionary<string, string>();
            foreach (var item in list)
            {
                if (!new_param.ContainsKey(item.Key))
                    new_param.Add(item.Key, item.Value);

                // Debug.Log(string.Format ( "  HttpMethod item.Key={0}, item.Value={1}", item.Key, item.Value ) ) ;
            }

            if (param == null) param = new Dictionary<string, string>();
            foreach (var item in param)
            {
                if (!new_param.ContainsKey(item.Key))
                    new_param.Add(item.Key, item.Value);

                // Debug.Log(string.Format ( "  HttpMethod item.Key={0}, item.Value={1}", item.Key, item.Value ) ) ;
            }
            //new_param

            foreach (var item in new_param)
            {
                if (item.Value != null)
                {
                    string fieldName = item.Key;
                    string fieldValue = item.Value;
                    requests.AddField(fieldName, fieldValue);

                }
            }
            string h = GetAppSignEx(new_param);
            // Logs.Log("h:" + h);

            requests.AddField("h", h);
            // Logs.Log("requests:" + Newtonsoft.Json.JsonConvert.SerializeObject(requests.GetFormFields()));

            */
            requests.Send();
            // requests.DisableCache = true ;

        }
        catch (Exception ex)
        {

            // GameController.AdActions += "HttpMethod ex=" + ex .Message + "\n";
            // Toast.Show("HttpMethod=" + ex.Message);
            Debug.LogError(ex.Message);

            // UIShow.Instance.showMessage("GuestRegister Exception ！" + ex.Message) ;
        }

    }
    public static Dictionary<string, int> ip_dic = new Dictionary<string, int>();
    public static bool CheckDic(string api)
    {
        if (ip_dic.ContainsKey(api))
        {
            if (ip_dic[api] >= 1)
            {
                return true;
            }
            else
            {
                ip_dic[api] += 1;
            }
        }
        else
        {
            ip_dic.Add(api, 1);
        }
        return false;
    }
    public static void SetApinum(string api)
    {
        if (ip_dic.ContainsKey(api))
        {
            ip_dic[api] = 0;
        }
    }

    public static string GetAppSignEx(Dictionary<string, string> dic)
    {

        // string key = "b3af64cd0f524b1cbb7f329b8813299d";
        // System.Web.Configuration.WebConfigurationManager.AppSettings["wx_key"].ToString(); ;
        // 商户平台 API安全里面设置的KEY  32位长度
        // 排序

        dic = dic.OrderBy(d => d.Key).ToDictionary(d => d.Key, d => d.Value);

        // 连接字段
        var sign = dic.Aggregate("", (current, d) => current + (d.Key + "=" + d.Value + "&"));
        //if (dic.ContainsKey("adn_type"))
        //{
        //    Util.CopyDebug("**********dic" + Newtonsoft.Json.JsonConvert.SerializeObject(dic));
        //    Util.CopyDebug("*********sign:" + sign);
        //}

        string last = sign.Substring(sign.Length - 1, 1);
        if (last == "&") sign = sign.Substring(0, sign.Length - 1);

        // sign += "key=" + key;
        // MD5

        //UnityEngine.Debug.Log ( "GetAppSignEx 0 =" + sign ) ;

        sign = GetMD5(sign); // System.Security. HashPasswordForStoringInConfigFile(sign, "MD5").ToUpper();
                             //if (dic.ContainsKey("adn_type"))
                             //{
                             //    Util.CopyDebug("*********sign:" + sign);
                             //}
                             //UnityEngine.Debug.Log("GetAppSignEx 1 =" + sign);

        // UnityEngine.Debug.Log("GetAppSignEx 3 =" + sign_final );
        // Logs.Log(" - sign1 =" + sign);
        return sign;
    }
    public static string GetMD5(string myString)
    {
        using (MD5 md5 = MD5.Create())
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(myString);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < hashBytes.Length; i++)
            {
                builder.Append(hashBytes[i].ToString("x2"));
            }

            return builder.ToString();
        }
    }
}

