using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddOutline : MonoBehaviour
{
    public GameObject OutlinedCube;

    private Outline outline;
    private Color setColor1 = new Vector4(0, 0.95f, 0.9f, 1);

    private void Awake()
    {
        PointerEvents.OnPointerUpdateForObject += SetOutline;
    }

    private void OnDestroy()
    {
        PointerEvents.OnPointerUpdateForObject -= SetOutline;
    }

    private void SetOutline(GameObject hitObject)
    {
        if (hitObject)
        {
            outline = hitObject.GetComponent<Outline>();
            outline.OutlineMode = Outline.Mode.OutlineVisible;
            outline.OutlineColor = setColor1;
            outline.OutlineWidth = 10.0f;
        }

    }
       
}
