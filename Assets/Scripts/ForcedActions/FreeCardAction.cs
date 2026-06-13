using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCardAction : PendingAction
{
    private readonly GalaxyShop _shop;
    private readonly int _amount;
    private readonly bool _addToHand;

    public override string Prompt => $"Discard {_amount} card(s).";

    public FreeCardAction(GalaxyShop shop, int amount, bool addToHand)
    {
        _shop = shop;
        _amount = amount;
        _addToHand = addToHand;
    }

    public override bool IsValidTarget(object target)
        => target is Card card && _shop.Contains(card);

    public override void Execute(object target)
    {
        Player player = CardGame.Instance.currentPlayer;
        if (target is Card card && card.MatchesPlayerFaction(player)){
            CardGame.Instance.galaxyShop.nextCardFree = true;
            if (_addToHand) CardGame.Instance.galaxyShop.addNextCardToHand = true;
            player.PurchaseCard(card);
        }
    }

    public override void OnActivated()
        => Messenger.Instance.ShowPrompt(Prompt);

    public override void OnResolved()
        => Messenger.Instance.HidePrompt();
}

