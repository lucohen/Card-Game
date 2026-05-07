using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForceBar : MonoBehaviour
{

    public Image[] notches = new Image[5];
    public int currentNotch;
    public static ForceBar Instance;

    private void Awake()
    {
        Instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        currentNotch = 2;
    }

    public void MoveBar(int amount)
    {
        if (CardGame.Instance.currentPlayer.faction == FactionEnum.Rebels)
        {
            for (int i = 0; i < amount; i++)
            {
                if (currentNotch < 4)
                {
                    notches[currentNotch].color = Color.white;
                    currentNotch += 1;
                }
                else
                {
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < amount; i++)
            {
                if (currentNotch > 0)
                {
                    notches[currentNotch].color = Color.white;
                    currentNotch -= 1;
                }
                else
                {
                    break;
                }
            }
        }
        SetCurrentNotchColor();
    }

    public void SetCurrentNotchColor()
    {
        if (currentNotch < 2)
        {
            notches[currentNotch].color = Color.blue;
        }
        else if (currentNotch > 2)
        {
            notches[currentNotch].color = Color.red;
        }
        else
        {
            notches[currentNotch].color = Color.black;
        }
    }
}
