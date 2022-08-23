using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [SerializeField] HandDeckController Hand;
    FinalSet[] FinalSets;
    List<Card> SelectedCards = new List<Card>();

    // Fields for UI raycasting things
    PointerEventData m_PointerEventData;
    [SerializeField]  GraphicRaycaster m_Raycaster;
    [SerializeField] EventSystem m_EventSystem;

    private void Awake() {
        FinalSets = GetComponentsInChildren<FinalSet>();

        // Debug.Log($"Number of FinalSet children: {FinalSets.Length}");
        // Debug.Log($"Awake) Final sets null? - {FinalSets == null}");
    }

    private void Update() {
        // if(Time.frameCount % 60 == 0) Debug.Log("In update...");
        if(Input.GetMouseButtonDown(0)) // If left mouse button clicked
        {
            //Set up the new Pointer Event
            m_PointerEventData = new PointerEventData(m_EventSystem);
            //Set the Pointer Event Position to that of the game object
            m_PointerEventData.position = Input.mousePosition; //this.transform.localPosition;
 
            //Create a list of Raycast Results
            List<RaycastResult> results = new List<RaycastResult>();
 
            //Raycast using the Graphics Raycaster and mouse click position
            m_Raycaster.Raycast(m_PointerEventData, results);

            Card hitCard = null;
            int index = 0;
            while(hitCard == null && index < results.Count)
            {
                // Loop through UI elements until finding a card or running out of objects
                GameObject GO = results[index++].gameObject;
                hitCard = GO.GetComponent<Card>();

                if(GO && !hitCard) {
                    // Debug.Log($"Hit {GO.name} instead of card...");
                } else {
                    // Debug.Log($"Value: {hitCard.Value} - Suit: {hitCard.Suit}");
                }
            }

            if(hitCard) {   // Only do card hit things if a card was actually hit and the rest of the game/UI wasn't clicked
                Transform cardTransform = hitCard.transform;

                // Loop through parents to ensure this card belongs to the player
                while (cardTransform.parent.gameObject != this.gameObject && cardTransform.parent.gameObject.GetComponent<Canvas>() == null)
                {
                    cardTransform = cardTransform.parent;
                }

                // If the parent of the current 'card' is a canvas, this 'card' does not have this player controller as a parent
                if(cardTransform.parent.gameObject.GetComponent<Canvas>() == null) {
                    if(!hitCard.IsSelected) {           // If the hit card isn't selected, add it to the selected card list and toggle the outline indicator on
                        SelectedCards.Add(hitCard);
                        hitCard.ToggleOutline();
                    } else {                            // If the hit card is selected, remove it from the selected card list and toggle the outline indicator off
                        SelectedCards.Remove(hitCard);
                        hitCard.ToggleOutline();
                    }

                    Debug.Log($"Cards currently selected:");
                    foreach (Card sCard in SelectedCards)
                    {
                        Debug.Log($"Suit: {sCard.Suit} - Value: {sCard.Value}");
                    }
                }
            }
        }
    }

    public void InitFinalSets(List<Card> cards) {
        // Debug.Log("Cards count: " + cards.Count);
        // Debug.Log($"Final sets null? - {FinalSets == null}");
        if(cards.Count == (FinalSets.Length * 2)) // 2 cards per set
        {
            for (int index = 0; index < cards.Count; index++)
            {
                int setIndex = index / 2; // Finds the index in the array, two cards per set
                int cardIndex = index % 2;

                // Debug.Log($"Set index: {setIndex}");
                // Debug.Log($"Card index: {cardIndex}");

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

    public void AddCardsToHand(List<Card> newCards) {
        Hand.PushMultiple(newCards);
        Hand.SortDeck();
    }

    public List<Card> _DebugPopFirstCard() {
        Card card = Hand.Pop()[0]; // Ugly as shit
        List<Card> list = new List<Card>();
        list.Add(card);

        return list;
    }
}
