using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/DrawCardEffect")]
public class DrawCardEffect : CardEffect
{
    public override void Resolve(CardGame game, EffectData data)
    {
        var drawData = (DrawCardData)data;
        for (int i = 0; i < drawData.amount; i++)
        {
            game.currentPlayer.deck.MoveTopCard(game.currentPlayer.hand);
        }
    }
}
