using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : Deck
{
    private float cardSpacing = 3f;
    public Deck mainDeck;
    public Deck otherDeck;
    public CardZoneBase zone;
    public bool playing = false;

    public override void AddCard(Card card) //adds the card info to the hand's "deck" and creates the body if it is drawn from a deck
    {
        base.AddCard(card);
        Debug.Log(card.body == null);
        if (card.body == null)
        {
            Debug.Log("body was null, instantiate at " + mainDeck);         
            CardBody newCard = Instantiate(card.bodyPrefab, mainDeck.transform.position, mainDeck.transform.rotation);
            card.body = newCard;
            newCard.Initialize(card);
            CardMovementManager.Instance.MoveToZone(zone, newCard);
            
        }
        else
        {
            Debug.Log("Body was not null, don't instantiate");
            Debug.Log(card.body.transform.position);
        }

    }

    public void Discard(Card card)
    {
        MoveCard(discardPile, card);
        StartCoroutine(CardMovementManager.Instance.MoveFromZone(discardPile.transform, card.body));

    }

    //public void MoveHand() //Move every card in hand to the correct position in relation to the main deck
    //{
    //    foreach (Card card in deckList)
    //    {
    //        CardBody body = card.body;
    //        Transform trans = body.transform;
    //        trans.position = new Vector3(trans.position.x, mainDeck.transform.position.y, trans.position.z);
    //        body.MoveWithSlot(trans);
    //    }
    //}

    public IEnumerator RefillHand(int max)
    {
        while (deckList.Count < max)
        {
            if (mainDeck.IsEmpty())
            {
                mainDeck.RefillDeck();
                //yield return new WaitForSeconds(0.1f);
            }
            mainDeck.AddCardToHand();
            yield return new WaitForSeconds(0.3f);
        }
        yield return new WaitForSeconds(1f);
        cardGame.canSwap = true;
    }

    public IEnumerator CreateNewHand(int max)
    {
        yield return new WaitForSeconds(0.1f);
        while (deckList.Count > 0)
        {
            Discard(deckList[^1]);
            yield return new WaitForSeconds(0.3f);
        }
        StartCoroutine(RefillHand(max));
    }

    public bool IsFull(int max)
    {
        return deckList.Count == max;
    }












}
