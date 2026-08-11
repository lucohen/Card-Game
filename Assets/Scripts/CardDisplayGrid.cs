using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDisplayGrid : MonoBehaviour
{

    public int columns = 2;
    public float horizontalPaddingPercent = 0f; // 10% margins
    public float verticalPaddingPercent = 0.1f;
    public float spacingX = 3.5f;
    public float spacingY = 4f;
    public GameObject background; //"Background" that blocks the clicker from hitting anything on the game board while choosing a new base
    public List<CardBody> displayedCards;
    [HideInInspector] public bool displaying = false; //The body checks to ensure the bases can be selected when clicked on
    
    public GameObject confirmOptions; //Another layer of blocker plane for the confirmation screen
    
    // Start is called before the first frame update

    public GameObject ui;
    public static CardDisplayGrid Instance;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DisplayCards(WhichDisplayGrid grid)
    {
        if (grid == WhichDisplayGrid.None)
        {
            return;
        }
        CardGroup cards = SelectGroup(grid);
        ui.SetActive(false);
        displaying = true;
        background.SetActive(true);

        int count = cards.deckList.Count;
        Debug.Log(count);
        Camera cam = Camera.main;

        float screenHeight = 2f * cam.orthographicSize;
        float screenWidth = screenHeight * cam.aspect;

        float usableWidth = screenWidth * (1f - horizontalPaddingPercent);
        float usableHeight = screenHeight * (1f - verticalPaddingPercent);

        int rows = Mathf.CeilToInt((float)count / columns);
        float spacingX = usableWidth / columns;
        float spacingY = usableHeight / rows;

        float startX = -usableWidth / 2f + spacingX / 2f;
        float startY = usableHeight / 2f - spacingY / 2f;
        for (int i = 0; i < count; i++)
        {
            int row = i / columns;
            int col = i % columns;

            CardBody newCard = Instantiate(cards.deckList[i].bodyPrefab);
            cards.deckList[i].body = newCard;
            newCard.Initialize(cards.deckList[i]);

            float x = startX + col * spacingX;
            float y = startY - row * spacingY;

            newCard.transform.position = new Vector3(x, y, -2);
            displayedCards.Add(newCard);
        }
    }

    public void OnClickTest()
    {
        Debug.Log("Control ts");
    }

    public void StopDisplaying()
    {
        confirmOptions.SetActive(false);
        background.SetActive(false);
        foreach (CardBody card in displayedCards)
        {
            Destroy(card.gameObject);

        }
        displayedCards.Clear();
        ui.SetActive(true);
    }

    public CardGroup SelectGroup(WhichDisplayGrid grid)
    {
        switch (grid)
        {
            case WhichDisplayGrid.playerDeck:
                return CardGame.Instance.currentPlayer.deck;
            case WhichDisplayGrid.playerDiscard:
                return CardGame.Instance.currentPlayer.discardPile;
            case WhichDisplayGrid.galaxyDeck:
                return CardGame.Instance.galaxyDeck;
            case WhichDisplayGrid.galaxyDiscard:
                return CardGame.Instance.galaxyShop.discardPile;
        }
        return null;
        
    }

    

    
}