using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHoveredEvents : MonoBehaviour
{
    public PointerEvents pointerEvents;
    public AudioSource audioSource_hover;
    public Vector3 hoveredScale = new Vector3(0.7f, 0.7f, 0.7f);
    private Vector3 originalScale = new Vector3(0.5f, 0.5f, 0.5f);

    private bool hadHit = false;
    private GameObject hittedObject;
    private GameObject previousHittedObject;

    private void Awake()
    {
        PointerEvents.OnPointerUpdateForObject += UpdateScaleOfObject;
        PointerEvents.OnPointerUpdateForObject += UpdateSound_hover;
    }

    private void OnDestroy()
    {
        PointerEvents.OnPointerUpdateForObject -= UpdateScaleOfObject;
        PointerEvents.OnPointerUpdateForObject -= UpdateSound_hover;
    }

    private GameObject getHittedObject()
    {
        hittedObject = pointerEvents.UpdatePointerStatus();
        previousHittedObject = hittedObject;
        return hittedObject;
    }
    
    // change the scale of the object when being hovered over by pointer
    private void UpdateScaleOfObject(GameObject hitObject)
    {
        if (hitObject)
        {
            getHittedObject().transform.localScale = hoveredScale;
        }
        else if (!hitObject)
        {
            previousHittedObject.transform.localScale = originalScale;
        }
    }

    //add sound when being hovered over by pointer
    private void UpdateSound_hover(GameObject hitObject)
    {
        if (hitObject && !hadHit)
        {
            audioSource_hover.Play();
            hadHit = true;
        }
        else if (!hitObject)
        {
            hadHit = false;
        }
    }
}
