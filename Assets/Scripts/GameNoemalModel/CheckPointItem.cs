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
        public GameObject isUnlock;
        // Start is called before the first frame update
        void Start()
        {

        }


        public void LoadData(CheckPoint point)
        {
            ErrorLogs.Get.DisplayLog("LoadData");
            pointID.gameObject.SetActive(true);
            pointID.text = point.id.ToString();
            if (point.unlock == 0)
            {
                isUnlock.SetActive(true);

            }
        }
    }
} 