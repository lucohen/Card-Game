
public enum GameEventType
{
    OnCardPurchased,
    OnCardPlayed,
    OnCardDrawn,
    OnTurnStart,
    OnTurnEnd,
    OnDamageDealt,
}

public class GameEventContext
{
    public CardGame game;
    public Card sourceCard;   // the card that triggered the event
    public Card targetCard;   // e.g. the card that was purchased
    public int numericValue;  // e.g. damage amount, gold spent
    public Player player;
}