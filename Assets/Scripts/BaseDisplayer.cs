using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseDisplayer : MonoBehaviour
{

    public int columns = 2;
    public float horizontalPaddingPercent = 0f; // 10% margins
    public float verticalPaddingPercent = 0.1f;
    public float spacingX = 3.5f;
    public float spacingY = 4f;
    public GameObject background; //"Background" that blocks the clicker from hitting anything on the game board while choosing a new base
    public List<BaseBody> displayedBases;
    [HideInInspector] public bool displaying = false; //The body checks to ensure the bases can be selected when clicked on
    public BaseBody bodyPrefab;
    public GameObject confirmOptions; //Another layer of blocker plane for the confirmation screen
    private Player player;
    private BaseBody chosenBase;
    private Vector3 resetPosition; // puts the base back in position if not chosen
    // Start is called before the first frame update

    public GameObject ui;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayBases(List<Base> bases, Player currentPlayer)
    {
        ui.SetActive(false);
        player = currentPlayer;
        displaying = true;
        background.SetActive(true);
        Debug.Log(columns);
        int count = bases.Count;

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

            bases[i].body = bodyPrefab;
            BaseBody newBase = Instantiate(bases[i].body);
            newBase.Initialize(bases[i], this);

            float x = startX + col * spacingX;
            float y = startY - row * spacingY;

            newBase.transform.position = new Vector3(x, y, -2);
            displayedBases.Add(newBase);
        }
    }

    public void Confirmation(BaseBody b)
    {
        Debug.Log("Confirmation");
        displaying = false;
        confirmOptions.SetActive(true);
        foreach (BaseBody baseBody in displayedBases)
        {
            if (baseBody == b)
            {
                resetPosition = baseBody.transform.position;
                baseBody.transform.position = new Vector3(0, 0, -4);
                break;
            }

        }
        chosenBase = b;
        confirmOptions.SetActive(true);
    }

    public void YesBase()
    {
        confirmOptions.SetActive(false);
        background.SetActive(false);

        for (int i = displayedBases.Count - 1; i >= 0; i--)
        {
            Destroy(displayedBases[i].gameObject);
        }

        displayedBases.Clear();
        ui.SetActive(true);

        player.ChangeBase(chosenBase.baseInfo);
    }

    public void NoBase()
    {
        confirmOptions.SetActive(false);
        chosenBase.transform.position = resetPosition;
        displaying = true;

    }

}
