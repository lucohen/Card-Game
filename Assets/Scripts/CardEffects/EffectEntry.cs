using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EffectEntry
{
    public CardEffect effect;

    [SerializeReference] public EffectData data;
}

[System.Serializable]
public abstract class EffectData
{
}