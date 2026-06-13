using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class PendingAction
{
    public abstract string Prompt { get; }          // e.g. "Discard a card"
    public abstract bool IsValidTarget(object target);
    public abstract void Execute(object target);

    public virtual void OnActivated() { }           // show UI prompt, highlight valid targets, etc.
    public virtual void OnResolved() { }            // clean up highlights, etc.
}