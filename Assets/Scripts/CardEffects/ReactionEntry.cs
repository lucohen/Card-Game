[System.Serializable]
public class ReactionEntry
{
    public ReactionTrigger trigger;
    public EffectEntry effect; // reuses your existing class exactly as-is
}