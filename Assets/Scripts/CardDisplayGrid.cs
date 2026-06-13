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
    public CardBody bodyPrefab;
    private Player player;
    private CardBody chosenBase;
    private Vector3 resetPosition; // puts the base back in position if not chosen
    // Start is called before the first frame update

    public GameObject ui;


    public void DisplayCards(Deck deck)
    {
        ui.SetActive(false);
        displaying = true;
        background.SetActive(true);

        int count = deck.Count();
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

            deck.deckList[i].body = bodyPrefab;
            CardBody newCard = Instantiate(deck.deckList[i].body);
            newCard.Initialize(deck.deckList[i]);

            float x = startX + col * spacingX;
            float y = startY - row * spacingY;

            newCard.transform.position = new Vector3(x, y, -2);
            displayedCards.Add(newCard);
        }
    }

    

    public void Deactivate()
    {
        background.SetActive(false);

        for (int i = displayedCards.Count - 1; i >= 0; i--)
        {
            Destroy(displayedCards[i].gameObject);
        }

        displayedCards.Clear();
        ui.SetActive(true);


    }

}
