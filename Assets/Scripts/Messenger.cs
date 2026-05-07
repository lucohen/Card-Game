using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Messenger : MonoBehaviour
{

    public GameObject messageBox;
    public TextMeshProUGUI message;
    public static Messenger Instance;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update

    public IEnumerator DisplayMessage(string m)
    {
        message.text = m;
        messageBox.SetActive(true);
        yield return new WaitForSeconds(2);
        messageBox.SetActive(false);
    }
}
