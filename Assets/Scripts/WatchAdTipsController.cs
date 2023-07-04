using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Yes.Game.Chicken
{
    public class WatchAdTipsController : BaseDialogController
    {
        public Text tx_title;
        public Text tx_Instructions;
        public static WatchAdTipsController _instance;
        public static WatchAdTipsController Get
        {
            get
            {
                if (_instance == null)
                {
                    string path = "WatchAdTipsController";
                    _instance = (WatchAdTipsController)WatchAdTipsController.InitBase(_instance, path);
                }
                return (WatchAdTipsController)_instance;
            }
        }
        public override void Awake()
        {
            _instance = this;
        }
        public void ShowWatchAdTips(PropFunType propFunType)
        {
            if (propFunType == PropFunType.BACKCARD)
            {
                tx_title.text = "撤回道具";
                tx_Instructions.text = "撤回最近的一张牌并把他放回到原位";
            }
            if (propFunType == PropFunType.REARRANGE)
            {
                tx_title.text = "洗牌道具";
                tx_Instructions.text = "随机打乱未使用的所有牌";
            }
            if (propFunType == PropFunType.BACKTHREE)
            {
                tx_title.text = "移出道具";
                tx_Instructions.text = "移出三张牌并且堆放到旁边";
            }
            if (propFunType == PropFunType.RELIFE)
            {
                tx_title.text = "复活道具";
                tx_Instructions.text = "复活并撤回三张牌到原位";
            }
        }
        public override void OnClose()
        {
            Destroy(gameObject);
        }
    }
    public enum PropFunType
    { 
        BACKCARD,
        REARRANGE,
        BACKTHREE,
        RELIFE
    }
}
