using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDrag : MonoBehaviour
{
    public float xOffset;
    public float yOffset;


    public void OnBeginDrag()
    {
        xOffset = this.transform.position.x - Input.mousePosition.x;
        yOffset = this.transform.position.y - Input.mousePosition.y;

    }

    public void OnDrag()
    {
        this.transform.position = new Vector3(xOffset + Input.mousePosition.x, yOffset + Input.mousePosition.y);
    }
}
