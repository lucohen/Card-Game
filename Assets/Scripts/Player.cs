using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//Manages player activity and the movement of their cards and resources
public class Player : MonoBehaviour
{
    public Deck deck;
    public Deck discardPile;
    public Hand hand; //Hand functions like a deck
    public int resources;
    public GameObject resourceCounter;
    public FactionEnum faction;
    public List<KeywordEnum> keywordEnums;
    public List<Base> bases;
    [HideInInspector] public Base currentBase;
    public CapitalShipZone capitalShipZone;
    public PlayArea capitalShipsInPlay;
    [HideInInspector] BaseBody baseBody;
    public int basesLeft;
    public Player opponent;
    public BaseDisplayer displayer;
    [HideInInspector] public bool myTurn;
    public CardGame cardGame;



    // Start is called before the first frame update
    void Start()
    {
        if (faction == FactionEnum.Empire)
        {
            myTurn = true;
        }
        resourceCounter.GetComponent<TMP_Text>().text = resources.ToString();
        GainResources(3);
        currentBase = bases[0];

        CreateBase();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void DisplayBases()
    {
        bases.Remove(currentBase);
        displayer.DisplayBases(bases, this);
    }

    public void ChangeBase(Base chosenBase)
    {
        if (baseBody != null)
        {
            
            Destroy(baseBody.gameObject);

        }
            

        currentBase = chosenBase;
        CreateBase();
    }

    public void CreateBase()
    {
        currentBase.hp = currentBase.maxHp;
        Debug.Log(currentBase.hp + "/" + currentBase.maxHp);
        baseBody = Instantiate(currentBase.bodyPrefab);
        currentBase.body = baseBody;
        baseBody.Initialize(currentBase, displayer);
        if (myTurn)
        {
            baseBody.transform.position = cardGame.gameBoard.playerBasePosition.position;
        }
        else
        {
            baseBody.transform.position = cardGame.gameBoard.opponentBasePosition.position;
        }
        CardGame.Instance.RegisterReactions(currentBase.reactions);
        foreach (EffectEntry entry in currentBase.onRevealEffects)
        {
            entry.effect.Resolve(entry.data);
        }
    }

    public void ChangeTurn()
    {
        myTurn = !myTurn;
    }

    public void ChangePosition()
    {
        Debug.Log("Swap positions");
        if (myTurn)
        {
            hand.playing = false;
            transform.position = cardGame.gameBoard.opponentPosition.position;
            if (!currentBase.keepAbilityDuringOpponentTurn)
            {
                CardGame.Instance.UnregisterReactions(currentBase.reactions);
            }
        }

        else
        {
            hand.playing = true;
            transform.position = cardGame.gameBoard.playerPosition.position;
            if (!currentBase.keepAbilityDuringOpponentTurn)
            {
                CardGame.Instance.RegisterReactions(currentBase.reactions);
            }
        }
        MoveBase();
        MoveHand(hand, deck.transform);
        MoveHand(capitalShipsInPlay, baseBody.transform);
    }

    public void MoveBase()
    {
        if (myTurn)
        {
            baseBody.transform.position = cardGame.gameBoard.opponentBasePosition.position;
        }
        else
        {
            baseBody.transform.position = cardGame.gameBoard.playerBasePosition.position;
        }

        Debug.Log(capitalShipZone.transform.position);
        capitalShipZone.transform.position = new Vector3(baseBody.transform.position.x + 22, baseBody.transform.position.y, 0);
    }

    public void MoveHand(CardGroup group, Transform front)
    {
            foreach (Card card in group.deckList)
            {
                CardBody body = card.body;
                Transform trans = body.transform;
                trans.position = new Vector3(trans.position.x, front.position.y, trans.position.z);
                body.MoveWithSlot(trans);
            }
        
    }

    public void GainResources(int num)
    {
        resources += num;
        resourceCounter.GetComponent<TMP_Text>().text = resources.ToString();
    }

    public void LoseResources(int num)
    {
        resources -= num;
        resourceCounter.GetComponent<TMP_Text>().text = resources.ToString();
    }

    public void PurchaseCard(Card card)
    {
        ManagePurchases.Instance.PurchaseCard(card, this);
    }

    public int NumCapitalShips()
    {
        return capitalShipsInPlay.deckList.Count;
    }


    public void StartTurn() //Called after sides are swapped and the turn has officially started.
    {
        foreach(CapitalShip ship in capitalShipsInPlay.deckList)
        {
            GainResources(ship.resources);
        }
    }
}
