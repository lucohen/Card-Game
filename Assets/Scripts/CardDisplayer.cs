using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardDisplayer : MonoBehaviour
{
    // Start is called before the first frame update
    private Card displayedCard;
    public GameObject bigCard;
    public GameObject stopButton;
    public GameObject exileButton;
    public GameObject miniExileButton;
    public GameObject CardDetails;

    public TextMeshProUGUI detailsNameText;
    public TextMeshProUGUI detailsSubtypeText;
    public TextMeshProUGUI detailsAbilityText;

    public TextMeshProUGUI attackText;
    public TextMeshProUGUI resourcesText;
    public TextMeshProUGUI forceText;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI subtypeText;
    public TextMeshProUGUI abilityText;
    public Image cardImage;
    public static CardDisplayer Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void DisplayCard(Card card)
    {
        if (card.currentAlliegance == CardGame.Instance.currentPlayer.faction)
        {
            exileButton.SetActive(true);
            Debug.Log(card.currentAlliegance);
            Debug.Log(card.currentLocation);
        }
        else
        {
            exileButton.SetActive(false);
        }
        displayedCard = card;
        attackText.text = card.baseAttack.ToString();
        resourcesText.text = card.baseResources.ToString();
        forceText.text = card.baseForce.ToString();
        nameText.text = card.cardName;
        costText.text = card.cost.ToString();
        cardImage.sprite = card.image;
        bigCard.SetActive(true);
    }

    public void DisplayMiniExileButton(CardBody card)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(card.transform.position);

        // Apply offset in screen space (pixels)
        screenPos += new Vector3(-85f, 50f, 0f);

        miniExileButton.transform.position = screenPos;
        miniExileButton.SetActive(true);
    }

    public void HideMiniExileButton()
    {
        miniExileButton.SetActive(false);
    }

    public void DisplayCardDetails(CardBody card)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(card.transform.position);

        detailsNameText.text = card.cardInfo.name;
        detailsSubtypeText.text = "";
        List<KeywordEnum> subtypes = card.cardInfo.subtypes;
        if (subtypes.Count >= 1)
        {
            detailsSubtypeText.text += subtypes[0];
        }
        if (card.cardInfo.subtypes.Count > 1)
        {
            for (int i = 1; i < subtypes.Count; i++)
            {
                detailsSubtypeText.text += (", " + subtypes[i]);
            }
        }
        detailsAbilityText.text = card.cardInfo.abilityDescription;

        // Apply offset in screen space (pixels)
        screenPos += new Vector3(350f, 0f, 0f);

        CardDetails.transform.position = screenPos;
        CardDetails.SetActive(true);
        if (card.cardInfo.currentAlliegance == CardGame.Instance.currentPlayer.faction)
        {
            Clicker.Instance.blockOfKeepHolding.transform.position = card.transform.position + new Vector3(-2.5f, 1.5f, 0f);
            DisplayMiniExileButton(card);
        }
    }

    public void HideCardDetails()
    {
        CardDetails.SetActive(false);
        HideMiniExileButton();
    }

    public void Exile()
    {
        if (displayedCard != null)
        {
            StartCoroutine(displayedCard.Exile());
            StopShowing();
        }
        else
        {
            HideCardDetails();
            StartCoroutine(Clicker.Instance.currentHoveredCard.cardInfo.Exile());
        }
    }

    public void StopShowing()
    {
        bigCard.SetActive(false);
        displayedCard = null;
    }
}
