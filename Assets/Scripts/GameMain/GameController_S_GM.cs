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
        public Text goldNum;
        //public GameObject checkpointObj;
        public AudioClip BGM_Click;

        public CheckPointModel checkPointModel;


        public GameObject startRecodObj; 
        public GameObject stopRecodObj;

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
            goldNum.text = ItemManager.Instance.GetItemCount(PropFunType.GOLD).ToString();
            UpdataRecordStatue();
        }
        public void ShowCheckpoint()
        {
            Loading.Show();
            CheckPointModel.GetPointData(1, 20, (result) =>
            {
                Loading.Hide();
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
            RankingData.Instance.GetSDKRankData();
        }
        public void OnStarkGameRecorder()
        {
            if (!GameRecorderController.Instance.isrecoding)
            {
                GameRecorderController.Instance.StartRecording();
            }
            UpdataRecordStatue();
        }
        public void OnStopGameRecorder()
        {
            if (GameRecorderController.Instance.isrecoding)
            {
                GameRecorderController.Instance.StopRecording();
            }
            UpdataRecordStatue();
        }
        public void OnShareGameRecorder()
        {
            GameRecorderController.Instance.ShareRecord();
        }
        public void EXstart()
        {
            SceneManager.LoadScene("GameScence");
        }


        public void UpdataRecordStatue()
        {
            if (GameRecorderController.Instance.isrecoding)
            {
                startRecodObj.SetActive(false);
                stopRecodObj.SetActive(true);
            }
            else
            {
                startRecodObj.SetActive(true);
                stopRecodObj.SetActive(false);

            }
        }

    }
}