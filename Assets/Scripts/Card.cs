﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
namespace Yes.Game.Chicken
{
    public class Card : MonoBehaviour
    {
        public Button btnCard;
        public Image imgCard;
        public Sprite[] clickSprites;
        public Sprite[] coveredSprites;
        public int id;

        public RectTransform rtf;

        public List<Card> coverCardList = new List<Card>();//当前卡牌覆盖的其他卡牌
        public List<Card> aboveCardList = new List<Card>();//覆盖当前卡牌的其他卡牌

        // Start is called before the first frame update
        void Start()
        {
            btnCard.onClick.AddListener(CardClickEvent);

        }

        // Update is called once per frame
        void Update()
        {

        }
        /// <summary>
        /// 设置卡牌图案 id
        /// </summary>
        public void SetCardSprite()
        {
            id = Random.Range(1, 11);
            imgCard.sprite = clickSprites[id - 1];
            SpriteState ss = btnCard.spriteState;
            ss.disabledSprite = coveredSprites[id - 1];
            btnCard.spriteState = ss;
        }
        /// <summary>
        /// 设置当前卡牌与其他卡牌覆盖关系
        /// </summary>
        /// <param name="targetCard">目标卡牌</param>
        public void SetCoverCardState(Card targetCard)
        {
            //本身卡牌屏幕的坐标点
            Vector2 cardPos = GUIUtility.GUIToScreenPoint(rtf.anchoredPosition);
            //目标卡牌屏幕的坐标点
            Vector2 targetCardPos = GUIUtility.GUIToScreenPoint(targetCard.rtf.anchoredPosition);
            if (Mathf.Abs(cardPos.x - targetCardPos.x) < DeckController.Instance.cardWidth - 0.01 &&
                Mathf.Abs(cardPos.y - targetCardPos.y) < DeckController.Instance.cardHeight - 0.01)
            {
                targetCard.AddAboveCard(this);
                targetCard.ClosebuttonClickState();
                coverCardList.Add(targetCard);
            }
        }
        /// <summary>
        /// 卡牌被覆盖
        /// </summary>
        public void ClosebuttonClickState()
        {
            btnCard.interactable = false;
        }
        /// <summary>
        /// 卡牌点击事件
        /// </summary>
        public void CardClickEvent()
        {
            btnCard.onClick.RemoveAllListeners();
            transform.SetSiblingIndex(500);
            //需要移出所有被覆盖的卡牌
            for (int i = 0; i < coverCardList.Count; i++)
            {
                coverCardList[i].RemoveAboveCard(this);
                coverCardList[i].JudgeCanClickState();
            }
            //Destroy(gameObject);
            int posID = -1;
            Transform targetTrans = DeckController.Instance.GetPickDeckTargetTrans(id, out posID);
            transform.DOMove(targetTrans.position, 0.5f).OnComplete(() =>
            {
                if (targetTrans.childCount > 0)
                {
                    transform.SetParent(DeckController.Instance.GetEmptyPickDeckTargetTrans(posID));
                }
                else
                {
                    transform.SetParent(targetTrans);
                }

                transform.localPosition = Vector3.zero;
                DeckController.Instance.JudgeClearCard();
            });
        }


        /// <summary>
        /// 目标卡牌覆盖了当前卡牌（我们自身去调用目标卡牌的方法）
        /// </summary>
        public void AddAboveCard(Card targetCard)
        {
            aboveCardList.Add(targetCard);
        }
        /// <summary>
        /// 移除当前卡牌被目标卡牌覆盖的引用（我们自身被其他覆盖到我们的卡牌(目标卡牌)去调用）
        /// </summary>
        /// <param name="aboveCard"></param>
        public void RemoveAboveCard(Card aboveCard)
        {
            aboveCardList.Remove(aboveCard);
        }
        /// <summary>
        /// 判断当前是否可以点击
        /// </summary>
        public void JudgeCanClickState()
        {
            btnCard.interactable = aboveCardList.Count <= 0;
        }

    }
}
