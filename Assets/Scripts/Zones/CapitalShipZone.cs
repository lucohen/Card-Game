using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapitalShipZone : CardZoneBase
{
    public override CardZoneEnum ZoneType => CardZoneEnum.CapitalShipArea;
    public Player player;
    

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("card"))
        {
            Debug.Log(ZoneType);
            if (other.GetComponent<CardBody>().cardInfo.ValidPlay(this) && other.GetComponent<CardBody>().cardInfo is CapitalShip)
            {
                ShowGhostSlot();
            }
        }
    }
}
