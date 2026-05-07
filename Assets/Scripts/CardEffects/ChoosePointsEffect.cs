using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[CreateAssetMenu(menuName = "Effects/ChoosePointsEffect")]
public class ChoosePointsEffect : CardEffect
{
    public override void Resolve(CardGame game, EffectData data)
    {
        Time.timeScale = 0f;
        var chooseData = (ChooseEffectData)data;
        ChoosePointsMenu.Instance.Resolve(chooseData);
    }

}

