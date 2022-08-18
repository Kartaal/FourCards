using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandDeckController : DeckController
{
    public override void PushMultiple(List<Card> newCards) {
        base.PushMultiple(newCards);

        SortDeck();
    }

    // Insertion sort from https://dotnettutorials.net/lesson/insertion-sort-in-csharp/
    private void SortDeck() {
        for (int i = 1; i < Cards.Count; i++)
        {
            Card val = Cards[i];
            for (int j = i - 1; j >= 0;)
            {
                if (val.Value < Cards[j].Value)
                {
                    Cards[j + 1] = Cards[j];
                    j--;
                    Cards[j + 1] = val;
                }
                else
                {
                    break;
                }   
            }
        }
    }
}
