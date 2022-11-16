using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class DeckController : MonoBehaviour
{
    public float cardWidth = 204.6f;
    public float cardHeight = 220.8f;
    public GameObject cardGo;
    private int row = 7;
    private int column = 7;
    private int layer = 8;
    private List<Card> cards = new List<Card>();
    public RectTransform deckTrans;//卡牌需要生成到的位置的transform引用
    public Transform[] pickDeckPosTrans;//捡牌堆的格子位置
    public RectTransform centerDeckTrans;//中间牌堆的基础位置（原点）
    private int[,,] centerDeck = new int[,,]//层 行 列
{
        //后5层
        {
            {0,0,0,0},
            {0,2,2,2},
            {0,2,2,2},
            {0,2,2,2},
            {0,2,2,2},
            {0,2,2,2},
            {0,0,0,0}
        },
        {
            {0,0,0,0},
            {0,2,2,2},
            {0,2,2,2},
            {0,2,2,2},
            {0,2,2,2},
            {0,2,2,2},
            {0,0,0,0}
        },
        {
            {0,0,0,0},
            {0,2,2,2},
            {0,2,2,2},
            {0,2,2,2},
            {0,2,2,2},
            {0,2,2,2},
            {0,0,0,0}
        },
        {
            {0,0,0,0},
            {0,2,2,2},
            {0,2,2,2},
            {0,2,2,2},
            {0,2,2,2},
            {0,2,2,2},
            {0,0,0,0}
        },
        {
            {0,0,0,0},
            {0,2,2,2},
            {0,2,2,2},
            {0,2,2,2},
            {0,2,2,2},
            {0,2,2,2},
            {0,0,0,0}
        },
        //上两层
        {
            {0,0,0,0},
            {0,1,1,1},
            {0,0,1,1},
            {0,0,1,1},
            {0,1,2,2},
            {0,2,2,1},
            {0,0,0,0}
        },
        {
            {0,0,0,0},
            {0,1,1,3},
            {0,0,2,2},
            {3,3,3,3},
            {0,0,0,3},
            {0,0,3,3},
            {0,0,0,0}
        },
        //最上层
        {
            {0,3,1,2},
            {0,0,0,0},
            {3,2,2,2},
            {3,3,3,3},
            {0,0,2,2},
            {0,0,0,0},
            {0,0,1,3}
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
        //遍历层
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
                CREATESTATE[] halfState = new CREATESTATE[column / 2];
                //列
                for (int i = 0; i < column; i++)
                {


                    GameObject go = null;

                    CREATESTATE cs ;


                    if (i <= column / 2)
                    {
                        //前半部分直接从三维数组取
                        cs = (CREATESTATE)centerDeck[k, j, i];
                        if (i != column / 2)
                        {
                            halfState[column / 2 - i - 1] = cs;
                        }
                    }
                    else
                    {
                        cs = halfState[i - column / 2 - 1];
                    }

                    switch (cs)
                    {
                        case CREATESTATE.NONE:
                            break;
                        case CREATESTATE.CREATE:
                            go = CreatCardGo(i, j, dirX, dirY);
                            break;
                        case CREATESTATE.RANDOM:
                            if (UnityEngine.Random.Range(0,2)==0?true:false)
                            {
                                //随机生成
                                go = CreatCardGo(i, j, dirX, dirY);
                            }
                            break;
                        case CREATESTATE.ONLYCREATE:
                            go = CreatCardGo(i, j, 0, 0);
                            break;
                        default:
                            break;
                    }
                    if (go)
                    {
                        Card card =  go.GetComponent<Card>();
                        card.SetCardSprite();
                        //设置覆盖关系

                        SetCoverState(card);

                        cards.Add(card);

                        go.name = "I:" + i.ToString() + " J:" + j.ToString() + " K:" + k.ToString();
                    }

                  
                }
            }
        }

    }
    /// <summary>
    /// 产生卡牌游戏物体
    /// </summary>
    private GameObject CreatCardGo(int column,int row,int dirX,int dirY)
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

    // Update is called once per frame
    void Update()
    {

    }
}
/// <summary>
/// 卡牌的生成状态枚举
/// </summary>
public enum CREATESTATE
{
    NONE,//该位置不生成卡牌
    CREATE,//生成并且位置可能偏移
    RANDOM,//可能生成也可能偏移
    ONLYCREATE//生成一定不偏移
}