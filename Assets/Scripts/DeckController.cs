using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
namespace Yes.Game.Chicken
{
    public class DeckController : MonoBehaviour
    {
        public float cardWidth = 204.6f;
        public float cardHeight = 220.8f;
        public GameObject cardGo;
        private int row;
        private int column;
        private int layer;
        private List<Card> cards = new List<Card>();
        public RectTransform deckTrans;//卡牌需要生成到的位置的transform引用
        public Transform[] pickDeckPosTrans;//捡牌堆的格子位置

        public RectTransform centerDeckTrans;//中间牌堆的基础位置（原点）
        public RectTransform leftColumnDeckTrans;
        public RectTransform rightColumnDeckTrans;
        public RectTransform leftDownDeckTrans;
        public RectTransform rightDownDeckTrans;

        public int[] pickDeckCardIDs;//存放当前选中卡牌堆里的卡牌ID（跟当前位置一一对应）
        private int createCardNum = 0;

        /// <summary>
        /// 卡牌总数
        /// </summary>
        private int totalCardNum = 15;


        private int[,,] centerDeck = new int[,,]//层 行 列
    {
        {
            {3,3,3,3},
            {3,0,3,3}
        },
        {
            {3,3,3,3},
            {3,3,3,3}
        }
    };
        private int[,,] centerCardIndex = new int[,,]//层 行 列
{
        {
            {1,1,1,2},
            {2,0,2,3}
        },
        {
            {3,3,4,4},
            {4,1,1,1}
        }
};
        public static DeckController Instance { get; set; }
        private void Awake()
        {
            Instance = this;
        }
        // Start is called before the first frame update
        void Start()
        {
            DeckData.GetDeckData((result)=> 
            {
            
            
            });

            pickDeckCardIDs = new int[7] { -1, -1, -1, -1, -1, -1, -1, };

            layer = centerDeck.GetLength(0);
            row = centerDeck.GetLength(1);
            column = centerDeck.GetLength(2);

            //遍历层--------------中间组
            for (int k = 0; k < layer; k++)
            {
                //行
                for (int j = 0; j < row; j++)
                {
                    //随机当前列是否偏移
                    bool ifMoveX = Convert.ToBoolean(UnityEngine.Random.Range(0, 2));
                    bool ifMoveY = Convert.ToBoolean(UnityEngine.Random.Range(0, 2));

                    int dirX = 0;
                    int dirY = 0;

                    if (ifMoveX)
                    {
                        //偏移方向是做还是右
                        dirX = UnityEngine.Random.Range(0, 2) == 0 ? 1 : -1;
                    }
                    if (ifMoveY)
                    {
                        //偏移方向上下
                        dirY = UnityEngine.Random.Range(0, 2) == 0 ? 1 : -1;
                    }
                    //对称生成后半部分
                    //CREATESTATE[] halfState = new CREATESTATE[column / 2];
                    //列
                    for (int i = 0; i < column; i++)
                    {


                        GameObject go = null;

                        CREATESTATE cs;
                        cs = (CREATESTATE)centerDeck[k, j, i];

                        //if (i <= column / 2)
                        //{
                        //    //前半部分直接从三维数组取
                        //    cs = (CREATESTATE)centerDeck[k, j, i];
                        //    if (i != column / 2)
                        //    {
                        //        halfState[column / 2 - i - 1] = cs;
                        //    }
                        //}
                        //else
                        //{
                        //    cs = halfState[i - column / 2 - 1];
                        //}

                        switch (cs)
                        {
                            case CREATESTATE.NONE:
                                break;
                            case CREATESTATE.CREATE:
                                go = CreatCardGo(i, j, dirX, dirY);
                                break;
                            case CREATESTATE.RANDOM:
                                if (UnityEngine.Random.Range(0, 2) == 0 ? true : false)
                                {
                                    //随机生成
                                    go = CreatCardGo(i, j, dirX, dirY);
                                }
                                break;
                            case CREATESTATE.ONLYCREATE:
                                go = CreatCardGo(i, j, 0, 0);
                                break;
                            case CREATESTATE.UPPERCREATE:
                                go = CreatCardGo(i, j, 0, -1);
                                break;//上
                            case CREATESTATE.LOWERCREATE:
                                go = CreatCardGo(i, j, 0, 1);
                                break;//下
                            case CREATESTATE.LEFTCREATE:
                                go = CreatCardGo(i, j, -1, 0);
                                break;//左
                            case CREATESTATE.RIGHTCREATE:
                                go = CreatCardGo(i, j, 1, 0);
                                break;//右
                            case CREATESTATE.UPPERLEFTCREATE:
                                go = CreatCardGo(i, j, -1, -1);
                                break;//左上
                            case CREATESTATE.UPPERRIGHTCREATE:
                                go = CreatCardGo(i, j, 1, -1);
                                break;//右上
                            case CREATESTATE.LOWERRLEFTCREATE:
                                go = CreatCardGo(i, j, -1, 1);
                                break;//左下
                            case CREATESTATE.LOWERRIGHTCREATE:
                                go = CreatCardGo(i, j, 1, 1);
                                break;//右下

                            default:
                                break;
                        }
                        if (go)
                        {
                            Card card = go.GetComponent<Card>();
                            //card.SetCardSprite();
                            card.SetCardSprite(centerCardIndex[k, j, i]);
                            
                            //设置覆盖关系
                            SetCoverState(card);

                            cards.Add(card);
                            createCardNum++;
                            go.name = "I:" + i.ToString() + " J:" + j.ToString() + " K:" + k.ToString();
                        }
                    }
                }
            }
            int createGroupNum = (totalCardNum - createCardNum) / 4;
            int redundantNum = totalCardNum - createCardNum - createGroupNum * 4;
            //左竖
            for (int i = createGroupNum + redundantNum; i > 0; i--)
            {
                CreatCard(leftColumnDeckTrans, 0, -i);
            }
            //右竖
            for (int i = createGroupNum; i > 0; i--)
            {
                CreatCard(rightColumnDeckTrans, 0, -i);
            }
            //左下
            for (int i = 0; i < createGroupNum; i++)
            {
                CreatCard(leftDownDeckTrans, i, 0);
            }
            //右下
            for (int i = createGroupNum; i > 0; i--)
            {
                CreatCard(rightDownDeckTrans, i, 0);
            }

        }
        /// <summary>
        /// 旁边卡碟
        /// </summary>
        /// <param name="zeroTrans"></param>
        /// <param name="indexX"></param>
        /// <param name="indexY"></param>
        private void CreatCard(RectTransform zeroTrans, int indexX, int indexY)
        {
            GameObject go = Instantiate(cardGo, deckTrans);
            RectTransform rft = go.GetComponent<RectTransform>();
            rft.anchoredPosition = zeroTrans.anchoredPosition + new Vector2(cardWidth * indexX * 0.15f, cardHeight * indexY * 0.15f);
            Card card = go.GetComponent<Card>();
            card.SetCardSprite();
            SetCoverState(card);
            cards.Add(card);

        }
        /// <summary>
        /// 产生卡牌游戏物体
        /// </summary>
        private GameObject CreatCardGo(int column, int row, int dirX, int dirY)
        {
            GameObject go = Instantiate(cardGo, deckTrans);
            go.GetComponent<RectTransform>().anchoredPosition =
                centerDeckTrans.anchoredPosition +
                new Vector2(cardWidth * (column + 0.5f * dirX), -cardHeight * (row + 0.5f * dirY));
            return go;
        }
        /// <summary>
        /// 当前新生成的卡牌与其他卡牌之间的关系
        /// </summary>
        /// <param name="card"></param>
        private void SetCoverState(Card card)
        {
            for (int i = 0; i < cards.Count; i++)
            {
                card.SetCoverCardState(cards[i]);
            }
        }
        /// <summary>
        /// 获取当前捡牌堆的目标位置
        /// </summary>
        /// <param name="currentID">当前新选中的牌</param>
        /// <param name="posID">格子位置ID索引</param>
        /// <returns></returns>
        public Transform GetPickDeckTargetTrans(int currentID, out int posID)
        {
            posID = -1;
            for (int i = 0; i < pickDeckCardIDs.Length; i++)
            {
                //判断选中卡牌有没有与新选中的卡牌id相等
                if (pickDeckCardIDs[i] == currentID && i + 1 <= pickDeckCardIDs.Length)
                {
                    posID = i + 1;
                    return pickDeckPosTrans[i + 1];
                }
            }
            Transform sf = GetEmptyPickDeckTargetTrans(posID);
            if (sf)
            {
                return sf;
            }
            Debug.Log("游戏结束");
            return null;
        }
        /// <summary>
        /// 获取选中卡牌堆位置
        /// </summary>
        /// <param name="posID">选中卡牌堆的格子位置索引id</param>
        /// <returns></returns>
        public Transform GetEmptyPickDeckTargetTrans(int posID = -1)
        {
            for (int i = 0; i < pickDeckPosTrans.Length; i++)
            {
                if (pickDeckCardIDs[i] == -1)
                {
                    pickDeckCardIDs[i] = posID;

                    if (posID != -1)
                    {
                        pickDeckPosTrans[i].SetSiblingIndex(posID);
                    }
                    return pickDeckPosTrans[i];
                }
            }
            Debug.Log("游戏结束");
            return null;
        }
        /// <summary>
        /// 排序卡牌位置和ID
        /// </summary>
        public void SortCardAndCardID()
        {
            SortCard();
            SortID();
        }
        /// <summary>
        /// 卡牌排序
        /// </summary>
        public void SortCard()
        {
            Transform[] tempTrans = new Transform[pickDeckPosTrans.Length];
            for (int i = 0; i < pickDeckPosTrans.Length; i++)
            {
                int siblingIndex = pickDeckPosTrans[i].GetSiblingIndex();
                tempTrans[siblingIndex] = pickDeckPosTrans[i];
            }
            for (int i = 0; i < pickDeckPosTrans.Length; i++)
            {
                pickDeckPosTrans[i] = tempTrans[i];
            }
        }

