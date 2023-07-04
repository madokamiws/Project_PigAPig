﻿using System.Collections;
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
            int id = DeckController.Get.currentLevelID;
            GameFinshData.SubmitLevelData(id, 0, (result) =>
            {

            });
            successObj.SetActive(false);
            failureObj.SetActive(true);
        }
        public void ShowSuccess()
        {
            int id = DeckController.Get.currentLevelID;
            GameFinshData.SubmitLevelData(id, 1, (result) =>
              {
                  if (result != null)
                  {
                      if (result.level_id > 0)
                      {
                          PlayerPrefs.SetInt("CurrentLevelIDMax", result.level_id);
                          PlayerPrefs.Save();
                      }
                  }
              });
            successObj.SetActive(true);
            failureObj.SetActive(false);
        }
        public void OnBackScence()
        {
            DeckController.Get.OnClickBackScence();
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
                    DeckController.Get.OnBackThree();
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
            DeckController.Get.InitCreatDeck();
        }

        public override void OnClose()
        {
            Destroy(gameObject);
        }
    }
}