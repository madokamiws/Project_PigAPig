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
        public RectTransform empPos_BackThree;

        public Transform tf_CenterDeckList;
        public List<Transform> xxx = new List<Transform>();

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
        int totalCardNum = 114;


        int[,,] centerDeck = new int[,,]//层 行 列
    {
                {
            {0,0,0,0,0,0,0},
            {0,4,4,4,4,4,0},
            {0,0,4,4,4,0,0},
            {0,0,0,4,0,0,0},
            {0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0}
        },
        {
            {0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0},
            {0,3,3,3,3,3,0},
            {0,8,8,8,8,8,0},
            {0,0,8,8,8,0,0},
            {0,0,4,0,4,0,0},
            {0,8,0,8,0,8,0},
            {0,0,0,0,0,0,0}
        },
        {
            {0,0,0,0,0,0,0},
            {0,0,7,0,7,0,0},
            {0,0,0,0,0,0,0},
            {0,0,0,7,0,0,0},
            {0,0,7,0,7,0,0},
            {0,0,7,7,7,0,0},
            {0,0,0,7,0,0,0},
            {0,0,0,0,0,0,0}
        },
        {
            {0,0,0,0,0,0,0},
            {0,0,6,0,6,0,0},
            {0,0,0,0,0,0,0},
            {0,0,0,6,0,0,0},
            {0,0,6,0,6,0,0},
            {0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0}
        },
        {
            {0,0,0,0,0,0,0},
            {0,4,0,0,0,4,0},
            {0,0,4,0,4,0,0},
            {0,0,0,4,0,0,0},
            {0,0,4,0,4,0,0},
            {0,4,0,0,0,4,0},
            {0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0}
        },
        {
            {0,0,0,0,0,0,0},
            {0,0,0,3,0,0,0},
            {0,0,0,3,0,0,0},
            {0,0,3,3,3,0,0},
            {0,0,3,3,3,0,0},
            {0,0,0,3,0,0,0},
            {0,0,0,3,0,0,0},
            {0,0,0,0,0,0,0}
        },
        {
            {0,3,8,0,9,3,0,},
            {0,3,8,0,9,3,0,},
            {0,3,8,0,9,3,0,},
            {0,3,0,4,0,3,0,},
            {0,3,0,4,0,3,0,},
            {0,3,8,0,9,3,0,},
            {0,3,8,0,9,3,0,},
            {0,3,8,0,9,3,0,}
        },
        {
            {0,4,0,4,0,3,0},
            {0,3,0,5,0,4,0},
            {0,5,0,5,0,3,0},
            {0,0,3,8,0,6,0},
            {0,4,7,5,4,6,0},
            {0,0,5,6,0,0,0},
            {0,4,0,0,0,5,0},
            {3,0,4,0,8,0,0}

                }
    };

            int[] DeckElementlist = new int[]
                {1,2,3,4,5,6,7,8,9,10};

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
                    ErrorLogs.Get.DisplayLog("currentId:"+ currentId);

                    int[,,] _centerDeck = result.center_deck;
                    //for (int layer = 0; layer < _centerDeck.GetLength(0); layer++)
                    //{
                    //    for (int row = 0; row < _centerDeck.GetLength(1); row++)
                    //    {
                    //        for (int column = 0; column < _centerDeck.GetLength(2); column++)
                    //        {
                    //            int element = _centerDeck[layer, row, column];
                    //            ErrorLogs.Get.DisplayLog(element.ToString(),false);
                    //        }

                    //        ErrorLogs.Get.DisplayLog(""); // 在每行结束后打印换行符
                    //    }

                    //    ErrorLogs.Get.DisplayLog(""); // 在每层结束后打印换行符
                    //}
                    //ErrorLogs.Get.DisplayLog("centerDeck:");

                    int[] _deckElementlist = result.deck_element_list;
                    //ErrorLogs.Get.DisplayLog("deckElementlist:");

                    int[,,] _centerCardIndex = result.center_card_index;





                    totalCardNum = result.card_total;
                    ErrorLogs.Get.DisplayLog("totalCardNum" + totalCardNum);

                    DisplayPointData(_centerDeck, _deckElementlist, totalCardNum, _centerCardIndex);
                });
            }
