using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BaseBody : MonoBehaviour
{


    public TextMeshProUGUI hpText;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI abilityText;
    [HideInInspector] public Base baseInfo;
    [HideInInspector] public bool beingDisplayed;
    public BaseDisplayer displayer;

    public void Initialize(Base info)
    {
        baseInfo = info;
        hpText.text = baseInfo.hp.ToString();
        nameText.text = baseInfo.baseName;
        abilityText.text = baseInfo.abilityDescription;
        beingDisplayed = false;
        displayer = BaseDisplayer.Instance;
    }

    private void OnDisable()
    {
        CardGame.Instance.UnregisterReactions(baseInfo.reactions);
    }

    public void OnBaseClicked()
    {
        if (displayer.displaying)
        {
            displayer.Confirmation(this);
        }
    }
}
