using System.Collections;
using System.Collections.Generic;
using StarkSDKSpace;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace Yes.Game.Chicken
{
    public class GameStart : SingletonPatternMonoBase<GameStart>
    {
        public Button startBtn;

        private AsyncOperation ao;
        //public bool ifLoadGameScene;

        void Start()
        {
            //开启关注抖音号API的MOCK
            //StarkSDKSpace.MockSetting.SwithMockModule(StarkSDKSpace.MockModule.FollowDouyin, true);

            //调用API时会弹出调试框
            //StarkSDK.API.FollowDouYinUserProfile(OnFollowCallback, OnFollowError);

            //startBtn.onClick.AddListener(StartGame(false));

            ao = SceneManager.LoadSceneAsync(1);
            ao.allowSceneActivation = false;
        }

        void OnFollowCallback()
        { 
        
        }
        void OnFollowError(int num1 ,string str_2)
        { 
        
        }

        public void StartGame(bool isneedverify = false)
        {
            if (isneedverify)
            {
                LoginController.Instance.OnSeverlogin((result) =>
                {
                    if (result)
                    {
                        ao.allowSceneActivation = true;
                    }

                });
            }
            else
            {
                ao.allowSceneActivation = true;
            }

            //else
            //{
            //    Destroy(gameObject);
            //}
        }
    }
}