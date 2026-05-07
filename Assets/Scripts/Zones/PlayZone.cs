using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayZone : CardZoneBase
{
    public override CardZoneEnum ZoneType => CardZoneEnum.PlayArea;
    public Player player;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("card"))
        {
            if (other.GetComponent<CardBody>().cardInfo.ValidPlay(this) && !(other.GetComponent<CardBody>().cardInfo is CapitalShip))
            {
                ShowGhostSlot();
            }
        }
    }
}
