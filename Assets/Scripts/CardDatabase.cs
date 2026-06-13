using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Database")]
public class CardDatabase : ScriptableObject
{

    private List<Card> AllCards;
    public List<Card> OuterRimPilots;
    public List<Card> RebelStarterCards;
    public List<Card> EmpireStarterCards;
    public List<Card> GalaxyDeckCards;
    private int assignID;
    public static CardDatabase Instance;
    public CardBody bodyPrefab;
    public CardBody CapitalShipBodyPrefab;

    
    // Start is called before the first frame update
    public void Awake()
    {
        AllCards = new List<Card>();
        AllCards.AddRange(RebelStarterCards);
        AllCards.AddRange(EmpireStarterCards);
        AllCards.AddRange(OuterRimPilots);
        AllCards.AddRange(GalaxyDeckCards);
        Instance = this;
        assignID = 0;
        foreach (Card card in AllCards)
        {
            card.bodyPrefab = bodyPrefab;
            if (card is CapitalShip)
            {
                ((CapitalShip)card).CapitalShipBodyPrefab = CapitalShipBodyPrefab;
            }
            card.assignCardID(assignID);
            assignID++;

            //Debug.Log(card.GetCardID());
        }
    }

    public void Exile(Card c)
    {
        AllCards.Remove(c);
    }

}
