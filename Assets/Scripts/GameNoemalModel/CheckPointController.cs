using System.Collections;
using System.Collections.Generic;
using StarkSDKSpace;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace Yes.Game.Chicken
{

    public class CheckPointController : BaseDialogController
    {
        public Transform tf_contentMain;
        public GameObject itemViewPointPrefab;
        public static CheckPointController _instance;
        public static CheckPointController Get
        {
            get
            {
                if (_instance == null)
                {
                    string path = "CheckPointController";
                    _instance = (CheckPointController)CheckPointController.InitBase(_instance, path);
                }
                return (CheckPointController)_instance;
            }
        }
        public void Display(CheckPointModel model)
        {
            BaseDialogController.DestoryChilds(tf_contentMain);
            if (model!=null)
            {
                for (int i = 0; i < model.config_levels.Count; i++)
                {
                    CheckPoint point = model.config_levels[i];

                    GameObject item = Instantiate(itemViewPointPrefab) as GameObject;
                    item.transform.SetParent(tf_contentMain);
                    item.transform.localPosition = Vector3.zero;
                    item.transform.localScale = new Vector3(1, 1, 1);
                    item.SetActive(true);
                    CheckPointItem itemView = item.GetComponent<CheckPointItem>();
                    if (itemView)
                    {
                        //itemView.LoadData(point, i);
                    }

                }
            }


        }

        public override void Awake()
        {
            _instance = this;
        }
    }
}