using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameContller : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Transform myTransform = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            transform.position = new Vector3(0.7f,6.54f,-1.37f);
            transform.rotation = Quaternion.Euler(56.0f,0.0f,0.0f);
        }
    }
}
