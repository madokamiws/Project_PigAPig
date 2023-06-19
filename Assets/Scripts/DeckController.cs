using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
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

        public Transform tf_CenterDeckList;

        public int[] pickDeckCardIDs;//存放当前选中卡牌堆里的卡牌ID（跟当前位置一一对应）
        private int createCardNum = 0;


        public Stack<CardMoveRecord> cardMoveHistory = new Stack<CardMoveRecord>();

        private const string LevelIDKey = "CurrentLevelID";
        /// <summary>
        /// 卡牌总数
        /// </summary>
        private int totalCardNum = 42;


        //private int[,,] centerDeck = new int[,,]//层 行 列

        //private int[,,] centerCardIndex = new int[,,]//层 行 列

        public static DeckController Instance { get; private set; }
        public static DeckController Get
        {
            get
            {
                if (!Instance)
                {
                    Instance = new DeckController();
                }
                return Instance;
            }
        }
        private void Awake()
        {
            Instance = this;
        }
        void AdjustCenterDeckPosition()
        {

            int col_offset = column > 7 ? 7 : column;

            int row_offset = row > 8 ? 7 : row;

            float offsetX = (7 - col_offset) * cardWidth * 0.5f;
            float offsetY = (8 - row_offset) * cardHeight * 1f;

    // 在现有位置上进行微调
    Vector2 currentCenterDeckPosition = centerDeckTrans.anchoredPosition;
    Vector2 adjustedCenterDeckPosition = currentCenterDeckPosition + new Vector2(offsetX, -offsetY);
    centerDeckTrans.anchoredPosition = adjustedCenterDeckPosition;
        }
        // Start is called before the first frame update
        public static int GetCurrentLevelID()
        {
            return PlayerPrefs.GetInt(LevelIDKey);
        }
        //public void 
        void Start()
        {
#if UNITY_EDITOR
        int totalCardNum = 15;


        int[,,] centerDeck = new int[,,]//层 行 列
    {
                {
                    {3,3,3},
                    {3,0,3},
                    {3,3,3},
                },
                {
                    {3,3,3},
                    {3,0,0},
                    {3,3,3},
                }
    };

            int[] DeckElementlist = new int[]
                {1,2,3};


    //        int[,,] centerCardIndex = new int[,,]//层 行 列
    //{
    //            {
    //                {1,2,3},
    //                {4,5,6},
    //                {7,8,9},
    //            },
    //            {
    //                {1,2,3},
    //                {4,5,6},
    //                {7,8,9}
    //            }
    //};
            
            DisplayPointData(centerDeck, DeckElementlist, totalCardNum);



#else
            int currentId = GetCurrentLevelID();
            if (currentId > 0)
            {
                DeckData.GetDeckData(currentId, (result) =>
                {
                    ErrorLogs.Get.DisplayLog("currentId"+ currentId);

                    var centerDeck = result.center_deck;
                    ErrorLogs.Get.DisplayLog("centerDeck" + centerDeck);

                    var deckElementlist = result.deckElementlist;
                    ErrorLogs.Get.DisplayLog("deckElementlist" + deckElementlist);

                    var centerCardIndex = result.center_card_index;
                    ErrorLogs.Get.DisplayLog("centerCardIndex" + centerCardIndex);


                    totalCardNum = result.card_total;
                    ErrorLogs.Get.DisplayLog("totalCardNum" + totalCardNum);

                    DisplayPointData(centerDeck, deckElementlist, totalCardNum, centerCardIndex);
                });
            }
#endif

        }
        public void DisplayPointData(int[,,] centerDeck,int[] deckElementlist ,int totalCardNum, int[,,] centerCardIndex = null)
        {

            layer = centerDeck.GetLength(0);
            row = centerDeck.GetLength(1); //行
            column = centerDeck.GetLength(2);//列

            List<int> temp_centerDecklist= new List<int>();

            if (centerCardIndex == null )
            {
                int nonZeroCount = 0;
                foreach (int item in centerDeck)
                {
                    if (item != 0)
                    {
                        nonZeroCount++;
                    }
                }
                if (totalCardNum % 3 != 0)
                {
                    ErrorLogs.Get.DisplayLog("卡牌总数不为3的倍数,请检查配置");
                    return;
                }
                if (nonZeroCount != totalCardNum)
                {
                    ErrorLogs.Get.DisplayLog("配置totalCardNum与centerDeck配置数量不符，请检查配置");
                    return;
                }


                for (int i = 0; i < totalCardNum; i += 3)
                {
                    int index = UnityEngine.Random.Range(0, deckElementlist.Length);
                    temp_centerDecklist.Add(deckElementlist[index]);
                    temp_centerDecklist.Add(deckElementlist[index]);
                    temp_centerDecklist.Add(deckElementlist[index]);
                }
                Shuffle(temp_centerDecklist);


            }


            AdjustCenterDeckPosition();
            pickDeckCardIDs = new int[7] { -1, -1, -1, -1, -1, -1, -1, };
            int temp_centerDecklist_index = 0;
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
                            if (centerCardIndex != null)
                            {
                                card.SetCardSprite(centerCardIndex[k, j, i]);
                            }
                            else
                            {
                                card.SetCardSprite(temp_centerDecklist[temp_centerDecklist_index]);
                                temp_centerDecklist_index++;
                            }


                            //设置覆盖关系
                            SetCoverState(card);

                            cards.Add(card);
                            createCardNum++;
                            card.row = j;
                            card.column = i;
                            card.layer = k;
                            card.cardDir = (int)cs;
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
            go.transform.SetParent(tf_CenterDeckList);
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
                if (card!= cards[i])
                {
                    card.SetCoverCardState(cards[i]);
                }
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
                    return pickDeckPosTrans[posID];
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
            //Invoke("SortGridPos", 0.25f);
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
                    int countToFinish = 3;
                    for (int j = startIndex; j < startIndex + 3; j++)
                    {
                        Transform child = pickDeckPosTrans[j].GetChild(0);
                        pickDeckCardIDs[j] = -1;
                        child.DOScale(Vector3.zero, 0.25f).OnComplete(() =>
                        {
                                Destroy(child.gameObject);
                                countToFinish--;
                                if (countToFinish == 0)
                                {
                                    StartCoroutine(DestroyAndSort(child.gameObject));
                                }
                        });
                    }
                    break;
                }
            }
        }
        /// <summary>
        /// 撤回一步功能
        /// </summary>
        public void OnBackSelection()
        {
            while (cardMoveHistory.Count > 0)
            {
                CardMoveRecord lastMove = cardMoveHistory.Pop();
                if (lastMove.CardTransform == null)
                {
                    // CardTransform 已被销毁，继续出栈下一个记录
                    continue;
                }

                Transform tf_lastCard = lastMove.CardTransform;
                tf_lastCard.SetParent(deckTrans);
                if (lastMove.tranIdIndex >= 0)
                {
                    pickDeckCardIDs[lastMove.tranIdIndex] = -1;
                }
                else
                {
                    for (int i = 0; i < pickDeckCardIDs.Length; i++)
                    {
                        if (pickDeckCardIDs[i]<0 && i-1>=0)
                        {
                            pickDeckCardIDs[i - 1] = -1;
                        }    
                    }     
                }

                // 将卡牌移回原来的位置
                tf_lastCard.DOMove(lastMove.OriginalPos, 0.5f).OnComplete(()=> {
                    if (tf_lastCard != null)
                    {
                        Card card = tf_lastCard.GetComponent<Card>();
                        card.Btn_AddListnener();
                        SortGridPos();
                        // 设置覆盖关系
                        SetCoverState(card);
                        if (!cards.Contains(card))
                        {
                            cards.Add(card);
                        }

                    }
                    else
                    {
                        Debug.Log("CardTransform has been destroyed.");
                    }
                });
                break;
            }
            if (cardMoveHistory.Count == 0)
            {
                Debug.Log("No more moves to undo.");
            }
        }
        /// <summary>
        /// 洗牌算法
        /// </summary>
        /// <param name="list"></param>
        public void Shuffle(List<int> list)
        {
            System.Random rand = new System.Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rand.Next(n + 1);
                int value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
        /// <summary>
        /// 打乱重新生成的数组
        /// </summary>
        public void ShuffleCenterDeckAndIndex(int[,,] centerDeck, int[,,] centerCardIndex, int totalCardNum)
        {
            List<(int deck, int index)> list = new List<(int deck, int index)>();

            for (int i = 0; i < layer; i++)
            {
                for (int j = 0; j < row; j++)
                {
                    for (int k = 0; k < column; k++)
                    {
                        list.Add((centerDeck[i, j, k], centerCardIndex[i, j, k]));
                    }
                }
            }

            System.Random rng = new System.Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }

            for (int i = 0; i < layer; i++)
            {
                for (int j = 0; j < row; j++)
                {
                    for (int k = 0; k < column; k++)
                    {
                        var pair = list[i * row * column + j * column + k];
                        centerDeck[i, j, k] = pair.deck;
                        centerCardIndex[i, j, k] = pair.index;
                    }
                }
            }
            //DisplayPointData(centerDeck, centerCardIndex, totalCardNum);

        }

        public void UpdateCenterDeckAndIndex()
        {
            // Get all child objects
            List<Transform> children = new List<Transform>();
            foreach (Transform child in tf_CenterDeckList)
            {
                if (child.GetComponent<Card>() != null) // If the child has Card script attached
                {
                    children.Add(child);
                }
            }

            // Print the number of child objects with Card script
            Debug.Log("Number of cards: " + children.Count);

            // Shuffle the child objects
            for (int i = 0; i < children.Count; i++)
            {
                // Choose a random index
                int randomIndex = UnityEngine.Random.Range(i, children.Count);
                // Swap positions
                children[i].SetSiblingIndex(randomIndex);
                children[randomIndex].SetSiblingIndex(i);
            }
            List<Card> children2 = new List<Card>();
            foreach (Transform child in tf_CenterDeckList)
            {
                if (child.GetComponent<Card>() != null)
                {
                    children2.Add(child.GetComponent<Card>());
                }
            }
            for (int i = 0; i < children2.Count; i++)
            {
                for (int j  = 0; j < children2.Count; j++)
                {
                    if (children2[i] != children2[j])
                    {
                        children2[i].SetCoverCardState(children2[j]);
                    }
                }
            }
        }
        /// <summary>
        /// cards中删掉选中的卡牌
        /// </summary>
        public void DeleteSelectedCard(Card card)
        {
            if (cards.Contains(card))
            {
                cards.Remove(card);
            }
            else
            {
                Debug.Log("Card not found in the list.");
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
        IEnumerator DestroyAndSort(GameObject obj)
        {
            yield return new WaitForEndOfFrame();
            //Destroy(obj);
            SortGridPos();
        }
        public struct CardMoveRecord
        {
            // 记录被移动的卡牌的Transform组件
            public Transform CardTransform;

            public Vector3 OriginalPos;

            public int tranIdIndex;
        }
        public void AddCardToPickDeck(Transform cardTransform, Vector3 originalPos,int posid)
        {
            cardMoveHistory.Push(new CardMoveRecord
            {
                CardTransform = cardTransform,
                OriginalPos = originalPos,
                tranIdIndex = posid,
            });
        }
        public void OnClickBackScence()
        {
            SceneManager.LoadScene("GameMain");
        }

    }
}