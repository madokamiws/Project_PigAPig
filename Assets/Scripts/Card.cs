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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetCardSprite()
    {
        id = Random.Range(1, 15);
        imgCard.sprite = clickSprites[id - 1];
        SpriteState ss =  btnCard.spriteState;
        ss.disabledSprite = coveredSprites[id - 1];
        btnCard.spriteState = ss;
    }
}
