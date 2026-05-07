using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameEvent { }

public class TurnStartEvent : GameEvent
{
    public Player player;
}

public class CardPurchasedEvent : GameEvent
{
    public Card card;
    public Player player;
}

public class CardPlayedEvent : GameEvent
{
    public Card card;
    public Player player;
}