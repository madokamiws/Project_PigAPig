﻿using System.Collections;
using System.Collections.Generic;
using StarkSDKSpace;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace Yes.Game.Chicken
{
    public class GameController_S_GM : MonoBehaviour
    {
        public Button btn_modelNormal;
        
        public GameObject checkpointObj;
        // Start is called before the first frame update
        void Start()
        {
            //btn_modelNormal.onClick.AddListener(OnClickModelNormal);


        }
        public void ShowCheckpoint()
        {
            CheckPointModel.GetPointData(1, 20, (result) =>
            {
                ErrorLogs.Get.DisplayLog("回调成功");
                CheckPointController.Get.Display(result);
            });
        }
        public void OnClickModelNormal()
        {
            //SceneManager.LoadScene("GameNormalModel");
        }

        public void ShowcheckpointObj()
        {
            checkpointObj.SetActive(true);
        }
        public void HidecheckpointObj()
        {
            checkpointObj.SetActive(false);
        }
        public void EXstart()
        {
            SceneManager.LoadScene("GameScence");
        }
    }
}