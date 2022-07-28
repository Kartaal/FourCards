using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] DeckController Deck;
    FinalSet[] FinalSets;


    private void Awake() {
        FinalSets = GetComponentsInChildren<FinalSet>();

        // Debug.Log($"Number of FinalSet children: {FinalSets.Length}");
    }

    public void InitFinalSets(List<Card> cards) {
        if(cards.Count == (FinalSets.Length * 2)) // 2 cards per set
        {
            for (int index = 0; index < cards.Count; index++)
            {
                int setIndex = index / 2; // Finds the index in the array, two cards per set
                int cardIndex = index % 2;

                Debug.Log($"Set index: {setIndex}");
                Debug.Log($"Card index: {cardIndex}");

                if(cardIndex == 0) { // QQQQQ -- Should find a way to clean this up 
                    FinalSets[setIndex].SetTopCard(cards[index]);
                } else {
                    FinalSets[setIndex].SetBottomCard(cards[index]);
                }
            }
        }
    }

    public FinalSet FindCardInFinalSet(Card card) {
        foreach (var set in FinalSets)
        {
            if(set.HasCard(card))
                return set;
        }

        return null; // Didn't find the card in any set
    }
}
