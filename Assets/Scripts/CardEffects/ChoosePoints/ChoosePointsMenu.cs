using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ChoosePointsMenu : MonoBehaviour

{
    private int points; //only use for AllIn type
    private int clicksAllowed;
    [HideInInspector]
    public bool isDistributed;
    public TextMeshProUGUI clicksLeft;
    public GameObject chooseScreen;
    public GameObject attackButton;
    public GameObject resourceButton;
    public GameObject forceButton;
    [HideInInspector] Card card;
    public bool[] options = new bool[3];
    public static ChoosePointsMenu Instance;

    private void Awake()
    {
        Instance = this;
    }
    public void Resolve(ChooseEffectData data)
    {
        clicksAllowed = data.clicksAllowed;
        points = data.pointsAvailable;
        card = data.card;
        isDistributed = (data.type == ChooseEffectData.Type.Distributed);
        chooseScreen.SetActive(true);
        options = data.options;
        if (options[0])
        {
            attackButton.SetActive(true);
        }
        else
        {
            attackButton.SetActive(false);
        }
        if (options[1])
        {
            resourceButton.SetActive(true);
        }
        else
        {
            resourceButton.SetActive(false);
        }
        if (options[2])
        {
            forceButton.SetActive(true);
        }
        else
        {
            forceButton.SetActive(false);
        }
    }

    public void StatButtonClicked(int t)
    {
        clicksAllowed -= 1;
        if (!isDistributed)
        {
            card.IncreaseStat(points, t);
            Debug.Log("Increased resources to " + card.resources);
            if (t == 0)
            {
                attackButton.SetActive(false);
            }
            else if (t == 1)
            {
                resourceButton.SetActive(false);
            }
            else if (t == 2)
            {
                forceButton.SetActive(false);
            }
        }
        else
        {
            card.IncreaseStat(1, t);
        }
        clicksLeft.text = clicksAllowed.ToString();
        if (clicksAllowed <= 0)
        {
            Time.timeScale = 1f;
            chooseScreen.SetActive(false);
        }
    }
}



