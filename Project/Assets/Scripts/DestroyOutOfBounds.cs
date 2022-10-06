using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour
{
    private float rightBound = 110;
    private float leftBound = -70;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

            //If an object goes past the players view in the game, remove that object
            if (transform.position.z > rightBound)
            {
                Destroy(gameObject);
            }

            //If an object goes past the players view in the game, remove that object
            else if (transform.position.z < leftBound)
            {
                Destroy(gameObject);
            }

        

    }
}
