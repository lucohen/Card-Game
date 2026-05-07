using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OuterRimPilotDeck : CardGroup
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnClick()
    {
        StartCoroutine(Click());
    }

    public IEnumerator Click()
    {
        Player player = CardGame.Instance.currentPlayer;
        Card card = deckList[0];
        if (card.cost <= player.resources)
        {
            player.LoseResources(card.cost);
            card.currentAlliegance = player.faction;
            CardBody newCard = Instantiate(card.bodyPrefab, transform.position, transform.rotation);
            card.body = newCard;
            newCard.Initialize(card);
            card.MoveInfo(player.discardPile);
            card.body.Move(player.discardPile.transform);
            card.body.GetComponent<BoxCollider>().enabled = false;
            while (card.body.isMoving)
            {
                yield return new WaitForEndOfFrame();
            }
            Destroy(card.body.gameObject);
        }
        else
        {
            Debug.Log("Not Enough Resources");
        }
    }
}
