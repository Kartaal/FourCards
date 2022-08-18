using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CardSuitEnum;
using static CardValueEnum;

public class PlayedDeckController : DeckController
{
    public void PushMultiple(List<Card> newCards) {
        int foursCounter = 0;

        base.PushMultiple(newCards); // Push the cards as normal

        // If a card played has value of 10, played cards are discarded
        if(newCards[0].Value == TEN)
        {
            DiscardDeck();
            return;
        }

        // Check top four cards for being four of a kind
        for (int index = Cards.Count - 4; index < Cards.Count; index++)
        {
            Card checkCard = Cards[index];
            if(checkCard.Value == newCards[0].Value)
            {
                foursCounter++;
            } else { // If a card tested has a different value, can't have four of a kind
                break;
            }
        }

        // Four of a kind on top of deck, discard played cards
        if(foursCounter == 4)
        {
            DiscardDeck();
        }
    }

    private void DiscardDeck() {
        Manager.DiscardCards(Cards, gameObject);
    }
}
