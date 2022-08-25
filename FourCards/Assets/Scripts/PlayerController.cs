using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameManager Manager;
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

                // Just check if the card is in hand or in final sets -- FindCardInFinalSet(hitCard)

                Transform cardTransform = hitCard.transform;

                // Loop through parents to ensure this card belongs to the player
                while ( cardTransform.parent != null
                    && cardTransform.parent.gameObject != this.gameObject 
                )
                {
                    cardTransform = cardTransform.parent;
                }

                // Only do card selection things with cards owned by player
                if(cardTransform.parent && cardTransform.parent.gameObject == this.gameObject)
                {
                    // Debug.Log($"Hand empty? {Hand.IsEmpty()}");
                    // Debug.Log($"Card transform name: {cardTransform.gameObject.name}");

                    // Dumb hardcoding...
                    if(cardTransform.gameObject.name == "PlayerHand" ||  // if card is in hand or...
                        (
                        Hand.IsEmpty() &&                                       // player's hand is empty
                        cardTransform.gameObject.name == "FinalSets_P"          // and we're in final sets container
                        )
                    )
                    {
                        // Toggle card selection
                        if(!hitCard.IsSelected) {
                            SelectedCards.Add(hitCard);
                            hitCard.ToggleOutline();
                        } else {
                            SelectedCards.Remove(hitCard);
                            hitCard.ToggleOutline();
                        }

                        Debug.Log($"{SelectedCards.Count} cards currently selected:");
                        foreach (Card sCard in SelectedCards)
                        {
                            Debug.Log($"Suit: {sCard.Suit} - Value: {sCard.Value}");
                        }
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

    public void PlaySelectedCards() {
        Manager.PlayCards(SelectedCards);
        Hand.RemoveSelected(SelectedCards);
        
        SelectedCards.Clear();
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
