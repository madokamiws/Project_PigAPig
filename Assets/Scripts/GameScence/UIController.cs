using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace Yes.Game.Chicken
{
    public class UIController : SingletonPatternMonoBase<UIController>
    {

        public void OnClickBackScence()
        {
            int id = DeckController.Get.current_user_level_record_id;
            ErrorLogs.Get.DisplayLog("BackShowFailure id = " + id);
            Loading.Show();
            GameFinishData.SubmitLevelData(id, 0, (result) =>
            {
                Loading.Hide();
                ErrorLogs.Get.DisplayLog("SubmitLevelData  ShowFailure  成功回调");
                if (result != null)
                {
                    if (result.level_id > 0)
                    {
                        PlayerPrefs.SetInt("CurrentLevelIDMax", result.level_id);
                        PlayerPrefs.Save();
                    }
                    SceneManager.LoadScene("GameMain");
                }
            });

        }
        public void OnRankingList()
        {

        }
        // Start is called before the first frame update
        void Start() 
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}