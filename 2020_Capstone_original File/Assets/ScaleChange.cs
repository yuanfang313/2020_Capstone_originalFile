using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleChange : MonoBehaviour
{
    public Vector3 originalScale;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       if (Input.GetKey(KeyCode.A))
        {
            transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
        } else
        {
            transform.localScale = originalScale;
        }
    }
}
