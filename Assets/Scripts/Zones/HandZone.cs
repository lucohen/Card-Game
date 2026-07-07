using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandZone : CardZoneBase
{
    public override CardZoneEnum ZoneType => CardZoneEnum.Hand;


    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("card") && hand.GetComponent<Hand>().playing && other.GetComponent<CardBody>().isDragging)
        {
            if (other.GetComponent<CardBody>().cardInfo.ValidShopDrop(this))
            {
                ShowGhostSlot();
            }
        }
    }

}


