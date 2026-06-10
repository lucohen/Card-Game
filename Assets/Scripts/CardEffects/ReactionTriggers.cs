
using UnityEngine;

public abstract class ReactionTrigger : ScriptableObject
{
    public abstract GameEventType ListenFor { get; }

    // Optional: filter which events actually fire the reaction.
    // e.g. "only when the purchased card costs 5+".
    // Return true by default so simple triggers need no extra logic.
    public virtual bool Matches(GameEventContext context) => true;
}