using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoPosition : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        Vector3 position = new Vector3(16.4209995f,0.476999998f,1.24300003f);
        transform.position = position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
