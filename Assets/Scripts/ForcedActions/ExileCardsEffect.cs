using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExileAction : PendingAction
{
    private readonly Hand _hand;
    private readonly int _max;
    private readonly int _min;
    private readonly HashSet<Card> _selected = new();
    private readonly WhichDisplayGrid _displayGrid;

    public override string Prompt => $"Exile {_max} card(s). ({_selected.Count}/{_max} selected)";

    public ExileAction(Hand hand, int max, int min, WhichDisplayGrid displayGrid)
    {
        _hand = hand;
        _max = max;
        _min = min;
        _displayGrid = displayGrid;
    }

    public override bool IsValidTarget(object target)
    {
        if (target is Card card)
        {
            if (card.body.currentZoneType == CardZoneEnum.None)
            {
                return true;
            }
            if (_hand == CardGame.Instance.currentPlayer.hand)
            {
                return target is Card c && (_hand.Contains(c) || CardGame.Instance.playArea.Contains(c)); //lets you also discard from play area
            }
            return _hand.Contains(card);
        }
        return false;
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
        else if (_selected.Count < _max)
        {
            Debug.Log(_selected.Count + " | " + _max);
            _selected.Add(card);
            card.body.Mark();
        }

        Messenger.Instance.ShowPrompt(Prompt);
        if (_selected.Count >= _min)
            ActionManager.Instance.confirmButton.SetActive(true);
        else
            ActionManager.Instance.confirmButton.SetActive(false);
    }

    public bool CanConfirm => _selected.Count == _max;



    public override void OnActivated()
    {
        CardDisplayGrid.Instance.DisplayCards(_displayGrid);
        Messenger.Instance.ShowPrompt(Prompt);
        if (_min == 0)
            ActionManager.Instance.confirmButton.SetActive(true);
    }

    public override void OnResolved()
    {
        foreach (var card in _selected)
        {
            Debug.Log(card.cardName);
            card.body.UnMark();
            CardGame.Instance.StartCoroutine(card.ExileRoutine());
        }
        Messenger.Instance.HidePrompt();
        ActionManager.Instance.confirmButton.SetActive(false);
        CardDisplayGrid.Instance.StopDisplaying();
    }
}

[CreateAssetMenu(menuName = "Effects/ExileCardsEffect")]
public class ExileCardsEffect : CardEffect                //Card effect that pushes the action onto the stack

{
    private Hand hand;
    public override void Resolve(EffectData data)
    {

        var exileData = (ExileCardsData)data;
        switch (exileData.whichHand)
        {
            case WhichPlayer.player:
                hand = CardGame.Instance.currentPlayer.hand;
                break;
            case WhichPlayer.opponent:
                hand = CardGame.Instance.currentPlayer.opponent.hand;
                break;
            case WhichPlayer.shop:
                hand = CardGame.Instance.galaxyShop.hand;
                break;
        }
        ExileAction action = new ExileAction(hand, exileData.amount, exileData.minAmount, exileData.DisplayGrid);

        ActionManager.Instance.Push(action);

    }
}

public class ExileCardsData : EffectData
{
    public WhichPlayer whichHand;
    public int amount;
    public int minAmount;
    public WhichDisplayGrid DisplayGrid;
}

public enum WhichDisplayGrid
{
    None,
    playerDeck,
    playerDiscard,
    galaxyDeck,
    galaxyDiscard
}

