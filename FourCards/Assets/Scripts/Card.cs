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
    private Outline Outline;
    [SerializeField] public CardSuitEnum Suit { get; private set; }
    [SerializeField] public CardValueEnum Value { get; private set; }
    public bool IsFaceUp { get; private set; } = true;
    public bool IsSelected { get; private set; } = false;

    private void Awake() {
        Img = gameObject.GetComponent<Image>();
        Outline = gameObject.GetComponent<Outline>();

        Outline.enabled = false;

        // Debug.Log($"Card face up: {IsFaceUp}");
        FlipCard();
        // Debug.Log($"After awake flip) Card face up: {IsFaceUp}");
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
        // Debug.Log($"Flipping card... face up is {IsFaceUp} - sprite is Face? {Img.sprite == Face}");
        if(IsFaceUp)
        {
            Img.sprite = Back;
            IsFaceUp = !IsFaceUp;
        } else {
            Img.sprite = Face;
            IsFaceUp = !IsFaceUp;
        }
    }

    public void ToggleOutline() {
        Outline.enabled = !Outline.enabled;
        IsSelected = !IsSelected;
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
        
        if(cardSprite) // Safeguard against missing or misnamed sprites
        {
            // Debug.Log($"Card given sprite with resource path <{resourcePath}>");
            Img.sprite = cardSprite;
            Face = cardSprite;
        } else {
            Debug.Log($"Card sprite not found with path: {resourcePath}");
        }
        
    }
}
