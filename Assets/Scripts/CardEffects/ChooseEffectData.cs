using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
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
