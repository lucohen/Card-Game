using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Bases/Base")]
public class Base : ScriptableObject
{
    public int maxHp;
    [HideInInspector] public int hp;
    public string baseName;
    [TextArea] public string abilityDescription;
    public FactionEnum faction;
    [HideInInspector] public BaseBody body;
    public List<EffectEntry> onRevealEffects;
    public List<ReactionEntry> reactions;
    public bool keepAbilityDuringOpponentTurn;
    public BaseBody bodyPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        Debug.Log("Took " + damage + " damage, " + hp + " hp remaining");
        body.hpText.text = hp.ToString();
        if (hp <= 0)
        {
            BaseDestroyed();
        }
    }

    public void BaseDestroyed()
    {
        Debug.Log("Base Destroyed");
        Destroy(body.gameObject);
        if (faction == FactionEnum.Rebels)
        {
            CardGame.Instance.rebelPlayer.DisplayBases();
        }
        else if (faction == FactionEnum.Empire)
        {
            CardGame.Instance.empirePlayer.DisplayBases();
        }
    }
}
