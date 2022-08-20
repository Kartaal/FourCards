using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandDeckController : DeckController
{
    [SerializeField] Card _CardPrefab;

    public override void PushMultiple(List<Card> newCards) {
        base.PushMultiple(newCards);

        foreach (Card card in newCards)
        {
            
            RectTransform cardTransform = card.gameObject.transform as RectTransform;
            RectTransform _prefab = _CardPrefab.gameObject.transform as RectTransform;
            
            cardTransform.SetParent(gameObject.transform, false);
            
            cardTransform.localScale = _prefab.localScale;
        }

        SortDeck();
    }

    // Insertion sort from https://dotnettutorials.net/lesson/insertion-sort-in-csharp/
    public void SortDeck() {
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

        // Remember to update the game object ordering to match the sorted list
        for (int index = 0; index < Cards.Count; index++)
        {
            Card card = Cards[index];
            RectTransform cardTransform = card.transform as RectTransform;

            cardTransform.SetSiblingIndex(index);
        }
    }
}
