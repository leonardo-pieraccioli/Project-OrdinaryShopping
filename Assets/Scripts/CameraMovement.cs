using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Camera c;
    // Start is called before the first frame update
    void Start()
    {
        c = Camera.main;
    }

    void FixedUpdate()
    {
        if(c.fieldOfView < 65)
        {
           c.fieldOfView += 0.1f;
        }
        Vector3 angles = transform.eulerAngles;
        if(angles.x < 50 || angles.x > 354) //360 - 6 = 354
        {
            angles.x -= 0.03f;
            transform.eulerAngles = angles;
        }   
    }


    void Update()
    {
        
    }
}
