using UnityEngine;

public class Slot : MonoBehaviour
{
    public GameObject ghostVisual;

    private void Awake()
    {
        ghostVisual.SetActive(true);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log("---------------------------");
    //    if (other.CompareTag("Card"))
    //    {
    //        ghostVisual.SetActive(true);
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Card"))
    //    {
    //        //ghostVisual.SetActive(false);
    //    }
    //}
}
