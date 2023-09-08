using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CardSuitEnum;
using static CardValueEnum;

public class PlayedDeckController : DeckController
{
    public override void PushMultiple(List<Card> newCards) {
        base.PushMultiple(newCards); // Push the cards as normal

        if (CheckAndResolveTenPlayed(newCards)) return;

        CheckAndResolveFourSuits(newCards);
    }

    private bool CheckAndResolveTenPlayed(IReadOnlyList<Card> newCards)
    {
        // If a card played has value of 10, played cards are discarded
        if (newCards[0].Value == TEN)
        {
            DiscardDeck();
            return true;
        }

        return false;
    }

    private void CheckAndResolveFourSuits(IReadOnlyList<Card> newCards)
    {
        int foursCounter = 0;

        // Check top four cards for being four of a kind
        if (Cards.Count > 3)
        {
            // Don't actually check for four of a kind if there are less than 4 cards in pile
            for (int index = Cards.Count - 4; index < Cards.Count; index++)
            {
                Card checkCard = Cards[index];
                if (checkCard.Value == newCards[0].Value)
                {
                    foursCounter++;
                }
                else
                {
                    // If a card tested has a different value, can't have four of a kind
                    break;
                }
            }
        }

        // Four of a kind on top of deck, discard played cards
        if (foursCounter == 4)
        {
            DiscardDeck();
            // QQQ don't update game's state...
        }
    }

    private void DiscardDeck() {
        Manager.DiscardCards(Cards, gameObject);
    }
}
