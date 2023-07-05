using System.Collections;
using System.Collections.Generic;
using StarkSDKSpace;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace Yes.Game.Chicken
{
    public class CheckPointItem : MonoBehaviour
    {
        public Text pointID;
        public int self_ID = 0;
        public GameObject isUnlock;
        // Start is called before the first frame update
        void Start()
        {

        }


        public void LoadData(CheckPoint point)
        {
            ErrorLogs.Get.DisplayLog("LoadData");
            pointID.gameObject.SetActive(true);
            self_ID = point.id;
            pointID.text = self_ID.ToString();

            if (point.unlock == 0)
            {
                isUnlock.SetActive(true);
                //if (self_ID-1 >= 0)
                //{
                //    PlayerPrefs.SetInt("CurrentLevelIDMax", self_ID-1);
                //    PlayerPrefs.Save();
                //}

            }
        }
        public void OnClickPoint()
        {
            if (self_ID > 0)
            {
                PlayerPrefs.SetInt("CurrentLevelIDMax", self_ID);
                PlayerPrefs.Save();
                ErrorLogs.Get.DisplayLog("保存CurrentLevelIDMax ：" + self_ID);
                SceneManager.LoadScene("GameScence");
            }
        }

        public void OnClose()
        {
            Destroy(gameObject);
        }
    }
} 