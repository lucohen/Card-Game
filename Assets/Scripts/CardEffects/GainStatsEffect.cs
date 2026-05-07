using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/GainStatsEffect")]
public class GainStatsEffect : CardEffect
{
    public override void Resolve(CardGame game, EffectData data)
    {
        var statsData = (GainStatsData)data;
        
        statsData.card.IncreaseStat(statsData.attack, 0);
        statsData.card.IncreaseStat(statsData.resources, 1);
        statsData.card.IncreaseStat(statsData.force, 2);

    }
}
