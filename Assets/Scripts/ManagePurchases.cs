using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagePurchases : MonoBehaviour
{

    public static event Action<ManagePurchases> OnCardPurchasedEvent;
    // Start is called before the first frame update
    public static void PurchaseCard(Card card, Player player)
    {
        if (card.cost <= player.resources)
        {
            player.LoseResources(card.cost);
            card.currentAlliegance = player.faction;
            card.MoveInfo(player.discardPile);
            player.StartCoroutine(CardMovementManager.Instance.MoveFromZone(player.discardPile.transform, card.body));
            player.StartCoroutine(CardGame.Instance.galaxyShop.RefillHand(3));
        }
        else
        {
            Debug.Log("Not Enough Resources");
            card.body.Move(card.body.slot.transform);
        }
    }
}
