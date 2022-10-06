using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    public float speed = 40.0f;

    // Start is called before the first frame update
    void Start()
    {
        // Can do here or Edit->Project Settings->Physics->Collision Matrix and remove the check box for Cars-Powerup
        // Physics.IgnoreLayerCollision(3, 6);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.forward);
    }
}
