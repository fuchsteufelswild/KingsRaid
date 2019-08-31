using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelPrompt : MonoBehaviour
{
    public enum TravelType { TOWN, RAID }
    public int travelID;
    public TravelType type;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameManager.instance.travel = this;
            GameManager.instance.travelID = this.travelID;
            GameManager.instance.toTravel = true;
            CanvasScript.instance.travelPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameManager.instance.toTravel = false;
            CanvasScript.instance.travelPrompt.SetActive(false);
        }
    }
}
