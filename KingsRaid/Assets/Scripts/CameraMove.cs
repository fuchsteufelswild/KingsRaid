using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(!Character.instance.started)
            Camera.main.transform.position = Vector3.Lerp(this.transform.position, new Vector3(this.transform.position.x, -3.1f, this.transform.position.z), Time.deltaTime * 2);
        if (Camera.main.transform.position.y < -3.07f && Camera.main.transform.position.y > -3.15f)
            Character.instance.started = true;
    }
}
