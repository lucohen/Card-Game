using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/MoveForceEffect")]
public class MoveForceEffect : CardEffect
{
    public override void Resolve(EffectData data)
    {
        var moveData = (MoveForceData)data;
        ForceBar.Instance.MoveBar(moveData.amount);
    }
}


public class MoveForceData : EffectData
{
    public int amount;
}

