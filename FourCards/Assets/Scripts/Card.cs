using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CardSuitEnum;

public class Card : MonoBehaviour
{
    [SerializeField] GameObject cardObj;
    Transform cardTransform;
    MeshRenderer cardRenderer;
    int frame = 0;

    private void Awake() {
        cardTransform = cardObj.transform;
        cardRenderer = cardObj.GetComponent<MeshRenderer>();
        cardTransform.localScale = new Vector3(5f, 1f, 5f);
        cardTransform.localPosition = new Vector3(0f, 0f, 0.05f);
    }


    private void Update() {
        frame++;
        /*
        if( frame % 240 == 0 )
        {
            setCardFace(CardSuitEnum.Spades, 4);
        } else if(frame % 120 == 0)
        {
            setCardFace(CardSuitEnum.Hearts, 4);
        }
        */
    }

    public void flipCard() {
        transform.RotateAround(transform.localPosition, Vector3.right, 180);
    }

    public void setCardFace(CardSuitEnum suit, int value) {
        string resourcePath = "Materials/Cards/Cards/" + value;

        switch (suit) {
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
            default:
                break;
        }

        Material cardMaterial = Resources.Load(resourcePath) as Material;
        cardRenderer.material = cardMaterial;
    }
}