#endif

        }
        public void DisplayPointData(int[,,] centerDeck,int[] deckElementlist ,int totalCardNum, int[,,] centerCardIndex = null)
        {

            layer = centerDeck.GetLength(0);
            ErrorLogs.Get.DisplayLog("centerDeck_layer" + layer);
            row = centerDeck.GetLength(1); //行
            ErrorLogs.Get.DisplayLog("centerDeck_row" + row);
            column = centerDeck.GetLength(2);//列
            ErrorLogs.Get.DisplayLog("centerDeck_column" + column);

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

                ErrorLogs.Get.DisplayLog("检测没有错误");
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
            ErrorLogs.Get.DisplayLog("AdjustCenterDeckPosition之后");
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
                            //go.transform.SetSiblingIndex(createCardNum);
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
            ErrorLogs.Get.DisplayLog("生成之后");
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
        /// 撤回三张卡牌
        /// </summary>
        public void OnBackThree()
        {
            for (int turn = 0; turn < 3; turn++)
            {
                while (cardMoveHistory.Count > 0)
                {
                    CardMoveRecord lastMove = cardMoveHistory.Pop();
                    if (lastMove.CardTransform == null)
                    {
                        continue;
                    }

                    Transform tf_lastCard = lastMove.CardTransform;
                    tf_lastCard.SetParent(tf_CenterDeckList);
                    if (lastMove.tranIdIndex >= 0)
                    {
                        pickDeckCardIDs[lastMove.tranIdIndex] = -1;
                    }
                    else
                    {
                        for (int i = 0; i < pickDeckCardIDs.Length; i++)
                        {
                            if (pickDeckCardIDs[i] < 0 && i - 1 >= 0)
                            {
                                pickDeckCardIDs[i - 1] = -1;
                            }
                        }
                    }
                    var pos = empPos_BackThree.position + new Vector3(turn * cardWidth,0,0);

        // 将卡牌移到EmpPos_BackThree
        tf_lastCard.DOMove(pos, 0.1f).OnComplete(() => {
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
                tf_lastCard.SetParent(tf_CenterDeckList);
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
                tf_lastCard.DOMove(lastMove.OriginalPos, 0.1f).OnComplete(()=> {
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
        /// 打乱center牌堆
        /// </summary>
        public void UpdateCenterDeckAndIndex()
        {
            cards.Clear();
            List<Transform> children = new List<Transform>();
            List<Vector3> positions = new List<Vector3>();
            foreach (Transform child in tf_CenterDeckList)
            {
                if (child.GetComponent<Card>() != null) // If the child has Card script attached
                {
                    children.Add(child);
                    positions.Add(child.position);
                    child.GetComponent<Card>().CleanCardData();
                    child.gameObject.SetActive(false);
                }
            }
            //Fisher-Yates洗牌算法打乱卡片顺序
            for (int i = children.Count - 1; i > 0; i--)
            {
                int swapIndex = UnityEngine.Random.Range(0, i + 1);
                Transform temp = children[i];
                children[i] = children[swapIndex];
                children[swapIndex] = temp;
            }
            // Swap positions
            for (int i = 0; i < children.Count; i++)
            {
                children[i].gameObject.SetActive(true);
                children[i].SetSiblingIndex(i);
                children[i].position = positions[i];
                var card = children[i].GetComponent<Card>();
                SetCoverState(card);
                cards.Add(card);
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

        public void ShowLog()
        {
            ErrorLogs.Get.ShowLog();
            //ErrorLogs.Get.DisplayLog("dasdasdasdasdasdasdasdasddasdhdfkhfjljfldjlakjfldjjfsl3or3980orj3lnrl3knc0q399399cjq c3qc9o3qcero9qec9oquec2queoq2ce9mqceojwlkahjflkjflsjfldjfdlsjflskjfdksjfjowjliamntnhshannzjkuzjm tgyayalliiuzjnmgj;lliujunhsutgnmnqa1010101010ajsjz8988888k2k1k****J1899Z0023834");
        }

    }
}