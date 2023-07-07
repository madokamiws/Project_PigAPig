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
            SceneManager.LoadScene("GameMain");
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