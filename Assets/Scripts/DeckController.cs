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
                //列
                for (int i = 0; i < column; i++)
                {
                    //随机当前列是否偏移
                    bool ifMoveX =  Convert.ToBoolean(UnityEngine.Random.Range(0, 2));
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


                    GameObject go = Instantiate(cardGo, deckTrans);
                    go.GetComponent<RectTransform>().anchoredPosition = 
                        centerDeckTrans.anchoredPosition + 
                        new Vector2(cardWidth * (i+0.5f*dirX), -cardHeight * (j + 0.5f * dirY));
                    go.name = "I:" + i.ToString() + " J:" + j.ToString() + " K:" + k.ToString();
                }
            }
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
