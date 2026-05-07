using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardZoneBase : MonoBehaviour
{
    public abstract CardZoneEnum ZoneType { get; }
    public GameObject frontPos;  //This is the deck the zone is next to, NOT the deck of cards in the zone.
    public CardGroup hand;
    protected List<Slot> slots = new List<Slot>();
    public Slot slotPrefab;
    public GameObject ghostSlot;
    public float spacing = 1f;
    public FactionEnum faction;

    private void Start()
    {
        ghostSlot.SetActive(false);
        
    }

    public virtual Slot CreateSlot()
    {
        Slot slot = Instantiate(slotPrefab, transform);

        Vector3 startPos = frontPos.transform.position + frontPos.transform.right * 3f;
        Vector3 offset = frontPos.transform.right * (spacing * slots.Count);

        slot.transform.position = startPos + offset; // WORLD space
        slots.Add(slot);

        return slot;
    }

    protected void ShowGhostSlot()
    {
        Vector3 startPos = frontPos.transform.position + frontPos.transform.right * 3f;
        Vector3 offset = frontPos.transform.right * (spacing * slots.Count);

        ghostSlot.transform.position = startPos + offset; // WORLD space
        ghostSlot.SetActive(true);
    }

    public void HideGhostSlot()
    {
        ghostSlot.SetActive(false);
    }


    public virtual void RemoveSlot(Slot slot)
    {
        slots.Remove(slot);
        Destroy(slot.gameObject);
        RepositionSlots();
    }

    protected void RepositionSlots()
    {
        Debug.Log("Reposition");
        for (int i = 0; i < slots.Count; i++)
        {
            Vector3 startPos = frontPos.transform.position + frontPos.transform.right * 3f;
            Vector3 offset = frontPos.transform.right * (spacing * i);
            slots[i].transform.position = startPos + offset;
            hand.deckList[i].body.Move(slots[i].transform);
            hand.deckList[i].body.slot = slots[i];
        }
    }

    
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("card"))
        {
            HideGhostSlot();
        }
    }

}

