using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static CardSuitEnum;
using static CardValueEnum;

public class Card : MonoBehaviour
{
    [SerializeField] Sprite Face;
    [SerializeField] Sprite Back;

    private Image Img;
    public CardSuitEnum Suit { get; private set; }
    public CardValueEnum Value { get; private set; }
    private bool IsFaceUp = true;

    int frame = 0;

    private void Awake() {
        Img = gameObject.GetComponent<Image>();
    }

    private void Update() {
        /*
        if(frame % 120 == 0)
        {
            SetCardFace(Suit, Value);
        } else if(frame % 60 == 0)
        {
            SetCardFace(CardSuitEnum.Hearts, CardValueEnum.JACK);
        }
        frame++;
        */
    }

    public void FlipCard() {
        if(IsFaceUp)
        {
            Img.sprite = Back;
            IsFaceUp = !IsFaceUp;
        } else {
            Img.sprite = Face;
            IsFaceUp = !IsFaceUp;
        }

    }

    public void SetCardFace(CardSuitEnum newSuit, CardValueEnum newValue) {
        
        string resourcePath = "Materials/Cards/Cards/Textures/"; // Path to the folder

        switch(newValue) // Find and append card's value to resource load path, ace and picture card use strings
        {
            case CardValueEnum.TWO:
                resourcePath += 2;
                break;
            case CardValueEnum.THREE:
                resourcePath += 3;
                break;
            case CardValueEnum.FOUR:
                resourcePath += 4;
                break;
            case CardValueEnum.FIVE:
                resourcePath += 5;
                break;
            case CardValueEnum.SIX:
                resourcePath += 6;
                break;
            case CardValueEnum.SEVEN:
                resourcePath += 7;
                break;
            case CardValueEnum.EIGHT:
                resourcePath += 8;
                break;
            case CardValueEnum.NINE:
                resourcePath += 9;
                break;
            case CardValueEnum.JACK:
                resourcePath += "J";
                break;
            case CardValueEnum.QUEEN:
                resourcePath += "Q";
                break;
            case CardValueEnum.KING:
                resourcePath += "K";
                break;
            case CardValueEnum.ACE:
                resourcePath += "ACE";
                break;
            case CardValueEnum.TEN:
                resourcePath += 10;
                break;
            default:
                Debug.Log($"SetCardFace called with invalid value! {newValue}");
                break;
        }

        Value = newValue;

        switch (newSuit) { // Find and append suit to resource load path
            case CardSuitEnum.Hearts:
                resourcePath += "ofHearts";
                break;
            case CardSuitEnum.Spades:
                resourcePath += "ofSpades";
                break;
            case CardSuitEnum.Diamonds:
                resourcePath += "ofDiamonds";
                break;
            case CardSuitEnum.Clubs:
                resourcePath += "ofClubs";
                break;
            default: // Should be impossible to reach this
                Debug.Log($"SetCardFace called with invalid suit! {newSuit}");
                break;
        }

        Suit = newSuit;

        Sprite cardSprite = Resources.Load<Sprite>(resourcePath);
        
        if(cardSprite && IsFaceUp) // Safeguard against missing or misnamed sprites, don't change if face down
        {
            Img.sprite = cardSprite;
        } else {
            Debug.Log($"Card sprite not found with path: {resourcePath}");
        }
        
    }
}
