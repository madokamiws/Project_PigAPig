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

        public PropFunType currentType;
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
            currentType = propFunType;
            if (propFunType == PropFunType.BACKCARD)
            {
                tx_title.text = "撤回道具";
                tx_Instructions.text = "撤回最近的一张牌并把他放回到原位";
            }
            else if(propFunType == PropFunType.REARRANGE)
            {
                tx_title.text = "洗牌道具";
                tx_Instructions.text = "随机打乱未使用的所有牌";
            }
            else if(propFunType == PropFunType.BACKTHREE)
            {
                tx_title.text = "移出道具";
                tx_Instructions.text = "移出三张牌并且堆放到旁边";
            }
            else if(propFunType == PropFunType.RELIFE)
            {
                tx_title.text = "复活道具";
                tx_Instructions.text = "复活并撤回三张牌到原位";
            }
        }

        public void OnWatchProp()
        {
            AdController.Instance.ShowRewardVideoAd((isWatchedTimeGreater) =>
            {
                if (currentType != null && isWatchedTimeGreater)
                {
                    if (currentType < PropFunType.RELIFE)
                    {
                        ItemManager.Instance.AddItem(currentType);
                        DeckController.Get.UpdatePropNum();
                    }
                    AdController.Instance.SubmitADData(1, 1, null, (result) =>
                    {

                        Destroy(gameObject);
                    });
                }
                else
                {
                    AdController.Instance.SubmitADData(1, 2, "观看广告时间不足", (result) =>
                    {
                        Toast.Show("观看广告失败，没有获得奖励");
                    });

                }

            });
        }
        public override void OnClose()
        {
            Destroy(gameObject);
        }
    }

}
