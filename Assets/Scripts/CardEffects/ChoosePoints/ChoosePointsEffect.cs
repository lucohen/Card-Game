using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[CreateAssetMenu(menuName = "Effects/ChoosePointsEffect")]
public class ChoosePointsEffect : CardEffect
{
    public override void Resolve(EffectData data)
    {
        Time.timeScale = 0f;
        var chooseData = (ChooseEffectData)data;
        ChoosePointsMenu.Instance.Resolve(chooseData);
    }

}

public class ChooseEffectData : EffectData
{
    public Card card;
    public enum Type
    {
        Distributed,
        AllIn
    }
    public Type type;

    public int clicksAllowed;
    public int pointsAvailable;
    public bool[] options = new bool[3];
}

