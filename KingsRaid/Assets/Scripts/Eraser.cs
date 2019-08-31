using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eraser : MonoBehaviour
{
    public float lifeTime;
    public float timeElapsed = 0.0f;

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed >= lifeTime)
            Destroy(this.gameObject);
    }
}
