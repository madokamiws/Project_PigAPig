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

        public void DisplayLog(string logs,bool ishuanhang = true)
        {
            if (ishuanhang)
            {
                tx_logs.text += "\n------" + logs;
                Debug.Log(logs);
            }
            else
            {
                tx_logs.text +=  logs;
                Debug.Log(logs);
            }

        }
        public void ShowLog()
        {
            gameObject.SetActive(true);
        }
        public void OnClickCloseLog()
        {
            gameObject.SetActive(false);
        }

    }
}