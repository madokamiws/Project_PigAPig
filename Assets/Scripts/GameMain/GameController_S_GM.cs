using System.Collections;
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

        //public GameObject checkpointObj;
        public AudioClip BGM_Click;

        public CheckPointModel checkPointModel;
        public static GameController_S_GM Instance { get; private set; }
        public static GameController_S_GM Get
        {
            get
            {
                if (!Instance)
                {
                    Instance = new GameController_S_GM();
                }
                return Instance;
            }
        }
        void Awake()
        {
            Instance = this;
            
        }
        void Start()
        {
            //btn_modelNormal.onClick.AddListener(OnClickModelNormal);
            //GameRecorderController.Instance.Show();
            AudioManager.Instance.PlayBGM(BGM_Click);
        }
        public void ShowCheckpoint()
        {
            CheckPointModel.GetPointData(1, 20, (result) =>
            {
                checkPointModel = result;
                ErrorLogs.Get.DisplayLog("回调成功");

                CheckPointController.Get.Display(checkPointModel);
            });
        }
        public void OnClickModelNormal()
        {
            //SceneManager.LoadScene("GameNormalModel");
        }

        public void OnShowSetting()
        {
            SettingController.Get.Show();
        }
        public void OnShowRanking()
        { 
        
        }
        public void OnStarkGameRecorder()
        {
            GameRecorderController.Instance.StartRecord();
        }
        public void EXstart()
        {
            SceneManager.LoadScene("GameScence");
        }

    }
}