using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGame : MonoBehaviour


{
    public Player rebelPlayer;
    public Player empirePlayer;
    [HideInInspector] public Player currentPlayer;
    public Deck galaxyDeck;
    public GalaxyShop galaxyShop;
    public OuterRimPilotDeck orp;
    public PlayArea playArea;
    public GameBoard gameBoard;
    public CardDatabase cardDatabase;
    public int max = 3;
    [HideInInspector] public bool canSwap = false;
    [HideInInspector] public bool canClick = false;

    public static CardGame Instance;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        cardDatabase.Awake();
        CreateStartingDecks();
        StartCoroutine(galaxyShop.RefillHand(max));
        StartCoroutine(rebelPlayer.hand.RefillHand(max));
        StartCoroutine(empirePlayer.hand.RefillHand(max));
        empirePlayer.hand.playing = true; //Helps prevent player from moving cards while cards are being moved
        currentPlayer = empirePlayer;
        StartCoroutine(StartBuffer());
    }
    void Update() 
    {
        
    }

    private IEnumerator StartBuffer()
    {
        yield return new WaitForSeconds(2f);
        canClick = true;
    }

    public void ChangeTurns()
    {
        if (canClick)
        {
            canClick = false;
            canSwap = false;
            if (rebelPlayer.deck.IsEmpty())
            {
                rebelPlayer.deck.RefillDeck();
            }
            if (empirePlayer.deck.IsEmpty())
            {
                empirePlayer.deck.RefillDeck();
            }
            if (empirePlayer.myTurn)
            {
                StartCoroutine(CreateNewHand(empirePlayer));
                currentPlayer = rebelPlayer;
            }
            else
            {
                StartCoroutine(CreateNewHand(rebelPlayer));
                currentPlayer = empirePlayer;
            }
        }

    }

    private IEnumerator CreateNewHand(Player player)
    {
        StartCoroutine(playArea.DiscardAll(player.discardPile));
        yield return new WaitForSeconds(0.3f * (playArea.deckList.Count+1));
        StartCoroutine(player.hand.CreateNewHand(max));
        yield return new WaitForSeconds(0.1f);
        while (!canSwap)
        {
            yield return new WaitForSeconds(0.1f);
        }
        SwapPlayerPositions();
        yield return new WaitForSeconds(0.5f);
        StartTurn();
    }

    private void StartTurn()
    {
        currentPlayer.StartTurn();
    }

    private void SwapPlayerPositions()
    {
        rebelPlayer.ChangePosition();
        empirePlayer.ChangePosition();
        SwapMyTurn();
        canClick = true;
    }
    

    private void CreateStartingDecks()
    {
        for (int i = 0; i < galaxyDeck.deckList.Count; i++)
        {
            //Card adding = galaxyDeck.deckList[i];
            //galaxyDeck.deckList.Add(adding);
            galaxyDeck.deckList[i].currentLocation = galaxyDeck;
            galaxyDeck.Shuffle();
        }
        for (int i = 0; i < rebelPlayer.deck.deckList.Count; i++)
        {
            //Card adding = rebelPlayer.deck.deckList[i];
            //rebelPlayer.deck.deckList.Add(adding);
            rebelPlayer.deck.deckList[i].currentLocation = rebelPlayer.deck;
            rebelPlayer.deck.Shuffle();
        }
        for (int i = 0; i < empirePlayer.deck.deckList.Count; i++)
        {
            //Card adding = empirePlayer.deck.deckList[i];
            //empirePlayer.deck.deckList.Add(adding);
            empirePlayer.deck.deckList[i].currentLocation = empirePlayer.deck;
            empirePlayer.deck.Shuffle();
        }
        for (int i = 0; i < orp.deckList.Count; i++)
        {
            //Card adding = empirePlayer.deck.deckList[i];
            //empirePlayer.deck.deckList.Add(adding);
            orp.deckList[i].currentLocation = orp;
        }
    }

    public void SwapMyTurn()
    {
        rebelPlayer.ChangeTurn();
        empirePlayer.ChangeTurn();
    }

    public void CommenceAttack()
    {
        foreach (Card card in playArea.deckList)
        {
            card.Attack();
        }
    }

    // Map each event type to the (effect, data) pairs listening to it
    private Dictionary<GameEventType, List<ReactionEntry>> _reactions = new();

    public void RegisterReactions(List<ReactionEntry> reactions)
    {
        foreach (var reaction in reactions)
        {
            if (reaction.trigger == null) continue;
            var key = reaction.trigger.ListenFor;
            if (!_reactions.ContainsKey(key))
                _reactions[key] = new();
            _reactions[key].Add(reaction);
        }
    }

    public void UnregisterReactions(List<ReactionEntry> reactions)
    {
        foreach (var reaction in reactions)
        {
            if (reaction.trigger == null) continue;
            var key = reaction.trigger.ListenFor;
            if (_reactions.TryGetValue(key, out var list))
                list.Remove(reaction);
        }
    }

    public void FireEvent(GameEventType type, GameEventContext context)
    {
        if (!_reactions.TryGetValue(type, out var list)) return;
        foreach (var reaction in new List<ReactionEntry>(list))
        {
            if (reaction.trigger.Matches(context))
                reaction.effect.effect.Resolve(reaction.effect.data);
        }
    }
}
