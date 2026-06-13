using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCardAction : PendingAction
{
    private readonly GalaxyShop _shop;
    private readonly int _amount;
    private readonly bool _addToHand;

<<<<<<< HEAD
    public override string Prompt => $"Discard {_amount} card(s).";
=======
    public override string Prompt => "Choose a card to purchase.";
>>>>>>> parent of 624a96e (Revert "Checkpoint")

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
<<<<<<< HEAD
            player.PurchaseCard(card);
=======
            ManagePurchases.Instance.PurchaseCard(card, player);
            ActionManager.Instance.Resolve();
>>>>>>> parent of 624a96e (Revert "Checkpoint")
        }
    }

    public override void OnActivated()
        => Messenger.Instance.ShowPrompt(Prompt);

    public override void OnResolved()
<<<<<<< HEAD
        => Messenger.Instance.HidePrompt();
}

=======
    {
        
        Messenger.Instance.HidePrompt();
    }
}

[CreateAssetMenu(menuName = "Effects/FreeCardEffect")]
public class FreeCardEffect : CardEffect                //Card effect that pushes the action onto the stack

{
    public override void Resolve(EffectData data)
    {

        var freeCardData = (FreeCardData)data;
        
        FreeCardAction action = new FreeCardAction(CardGame.Instance.galaxyShop, freeCardData.amount, freeCardData.addToHand);

        ActionManager.Instance.Push(action);

    }
}

public class FreeCardData : EffectData
{
    public bool addToHand;
    public int amount;
}
>>>>>>> parent of 624a96e (Revert "Checkpoint")