        /// <summary>
        /// ID排序
        /// </summary>
        public void SortID()
        {
            for (int i = 0; i < pickDeckCardIDs.Length; i++)
            {
                if (pickDeckPosTrans[i].childCount > 0)
                {
                    pickDeckCardIDs[i] = pickDeckPosTrans[i].GetChild(0).GetComponent<Card>().id;
                }
                else
                {
                    pickDeckCardIDs[i] = -1;
                }
            }
        }
        /// <summary>
        /// 卡牌消除判定方法
        /// </summary>
        public void JudgeClearCard()
        {
            SortCardAndCardID();
            ClearCards();
            Invoke("SortGridPos", 0.25f);
        }
        /// <summary>
        /// 三消算法
        /// </summary>
        public void ClearCards()
        {
            int sameCount = 0;
            int judgeID = -1;
            int startIndex = -1;
            for (int i = 0; i < pickDeckCardIDs.Length; i++)
            {
                //遍历到空格子直接结束
                if (pickDeckCardIDs[i] == -1)
                {
                    break;
                }
                if (sameCount == 0)
                {
                    sameCount++;
                    judgeID = pickDeckCardIDs[i];
                    startIndex = i;
                }
                else
                {
                    if (judgeID == pickDeckCardIDs[i])
                    {
                        sameCount++;
                    }
                    else
                    {
                        sameCount = 1;
                        judgeID = pickDeckCardIDs[i];
                        startIndex = i;
                    }
                }
                if (sameCount >= 3)
                {
                    for (int j = startIndex; j < startIndex + 3; j++)
                    {
                        pickDeckCardIDs[j] = -1;
                        Destroy(pickDeckPosTrans[j].GetChild(0).gameObject);
                    }
                    break;
                }
            }
        }
        /// <summary>
        /// 排序消除后的格子位置
        /// </summary>
        public void SortGridPos()
        {
            for (int i = 0; i < pickDeckPosTrans.Length; i++)
            {
                if (pickDeckPosTrans[i].childCount <= 0)
                {
                    pickDeckPosTrans[i].SetSiblingIndex(6);
                }
            }
            SortCardAndCardID();
        }

    }
}