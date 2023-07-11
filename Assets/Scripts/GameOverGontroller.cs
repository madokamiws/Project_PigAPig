using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace Yes.Game.Chicken
{
    public class GameOverGontroller : BaseDialogController
    {
        public GameObject successObj;
        public GameObject failureObj;
        public AudioClip sound_success;
        public AudioClip sound_failure;
        public static GameOverGontroller _instance;
        public static GameOverGontroller Get
        {
            get
            {
                if (_instance == null)
                {
                    string path = "GameOverGontroller";
                    _instance = (GameOverGontroller)GameOverGontroller.InitBase(_instance, path);
                }
                return (GameOverGontroller)_instance;
            }
        }
        public override void Awake()
        {
            _instance = this;
        }
        public void ShowFailure()
        {
            int id = DeckController.Get.current_user_level_record_id;
            ErrorLogs.Get.DisplayLog("ShowFailure id = "+  id);
            AudioManager.Instance.PlaySound(sound_failure);
            GameFinishData.SubmitLevelData(id, 0, (result) =>
            {
                ErrorLogs.Get.DisplayLog("SubmitLevelData  ShowFailure  成功回调");
                if (result != null)
                {
                    if (result.level_id > 0)
                    {
                        PlayerPrefs.SetInt("CurrentLevelIDMax", result.level_id);
                        PlayerPrefs.Save();
                    }
                }
            });
            successObj.SetActive(false);
            failureObj.SetActive(true);
        }
        public void ShowSuccess()
        {
            int id = DeckController.Get.current_user_level_record_id;
            ErrorLogs.Get.DisplayLog("ShowSuccess id = " + id);
            AudioManager.Instance.PlaySound(sound_success);
            GameFinishData.SubmitLevelData(id, 1, (result) =>
              {
                  ErrorLogs.Get.DisplayLog("SubmitLevelData  成功回调");
                  if (result != null)
                  {
                      ItemManager.Instance.SetItem(PropFunType.GOLD, result.current_golds);
                      if (result.level_id > 0)
                      {
                          ErrorLogs.Get.DisplayLog("result.level_id  =" + result.level_id);
                          PlayerPrefs.SetInt("CurrentLevelIDMax", result.level_id);
                          PlayerPrefs.Save();
                      }
                      try { RankingData.Instance.SetSDKRankData(result.total_golds); }
                      catch (Exception ex)
                      {
                          ErrorLogs.Get.DisplayLog("SetSDKRankData报错:" + ex.Message);
                      }
                  }
              });
            successObj.SetActive(true);
            failureObj.SetActive(false);
        }
        public void OnBackScence()
        {
            UIController.Instance.OnClickBackScence();
        }
        public void OnReStart()
        {
            DeckController.Get.InitCreatDeck(DeckController.Get.currentLevelID);
            OnClose();
        }
        public void OnReLife()
        {
            AdController.Instance.ShowRewardVideoAd((isWatchedTimeGreater) =>
            {
                if (isWatchedTimeGreater)
                {
                    DeckController.Get.OnBackThree(true);
                    OnClose();
                    ErrorLogs.Get.DisplayLog("GameOverGontroller中激励广告: watchedTime 大于 effectiveTime");
                }
                else
                {
                    // watchedTime 小于等于 effectiveTime 的处理逻辑
                    ErrorLogs.Get.DisplayLog("GameOverGontroller中激励广告: watchedTime 小于等于 effectiveTime");
                }
            });
        }
        public void OnNextPoint()
        {
             int level = PlayerPrefs.GetInt("CurrentLevelIDMax");
            if (level >= 0)
            {
                DeckController.Get.InitCreatDeck(level);
                OnClose();
            }
            else
            {
                UIController.Instance.OnClickBackScence();
            }

        }

        public override void OnClose()
        {
            Destroy(gameObject);
        }
    }
}