using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Capital Ship")]
public class CapitalShip : Card
{
    [HideInInspector] public CardBody CapitalShipBodyPrefab;

    public void Transform()
    {
        Slot s = body.slot;
        Transform t = body.transform;
        Destroy(body.gameObject);
        CardBody newCard = Instantiate(CapitalShipBodyPrefab, t.transform.position, t.transform.rotation);
        body = newCard;
        newCard.Initialize(this);
        newCard.slot = s;
    }

    public override bool ValidPlay(CardZoneBase targetZone) //check if card can be played from hand
    {
        if (targetZone.ZoneType == CardZoneEnum.CapitalShipArea)
        {
            return body.currentZoneType == CardZoneEnum.Hand && CanBePlayed();
        }
        return false;
    }

    public override IEnumerator Play(CardZoneBase targetZone)
    {
        yield return base.Play(targetZone);
        Transform();
        body.currentZone = targetZone;
        body.currentZoneType = CardZoneEnum.CapitalShipArea; 
    }
}
