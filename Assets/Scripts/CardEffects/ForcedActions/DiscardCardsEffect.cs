using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscardAction : PendingAction
{
    private readonly Hand _hand;
    private readonly int _required;
    private readonly bool _canEndEarly;
    private readonly HashSet<Card> _selected = new();

    public override string Prompt => $"Discard {_required} card(s). ({_selected.Count}/{_required} selected)";

    public DiscardAction(Hand hand, int required, bool canEndEarly)
    {
        _hand = hand;
        _required = required;
        _canEndEarly = canEndEarly;

    }

    public override bool IsValidTarget(object target)
    {
        if (_hand == CardGame.Instance.currentPlayer.hand)
            {
            return target is Card c && (_hand.Contains(c) || CardGame.Instance.playArea.Contains(c)); //lets you also discard from play area
        }
        return target is Card card && _hand.Contains(card);
    }

    // Toggle selection on click instead of immediately executing
    public override void Execute(object target)
    {
        if (target is not Card card) return;

        if (_selected.Contains(card))
        {
            _selected.Remove(card);
            card.body.UnMark();
        }
        else if (_selected.Count < _required)
        {
            _selected.Add(card);
            card.body.Mark();
        }

        Messenger.Instance.ShowPrompt(Prompt);
        ActionManager.Instance.confirmButton.SetActive(true);
    }

    public bool CanConfirm => (_selected.Count == _required || _canEndEarly);

    

    public override void OnActivated()
        => Messenger.Instance.ShowPrompt(Prompt);

    public override void OnResolved()
    {
        foreach (var card in _selected)
        {
            card.body.UnMark();
            if (card.currentLocation is Hand)
                ((Hand)card.currentLocation).Discard(card);
            else if (card.currentLocation is PlayArea)
                ((PlayArea)card.currentLocation).Discard(card, CardGame.Instance.currentPlayer.discardPile); //kind of messy but whatever for now
        }
        Messenger.Instance.HidePrompt();
        ActionManager.Instance.confirmButton.SetActive(false);
    }
}

[CreateAssetMenu(menuName = "Effects/DiscardCardsEffect")]
public class DiscardCardsEffect : CardEffect                //Card effect that pushes the action onto the stack

{
    private Hand hand;
    public override void Resolve(EffectData data)
    {

        var discardData = (DiscardCardsData)data;
        switch (discardData.whichHand)
        {
            case WhichHand.player:
                hand = CardGame.Instance.currentPlayer.hand;
                break;
            case WhichHand.opponent:
                hand = CardGame.Instance.currentPlayer.opponent.hand;
                break;
            case WhichHand.shop:
                hand = CardGame.Instance.galaxyShop.hand;
                break;
        }
        DiscardAction action = new DiscardAction(hand, discardData.amount, discardData.canEndEarly);

        ActionManager.Instance.Push(action);
        
    }
}

public class DiscardCardsData : EffectData
{
    public WhichHand whichHand;
    public int amount;
    public bool canEndEarly;
}

public enum WhichHand{
    player,
    opponent,
    shop,
}