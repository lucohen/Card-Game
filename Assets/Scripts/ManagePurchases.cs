using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ManagePurchases : MonoBehaviour
{
    public static ManagePurchases Instance;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    public void PurchaseCard(Card card, Player player)
    {
        Debug.Log("Test");
        bool free = CardGame.Instance.galaxyShop.nextCardFree;
        bool addToHand = CardGame.Instance.galaxyShop.addNextCardToHand;
        if (card.cost <= player.resources || free)
        {
            if (!free) player.LoseResources(card.cost);
            else CardGame.Instance.galaxyShop.nextCardFree = false;
            card.currentAlliegance = player.faction;
            if (addToHand)
            {
                card.MoveInfo(player.hand);
                CardMovementManager.Instance.MoveBetweenZones(player.hand.zone, card.body);
                CardGame.Instance.galaxyShop.addNextCardToHand = false;
            }
            else
            {
                card.MoveInfo(player.discardPile);
                StartCoroutine(CardMovementManager.Instance.MoveFromZone(player.discardPile.transform, card.body));
            }
            StartCoroutine(CardGame.Instance.galaxyShop.RefillHand(3));

            var context = new GameEventContext
            {
                targetCard = card,
                player = player,
                numericValue = card.cost,
            };
            CardGame.Instance.FireEvent(GameEventType.OnCardPurchased, context);
        }
        else
        {
            Debug.Log("Not Enough Resources");
            card.body.Move(card.body.slot.transform);
        }
    }


}
