using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public Button btnCard;
    public Image imgCard;
    public Sprite[] clickSprites;
    public Sprite[] coveredSprites;
    public int id;

    public RectTransform rtf;
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
        id = Random.Range(1, 15);
        imgCard.sprite = clickSprites[id - 1];
        SpriteState ss =  btnCard.spriteState;
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
        if (Mathf.Abs(cardPos.x - targetCardPos.x) < DeckController.Instance.cardWidth-0.01 && 
            Mathf.Abs(cardPos.y - targetCardPos.y)< DeckController.Instance.cardHeight-0.01 )
        {
            targetCard.ClosebuttonClickState();
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
        Destroy(gameObject);
    }
}
