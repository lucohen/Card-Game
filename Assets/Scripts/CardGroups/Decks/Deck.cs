using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : CardGroup
{
    //public GameObject GalaxyShop;
    public Hand hand;
    public DeckEnum deckEnum;
    public Deck discardPile;

    
    

    private void Awake()
    {
        

        Shuffle();
        
    }

    public virtual void OnDeckClicked()
    {
        AddCardToHand();
    }

    

    public void Shuffle()
    {
        for (int i = 0; i < deckList.Count; i++)
        {
            int rand = Random.Range(0, deckList.Count);
            Card temp = deckList[i];
            deckList[i] = deckList[rand];
            deckList[rand] = temp;

        }
    }

    
    


    public void MoveTopCard(CardGroup goTo)   //Move the top card of a deck stack
    {
        //Debug.Log("Move to " + goTo + " from " + this);
        Card card = RemovetopCard();
        if (card != null)
        {
            goTo.AddCard(card);
        }
        else
        {
            RefillDeck();
            Debug.Log("Deck is Empty " + this);
        }

    }


    public void AddCardToHand()   //Move the top card from a deck stack to its hand
    {
        MoveTopCard(hand);
    }


    public void RefillDeck()
    {
        Debug.Log("RefillDeck " + this);
        int a = discardPile.deckList.Count;
        for (int i = 0; i < a; i++)
        {
            discardPile.MoveTopCard(this);
        }
        Shuffle();
    }

}

