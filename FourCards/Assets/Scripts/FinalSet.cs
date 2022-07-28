using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalSet : MonoBehaviour
{
    [SerializeField] Card TopCard;
    [SerializeField] Card BottomCard;

    public bool HasTopCard() {
        return TopCard == null;
    }

    public Card PopCard() {
        if(TopCard) {
            Card card = TopCard;
            TopCard = null;

            return card;
        } else {
            Card card = BottomCard;
            BottomCard = null;

            return card;
        }
    }

    public void SetTopCard(Card card) {
        TopCard = card;
        Transform cardTransform = card.gameObject.transform;

        // Debug.Log($"TopCard: {TopCard} - {TopCard.Value}, {TopCard.Suit}");

        cardTransform.SetParent(gameObject.transform, false);
        cardTransform.SetAsLastSibling();

        if(!card.IsFaceUp) {
            card.FlipCard();
        }
    }

    public void SetBottomCard(Card card) {
        BottomCard = card;
        Transform cardTransform = card.gameObject.transform;

        // Debug.Log($"BottomCard: {BottomCard} - {BottomCard.Value}, {BottomCard.Suit}");

        cardTransform.SetParent(gameObject.transform, false);
        cardTransform.SetAsFirstSibling();

        if(card.IsFaceUp) {
            card.FlipCard();
        }
    }

    public bool HasCard(Card card) {
        return TopCard == card || BottomCard == card;
    }
}
