using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGroup : MonoBehaviour
{
    public CardGame cardGame;
    public List<Card> deckList = new List<Card>();
    //public GameObject GalaxyShop;
    public bool active;



    private void Awake()
    {

    }




    public bool IsEmpty()
    {
        if (deckList.Count == 0)
        {
            return true;
        }
        return false;
    }

    public Card CheckTopCard()
    {
        return deckList[deckList.Count - 1];
    }

    public Card RemovetopCard() //Remove the top card from a deck stack
    {
        if (deckList.Count < 1)
        {
            Debug.Log("No Cards In Deck " + this);
            return null;
        }
        Card card = deckList[deckList.Count - 1];
        deckList.RemoveAt(deckList.Count - 1);
        return card;
    }

    public virtual Card RemoveCard(int cardID)  //Remove the card with a given ID from a deck (Usually used on hands)
    {
        for (int i = 0; i < deckList.Count; i++)
        {
            if (deckList[i].GetCardID() == cardID)
            {
                Card card = deckList[i];
                deckList.RemoveAt(i);
                return card;

            }
        }
        return null;
    }

    public virtual void AddCard(Card card)
    {
        deckList.Add(card);
        card.currentLocation = this;


    }

    public int Count()
    {
        return deckList.Count;
    }

    public void MoveCard(CardGroup goTo, Card cardToMove)
    {
        Debug.Log("MoveCard " + cardToMove.name + " | " + cardToMove.GetCardID() + " to " + goTo);
        Card card = RemoveCard(cardToMove.GetCardID());
        goTo.AddCard(card);
    }



}

