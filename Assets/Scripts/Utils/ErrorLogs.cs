using System.Collections;
using System.Collections.Generic;
using StarkSDKSpace;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace Yes.Game.Chicken
{
    public class ErrorLogs : BaseDialogController
    {
        public Text tx_logs;
        public static ErrorLogs _instance;
        public static ErrorLogs Get
        {
            get
            {
                if (_instance == null)
                {
                    string path = "apiLog";
                    _instance = (ErrorLogs)ErrorLogs.InitBase(_instance, path);
                }
                return (ErrorLogs)_instance;
            }
        }
        public override void Awake()
        {
            _instance = this;
        }

        public void DisplayLog(string logs)
        {

            tx_logs.text += "\n------"+logs;
        }

    }
}