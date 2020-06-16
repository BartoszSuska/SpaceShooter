using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingingAxe : MonoBehaviour
{
    float modifier = 50f;

    void Update()
    {
        Vector3 targetDirection = new Vector3(0f, 0f, 60f);

        if(transform.rotation.z >= 60f || transform.rotation.z <= -60f)
        {
            modifier *= -1f;
        }

        transform.Rotate(Vector3.forward * modifier * Time.deltaTime);    
    }
}
