using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using System.Reflection;
using System.Linq;
using UnityEditor;

namespace Yes.Game.Chicken
{
    [System.Serializable]
    public class BaseModel
    {

        public int id { get; set; }

        // [IgnoreAttribute]
        // public string sid { get; set; }

        public int error_code { get; set; }


        public BaseModel()
        {

        }

        //

        public static string GetNetworkReachabilityType()
        {
            string _net = "";
            if (Application.internetReachability == NetworkReachability.NotReachable)
                _net = "";
            else if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
                _net = "g";
            else if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
                _net = "wifi";

            return _net;
            // Application.internetReachability.
            // return Application.internetReachability ;
            // NetworkReachability

        }


    }

}
