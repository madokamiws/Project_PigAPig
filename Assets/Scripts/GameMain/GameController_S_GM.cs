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
        // Start is called before the first frame update
        void Start()
        {
            //btn_modelNormal.onClick.AddListener(OnClickModelNormal);


        }
        public void ShowCheckpoint()
        {
            CheckPointModel.GetPointData(1, 20, (result) =>
            {
                CheckPointController.Get.Display(result);
            });
        }
        public void OnClickModelNormal()
        {
            //SceneManager.LoadScene("GameNormalModel");
        }
    }
}