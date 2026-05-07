using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardBody : MonoBehaviour
{
    [HideInInspector] public Vector3 originalScale;
    [HideInInspector] public bool isHovered;

    public TextMeshProUGUI attackText;
    public TextMeshProUGUI resourcesText;
    public TextMeshProUGUI forceText;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI costText;
    public Image cardImage;
    private Transform target;
    private float moveSpeed;
    public CardZoneEnum currentZoneType;
    public CardZoneBase currentZone;
    public Slot slot;
    //public Camera cam;
    public float dragHeight = 2f;
    [HideInInspector] public Card cardInfo;
    [HideInInspector] public bool isDragging = false;
    [HideInInspector] public bool isMoving =false;
    private LineRenderer line;
    // Start is called before the first frame update
    void Start()
    {
        
    }



    void Update()
    {
        if (line != null)
        {
            line.SetPosition(0, transform.position);
        }

        // Smooth movement
        if (isMoving)
        {
            float distance = Vector3.Distance(transform.position, target.position);

            // Speed increases with distance
            float dynamicSpeed = Mathf.Clamp(moveSpeed * distance, moveSpeed, moveSpeed * 5f);

            transform.position = Vector3.MoveTowards(
                transform.position,
                target.position,
                dynamicSpeed * Time.deltaTime
            );

            if (distance < 0.01f)
            {
                transform.position = target.position;
                isMoving = false;
            }
        }
        if (isDragging || isMoving || slot == null) return;

        Vector3 targetScale = originalScale;
        Vector3 targetPosition = slot.transform.position;

        if (isHovered)
        {
            targetScale = originalScale * Clicker.Instance.hoverScaleMultiplier;
            targetPosition = slot.transform.position + Vector3.up * Clicker.Instance.hoverLift;
        }

        transform.localScale = Vector3.Lerp(
            transform.localScale,
            targetScale,
            Time.deltaTime * Clicker.Instance.hoverSmoothSpeed
        );

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            Time.deltaTime * Clicker.Instance.hoverSmoothSpeed
        );
    }

    public void Initialize(Card info)
    {
        originalScale = transform.localScale;
        cardInfo = info;
        attackText.text = cardInfo.baseAttack.ToString();
        resourcesText.text = cardInfo.baseResources.ToString();
        forceText.text = cardInfo.baseForce.ToString();
        nameText.text = cardInfo.cardName;
        costText.text = cardInfo.cost.ToString();
        cardImage.sprite = cardInfo.image;
        moveSpeed = 20f;
        SetCardColor();
        cardInfo.ResetCard();

    }

    public void Move(Transform transform) //physically moves the card to a new position
    {
        isDragging = false;
        target = transform;
        isMoving = true;
    }

    public void MoveWithSlot(Transform transform)
    {
        slot.transform.position = transform.position;
        transform.position = transform.position;

    }

    public void Drop() //check the zone under the card. If it's a valid place to drop it, move the card to that zone and remove the old slot
    {
        if (cardInfo.currentLocation is PlayArea)
        {
            GameObject targetCard = DetectCardUnderCard();
            if (targetCard != null)
            {
                if (cardInfo.ValidAttack(targetCard))
                {
                    Debug.Log(targetCard);

                    // Commit attack logic
                    cardInfo.CommitToAttack(targetCard);

                    // Draw line between this card and the target
                    DrawLine(targetCard);

                    Move(slot.transform);
                    return;
                }
            }
        }
        CardZoneBase targetZone = DetectZoneUnderCard();
        if (CardMovementManager.Instance.PotentiallyDroppable(this, targetZone))
        {
            if (cardInfo.ValidShopDrop(targetZone)) //card is from shop (purchases if valid)
            {
                CardGame.Instance.currentPlayer.PurchaseCard(cardInfo);

            }
            else if (cardInfo.ValidPlay(targetZone)) //card is being played from hand
            {
                StartCoroutine(cardInfo.Play(targetZone));
            }
            else
            {
                Move(slot.transform); // snap back
            }

        }
        else
        {
            Move(slot.transform); // snap back
        }
    }

    public CardZoneBase DetectZoneUnderCard()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, 1f);
        foreach (Collider hit in hits)
        {
            CardZoneBase zone = hit.GetComponentInParent<CardZoneBase>();
            if (zone != null)
                zone.HideGhostSlot();
                return zone;
        }
        return null;
    }

    public void Exile()
    {
        StartCoroutine(CardMovementManager.Instance.MoveFromZone(transform, this));
        Destroy(gameObject);
    }

    public void OnClick()
    {
        Debug.Log(cardInfo.currentLocation + " | " + cardInfo.cardName + " | " + cardInfo.GetCardID());
        Debug.Log(cardInfo is CapitalShip);
        CardDisplayer.Instance.DisplayCard(cardInfo);
    }

    public void OnHold()
    {

    }

    GameObject DetectCardUnderCard()
    {
        Collider[] hits = Physics.OverlapBox(
        gameObject.GetComponent<Collider>().bounds.center,
        gameObject.GetComponent<Collider>().bounds.size);
        GameObject bestTarget = null;
        float bestOverlap = 0f;

        foreach (Collider hit in hits)
        {
            Debug.Log(hit);
            CardBody card = hit.GetComponent<CardBody>();
            BaseBody hitBase = hit.GetComponent<BaseBody>();

            if (card != null && card != this)
            {

                float overlap = GetOverlapArea(gameObject.GetComponent<Collider>().bounds, hit.bounds);

                if (overlap > bestOverlap)
                {
                    bestOverlap = overlap;
                    bestTarget = card.gameObject;
                }
            }

            if (hitBase != null)
            {

                float overlap = GetOverlapArea(gameObject.GetComponent<Collider>().bounds, hit.bounds);

                if (overlap > bestOverlap)
                {
                    bestOverlap = overlap;
                    bestTarget = hitBase.gameObject;
                }
            }
        }
        return bestTarget;
    }

    float GetOverlapArea(Bounds a, Bounds b)
    {
        float overlapX = Mathf.Max(0, Mathf.Min(a.max.x, b.max.x) - Mathf.Max(a.min.x, b.min.x));
        float overlapY = Mathf.Max(0, Mathf.Min(a.max.y, b.max.y) - Mathf.Max(a.min.y, b.min.y));

        return overlapX * overlapY;
    }

    public void DrawLine(GameObject targetCard)   //testing purposes
    {
        // Draw line between this card and the target
        line = GetComponent<LineRenderer>();
        if (line == null)
        {
            line = gameObject.AddComponent<LineRenderer>();
        }

        // Configure line (you can tweak these)
        line.positionCount = 2;
        line.startWidth = 0.05f;
        line.endWidth = 0.05f;

        // Set positions
        line.SetPosition(0, transform.position);              // attacking card
        line.SetPosition(1, targetCard.transform.position);   // target card

        // Optional: make sure it's visible
        line.enabled = true;
    }

    

    public void SetCardColor()      //testing purposes
    {
        if (cardInfo.faction == FactionEnum.Rebels)
        {
            GetComponent<SpriteRenderer>().color = Color.red;
        }
        else if (cardInfo.faction == FactionEnum.Neutral)
        {
            GetComponent<SpriteRenderer>().color = Color.gray;
        }
        else if (cardInfo.faction == FactionEnum.Empire)
        {
            GetComponent<SpriteRenderer>().color = Color.blue;
        }
    }


    public void View()
    {

    }

}
