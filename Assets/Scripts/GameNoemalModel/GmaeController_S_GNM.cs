using System.Collections;
using System.Collections.Generic;
using StarkSDKSpace;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace Yes.Game.Chicken
{
    public class GmaeController_S_GNM : MonoBehaviour
    {
        //public Button btn_modelNormal;
        // Start is called before the first frame update
        void Start()
        {

            CheckPointModel.GetPointData(1, 20, (result) =>
            {
                CheckPointController.Get.Display(result);
            });
        }
        public void OnClose()
        {
            gameObject.SetActive(false);

        }

    }
}