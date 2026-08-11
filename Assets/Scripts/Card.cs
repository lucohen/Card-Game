using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[CreateAssetMenu(menuName = "Cards/Card")]
public class Card : ScriptableObject
{

    [HideInInspector] public CardBody body;
    [HideInInspector] public CardGroup currentLocation;
    [HideInInspector] public CardBody bodyPrefab;
    public List<EffectEntry> onPlayEffects;
    public List<EffectEntry> Abilities;
    public List<EffectEntry> onExileEffects;
    public List<ReactionEntry> reactions;
    public int baseAttack;
    public int baseResources;
    public int baseForce;
    public int hp;
    public List<KeywordEnum> subtypes;
    [HideInInspector] public int attack;
    [HideInInspector] public int resources;
    [HideInInspector] public int force;
    public string abilityDescription;
    public int[] rewards; //two ints, first is resources, second is force
    public string cardName;
    public Sprite image;
    public int cost;
    private int cardID;
    [HideInInspector] public bool hasActivated = false;
    [HideInInspector] public bool hasAttacked = false;
    public bool addToHandFromShop;
    public FactionEnum faction;
    [HideInInspector] public FactionEnum currentAlliegance;
    private GameObject targetToAttack;


    private void Awake()
    {
        currentAlliegance = faction;
    }


    public void MoveInfo(CardGroup deck)         //Register the card in another deck/hand 
    {
        currentLocation.MoveCard(deck, this);
    }

    public void assignCardID(int i)
    {
        cardID = i;
    }

    public int GetCardID()
    {
        return cardID;
    }

    public bool CanBePlayed()
    {
        if (currentLocation.GetComponent<Hand>() != null)
        {
            if (!hasActivated && currentLocation.GetComponent<Hand>().playing)
            {
                return true;
            }
        }
        return false;
    }

    public virtual IEnumerator Play(CardZoneBase targetZone)
    {
        if (!hasActivated)
        {
            ActivateOnPlayAbilities();
            yield return new WaitForFixedUpdate();
            Debug.Log("Resources: " + resources);
            CardGame.Instance.currentPlayer.GainResources(resources);
            MoveInfo(targetZone.hand);
            CardMovementManager.Instance.MoveBetweenZones(targetZone, body);
            ForceBar.Instance.MoveBar(baseForce);
            CardGame.Instance.RegisterReactions(reactions);
        }
        else
        {
            Debug.Log("Card already played");
            body.Move(body.slot.transform);
        }
    }

    public void ActivateAbility()
    {
        if (!hasActivated && CanActivateAbility())
        {
            Debug.Log("Activate");
            foreach (EffectEntry e in Abilities)
            {
                e.effect.Resolve(e.data);
            }
            hasActivated = true;
            SpriteRenderer sr = body.GetComponent<SpriteRenderer>();
            // Sets opacity to 50% (0.5f) while keeping RGB values at 1 (white/no tint)
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.5f);
        }
        else
        {
            Debug.Log("Card already played");
        }
    }



    public void CommitToAttack(GameObject attackedCard)
    {
        body.slot.gameObject.SetActive(true);
        targetToAttack = attackedCard;
    }


    public bool ValidShopDrop(CardZoneBase targetZone) // check if card can be purchased from shop
    {
        if (targetZone.ZoneType == CardZoneEnum.Hand)
        {
            Debug.Log(body.currentZoneType);
            return (faction == targetZone.faction || faction == FactionEnum.Neutral)
                && body.currentZoneType == CardZoneEnum.Shop && targetZone.hand.GetComponent<Hand>().playing;
        }
        return false;
    }

    public virtual bool ValidPlay(CardZoneBase targetZone) //check if card can be played from hand
    {
        if (targetZone.ZoneType == CardZoneEnum.PlayArea)
        {
            return body.currentZoneType == CardZoneEnum.Hand && CanBePlayed();
        }
        return false;
    }

    public bool ValidAttack(GameObject toAttack)
    {
        if (hasAttacked)
        {
            return false;
        }
        if (toAttack.GetComponent<BaseBody>() != null)
        {
            if (toAttack.GetComponent<BaseBody>().baseInfo.faction != faction)
            {
                if (CardGame.Instance.currentPlayer.opponent.NumCapitalShips() == 0)
                {
                    return true;
                }
                else
                {
                    Messenger.Instance.StartCoroutine(Messenger.Instance.DisplayMessage("Capital Ships Remain"));
                }
            }
        }
        else if (toAttack.GetComponent<CardBody>() != null)
        {
            Card card = toAttack.GetComponent<CardBody>().cardInfo;
            return card.faction != faction && (card.body.currentZoneType == CardZoneEnum.Shop || card.body.currentZoneType == CardZoneEnum.CapitalShipArea);
        }
        return false;
    }

    public bool MatchesPlayerFaction(Player player)
    {
        return (faction == FactionEnum.Neutral || faction == player.faction);
    }

    //public bool ValidReturn(CardZoneBase targetZone) // Check if card can be returned from play area to hand
    //{
    //    return body.currentZoneType == CardZone.PlayArea && targetZone.hand.GetComponent<Hand>().playing && body.isDragging;
    //}

    public void Attack()
    {
        if (!hasAttacked)
        {
            if (targetToAttack.GetComponent<BaseBody>() != null)
            {
                targetToAttack.GetComponent<BaseBody>().baseInfo.TakeDamage(attack);
            }
            else if (targetToAttack.GetComponent<CardBody>() != null)
            {
                targetToAttack.GetComponent<CardBody>().cardInfo.TakeDamage(attack);
            }
            hasAttacked = true;
            body.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, .5f);
            targetToAttack = null;
            Destroy(body.GetComponent<LineRenderer>());
        }

    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Ow x " + damage);
    }

    public void ResetCard()
    {
        hasAttacked = false;
        hasActivated = false;
        attack = baseAttack;
        resources = baseResources;
        force = baseForce;
        body.SetCardColor();
    }


    public IEnumerator ExileRoutine()
    {
        Debug.Log("Exile");
        currentLocation.RemoveCard(cardID);
        if (body != null && body.currentZoneType != CardZoneEnum.None)
        {
            body.Exile();
        }
        if (currentAlliegance != FactionEnum.Neutral)
            ActivateOnExileAbilities();
        yield return new WaitForFixedUpdate();
        CardDatabase.Instance.Exile(this);
    }



    public bool CanActivateAbility()
    {
        return (currentLocation is PlayArea && !hasActivated);
    }

    public void IncreaseStat(int amount, int type) //for type, 0 is attack, 1 is resources, 2 is force
    {
        if (type == 0)
        {
            attack += amount;
            body.attackText.text = attack.ToString();
        }
        else if (type == 1)
        {
            resources += amount;
            body.resourcesText.text = resources.ToString();
        }
        if (type == 2)
        {
            force += amount;
            body.forceText.text = force.ToString();
        }
    }

    public void ActivateOnPlayAbilities()
    {
        if (onPlayEffects.Count > 0)
        {
            foreach (EffectEntry e in onPlayEffects)
            {
                e.effect.Resolve(e.data);
            }
        }
    }

    public void ActivateOnExileAbilities()
    {
        if (onExileEffects.Count > 0)
        {
            foreach (EffectEntry e in onExileEffects)
            {
                e.effect.Resolve(e.data);
            }
        }
    }



}


