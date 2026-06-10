
using UnityEngine;

[CreateAssetMenu(menuName = "Triggers/OnCardPurchased")]
public class OnPurchaseTrigger : ReactionTrigger
{
    public int minCost;
    public override GameEventType ListenFor => GameEventType.OnCardPurchased;
    public override bool Matches(GameEventContext context) => context.targetCard.cost >= minCost;
}