using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddOutline : MonoBehaviour
{
    public GameObject OutlinedCube;

    private Outline outline;
    private Color setColor1 = new Vector4(0, 0.95f, 0.9f, 1);
    private Color setColor2 = new Vector4(1, 0, 0, 1);
    private bool outlineShow = false;
    private bool colorChange = false;

    void Start()
    {
        outline = OutlinedCube.GetComponent<Outline>();
        outline.OutlineMode = Outline.Mode.OutlineHidden;
        outline.OutlineColor = setColor1;
        outline.OutlineWidth = 10.0f;
    }

    void Update()
    {
        CheckKey();
        ChangeOutline();
        ChangeColor();
    }
    
    private void CheckKey()
    {
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.A))
        {
            outlineShow = true;
        }
        else if (!Input.GetKey(KeyCode.Space))
        {
            outlineShow = false;
        }
    }

    private void ChangeOutline()
    {
        if(outlineShow)
        {
            outline.OutlineMode = Outline.Mode.OutlineVisible;
        } else if (!outlineShow)
        {
            outline.OutlineMode = Outline.Mode.OutlineHidden;
        }
    }

    private void ChangeColor()
    {
        
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.Space))
        {
            outline.OutlineColor = setColor2;
        }
        else
        {
            outline.OutlineColor = setColor1;
        }
    }
    
}
