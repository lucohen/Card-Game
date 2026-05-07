using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayArea : CardGroup
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator DiscardAll(Deck discardPile)
    {
        while (deckList.Count > 0)
        {
            Discard(deckList[^1], discardPile);
            yield return new WaitForSeconds(0.3f);
        }
    }
    public void Discard(Card card, Deck discardPile)
    {
        MoveCard(discardPile, card);
        StartCoroutine(CardMovementManager.Instance.MoveFromZone(discardPile.transform, card.body));

    }
}
