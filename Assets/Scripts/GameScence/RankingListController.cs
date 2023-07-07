using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Yes.Game.Chicken
{
    public class RankingListController : BaseDialogController
    {
        public static RankingListController _instance;
        public static RankingListController Instance
        {
            get
            {
                if (_instance == null)
                {
                    string path = "RankingListController";
                    _instance = (RankingListController)RankingListController.InitBase(_instance, path);
                }
                return (RankingListController)_instance;
            }
        }
        public override void Awake()
        {
            _instance = this;
        }
        public void ShowRanking()
        { 
        
        }

    }
}