using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMovementManager : MonoBehaviour
{
    public static CardMovementManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveToZone(CardZoneBase newZone, CardBody card)  //create a new slot in the zone being entered and move there. 
    {
        Slot newSlot = newZone.CreateSlot();
        card.slot = newSlot;
        card.currentZoneType = newZone.ZoneType;
        Debug.Log(newZone.ZoneType + " | " + card.currentZoneType);
        card.currentZone = newZone;
        Debug.Log(newZone + " | " + card.currentZone);
        card.Move(card.slot.transform);
    }

    public void MoveBetweenZones(CardZoneBase newZone, CardBody card) //For use ONLY when moving from one zone to another. Remove the old slot, then call MoveToZone
    {
        Slot oldSlot = card.slot;
        CardZoneBase oldZone = card.currentZone;
        MoveToZone(newZone, card);
        oldZone.RemoveSlot(oldSlot);
//        Debug.Log("--- " + newZone);
    }

    public IEnumerator MoveFromZone(Transform destination, CardBody card) //Remove card body from zone and destroy it when it reaches its destination
    {
        Slot oldSlot = card.slot;
        CardZoneBase oldZone = card.currentZone;
        oldZone.RemoveSlot(oldSlot);
        card.Move(destination);
        while (card.isMoving)
        {
            yield return new WaitForEndOfFrame();
        }
        Destroy(card.gameObject);
    }

    public bool PotentiallyDroppable(CardBody card, CardZoneBase targetZone)
    {
        return targetZone != null && targetZone.ZoneType != card.currentZoneType && targetZone.ZoneType != CardZoneEnum.Shop;
    }



}
