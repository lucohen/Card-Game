using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Clicker : MonoBehaviour
{
    private GameObject selectedObject;
    private Vector3 mouseDownPos;
    private bool isDragging;
    private bool isHolding;
    private bool doneHolding;
    public GameObject blockOfKeepHolding;
    public Image fillBar;
    public float chargeSpeed = 0.5f; // Change in inspector
    private float currentCharge = 0f;
    public float clickThreshold = 10f; // pixels
    [HideInInspector] public CardBody currentHoveredCard;

    public float hoverScaleMultiplier = 1.2f;
    public float hoverLift = 0.3f;
    public float hoverSmoothSpeed = 10f;
    public static Clicker Instance;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (!isDragging)
        {
            HandleHover();
        }
        // MOUSE DOWN
        if (Input.GetMouseButtonDown(0))
        {
            if (CanClick())
            {
                mouseDownPos = Input.mousePosition;
                RaycastHit hit = CastRay();
                if (hit.collider != null)
                {
                    if (hit.collider.CompareTag("card"))
                    {
                        selectedObject = hit.collider.gameObject;
                        isDragging = false;
                        
                        fillBar.transform.position = mouseDownPos;
                        isHolding = true;
                        
                        CardDisplayer.Instance.HideCardDetails();
                    }
                    if (!ActionManager.Instance.HasPendingActions)
                    {
                        if (hit.collider.CompareTag("deck"))
                        {
                            hit.collider.GetComponent<Deck>().OnDeckClicked();
                        }
                        if (hit.collider.CompareTag("base"))
                        {
                            hit.collider.GetComponent<BaseBody>().OnBaseClicked();
                        }
                    }
                }
            
            }
        }

        // MOUSE HELD
        if (Input.GetMouseButton(0) && selectedObject != null && !ActionManager.Instance.HasPendingActions)
        {
            if (Vector3.Distance(Input.mousePosition, mouseDownPos) > clickThreshold)
            {
                isDragging = true;
                selectedObject.GetComponent<CardBody>().isDragging = true;
                DragSelectedObject();
                currentCharge = 0;
            }
            else if (currentCharge < 1 && isHolding)
            {
                currentCharge = Mathf.Clamp01(currentCharge + Time.deltaTime * chargeSpeed);
            }
            else if (isHolding)
            {
                doneHolding = true;
                selectedObject.GetComponent<CardBody>().OnHold();
                selectedObject = null;
                currentCharge = 0;
            }
            fillBar.fillAmount = currentCharge;
        }

        // MOUSE UP
        if (Input.GetMouseButtonUp(0) && selectedObject != null)
        {
            currentCharge = 0;
            if (isDragging)
            {
                // DRAG RELEASE
                selectedObject.GetComponent<CardBody>().Drop();
            }
            else if (doneHolding)
            {
                doneHolding = false;
            }
            else
            {
                // CLICK

                selectedObject.GetComponent<CardBody>().OnClick();
            }

            selectedObject = null;
            isDragging = false;
        }
    }

    private void DragSelectedObject()
    {
        Vector3 position = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.WorldToScreenPoint(selectedObject.transform.position).z
        );

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
        selectedObject.transform.position = new Vector3(
            worldPosition.x,
            worldPosition.y + 0.25f,
            worldPosition.z
        );
    }

    private RaycastHit CastRay()
    {
        Vector3 screenMousePosFar = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.farClipPlane
        );

        Vector3 screenMousePosNear = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.nearClipPlane
        );

        Vector3 worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);
        Vector3 worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);

        RaycastHit hit;
        Physics.Raycast(
            worldMousePosNear,
            worldMousePosFar - worldMousePosNear,
            out hit
        );

        return hit;
    }

    private bool CanClick()
    {
        CardBody[] cards = Object.FindObjectsOfType<CardBody>();
        foreach (CardBody card in cards)
        {
            if (card.isMoving)
            {
                return false;
            }
        }
        return true;
    }

    private void HandleHover()
    {
        RaycastHit hit = CastRay();
        CardBody newHoveredCard = null;

        if (hit.collider != null && hit.collider.CompareTag("card"))
        {
            newHoveredCard = hit.collider.GetComponent<CardBody>();
        }
        if (hit.collider != null && hit.collider.CompareTag("keepHolding"))
        {
            newHoveredCard = currentHoveredCard;
        }
        // If hovered card changed
        if (newHoveredCard != currentHoveredCard)
        {
            // Exit old
            if (currentHoveredCard != null)
            {
                OnHoverExit(currentHoveredCard);
            }

            // Enter new
            if (newHoveredCard != null)
            {
                OnHoverEnter(newHoveredCard);
            }

            currentHoveredCard = newHoveredCard;
        }
    }

    private void OnHoverEnter(CardBody card)
    {
        card.isHovered = true;
        
        CardDisplayer.Instance.DisplayCardDetails(card);
    }

    private void OnHoverExit(CardBody card)
    {
        
        card.isHovered = false;
        CardDisplayer.Instance.HideCardDetails();
        
    }

}

