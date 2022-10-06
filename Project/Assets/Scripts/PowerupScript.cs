using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupScript : MonoBehaviour
{
    Vector3 startPos;
    public float speed = 0.02f;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = startPos +
            new Vector3(0, 0.2f * Mathf.Sin(speed * Time.frameCount) , 0);
    }
}
